using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect.Wpf.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;
using System.ComponentModel;
using Microsoft.Kinect.Face;
using System.Globalization;
using System.IO;
using Project_Illuminate_UI.Pages;
using Microsoft.ServiceBus.Notifications;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Project_Illuminate_UI
{

    public delegate void SpeechEvent(string command);
    public partial class App : Application
    {
        
        public static Frame NavigationFrame { get; set; }
        internal KinectRegion KinectRegion { get; set; }
        static Entities db = new Entities();

        #region vars
        private static KinectSensor kinectSensor = null;
        private static BodyFrameReader bodyFrameReader = null;
        private static BodyFrameSource bodySource = null;
        private static BodyFrameReader bodyReader = null;
        private static HighDefinitionFaceFrameSource highDefinitionFaceFrameSource = null;
        private static HighDefinitionFaceFrameReader highDefinitionFaceFrameReader = null;
        private static FaceAlignment currentFaceAlignment = null;
        private static FaceModel currentFaceModel = null;
        private static Body currentTrackedBody = null;
        private static ulong currentTrackingId = 0;

        //array of bodies
        private static Body[] bodies = null;

        //100 frame counter for body learning
        public static int learnintervalbody { get; set; }
        //100 frame counter for face learning
        public static int learnintervalface { get; set; }
        //100 frame array to recognise individual bodies in view
        static int[] recintervalbody=new int[6];
        //100 frame counter for facial recognition
        static int recintervalface;
        //height value to be saved in database
        public static double recorderdHeight { get; set; }
        //height value used for recognition
        public static double[] comparisionHeight = new double[6];
        //message to display after body recognition
        public static string bodyResult { get; set; }
        //increment value for face rotation
        private const double FaceRotationIncrementInDegrees = 5.0;
        //array for face vertices(2000)
        static IReadOnlyList<CameraSpacePoint> vertices;
        //array to map points used in angle calculation for face
        static CameraSpacePoint[] colorPoint;
        //array to save face angles
        static double[] angles;
        //result of face recognition
        public static bool faceResult { get; set; }
        //signal valid face recognition result
        public static bool okToGoface { get; set; }
        //signal valid body recognition results(for the individual bodies in view)
        public static bool[] okToGoBody = new bool[6];
        //counter to tell how many bodies have been scanned for 100 frames
        public static int okToGoBodyGlobalCounter { get; set; }
        //signal valid body recognition for all bodies in view
        public static bool okToGoBodyGlobal { get; set; }
        //window console for head alignment
        public static Window headAlignmentWindow;
        public static TextBox temporaryConsole;
        //list of users in the database
        public static List<User> userlist;
        //counter of bodies inv iew
        public static int bodiesInSight { get; set; }
        //individual counters for every body in view
        public static bool[] initCounters = new bool[6];
        //signals user not recognized in body recognition
        public static bool userNotRec { get; set; }

        public static bool okToGoBodyLearn { get; set; }


        #endregion
        //open the kinect
        public static void OpenKinect()
        {
            kinectSensor = KinectSensor.GetDefault();
            kinectSensor.Open();
        }
        //read users from the database
        public static void ReadInUsers()
        {
            userlist = new List<User>();
            var users = from u in db.Users
                        select u;
            foreach (var user in users)
            {
                userlist.Add(user);
            }
        }
        //create the console window for head alignment
        public static void CreateHeadAlignmentWindow()
        {
            headAlignmentWindow = new Window();
            headAlignmentWindow.Width = 200;
            headAlignmentWindow.Height = 200;
            headAlignmentWindow.Background = new SolidColorBrush(Colors.Black);
            temporaryConsole = new TextBox();
            headAlignmentWindow.Content = temporaryConsole;
            headAlignmentWindow.Show();
        }
        //initialize the body recognition variables
        public static void InitializeBody()
        {
            //read in the users from databse
            ReadInUsers();
            //assign initial values to the variables
            for (int i = 0; i < initCounters.Length; i++)
            {
                okToGoBody[i] = false;
                comparisionHeight[i] = 0;
                recintervalbody[i] = 0;
                initCounters[i] = false;
            }
            okToGoBodyGlobalCounter = 0;
            okToGoBodyGlobal = false;
            recorderdHeight = 0;
            userNotRec = true;
            bodyResult = "";
            okToGoBodyLearn = false;
            //open the body reader
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            //assign frame arrived event
            if (bodyFrameReader != null)
            {
                bodyFrameReader.FrameArrived += Reader_FrameArrived;
            }
        }

        private static void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            //if data is received
            bool dataReceived = false;

            //use the frame from the sensor
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                //if frame is valid
                if (bodyFrame != null)
                {
                    //if the bodies array is empty
                    if (bodies == null)
                    {
                        //initialize body array
                        bodies = new Body[bodyFrame.BodyCount];
                    }
                    //refresh body data
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                    //count the bodies in view
                    bodiesInSight = bodies.Count(x => x.IsTracked);
                }
            }

            //if the data is received
            if (dataReceived)
            {
                //loop through the bodies array
                for (int i = 0; i < bodies.Length;i++ )
                {
                    //if the current body in the array is tracked
                    if (bodies[i].IsTracked)
                    {
                        //if the body has not yet started recognition cycle
                        if (initCounters[i] == false)
                        {
                            //begin recognition cycle
                            initCounters[i] = true;
                            //set recognition number of frames to 100
                            recintervalbody[i] = 100;
                        }
                        //grab the body joints
                        IReadOnlyDictionary<JointType, Joint> joints = bodies[i].Joints;

                        //check if the head and feet are tracked(visible to the sensor)
                        if (joints[JointType.Head].TrackingState == TrackingState.Tracked &&
                            joints[JointType.FootLeft].TrackingState == TrackingState.Tracked
                            && joints[JointType.FootRight].TrackingState == TrackingState.Tracked)
                        {
                            //calculate the height for the body
                            double height = Math.Round(GetHeight(bodies[i]),2);
                            //add it up to the height variable
                            recorderdHeight += height;

                            //if in learning cycle(100 frames)
                            if (learnintervalbody >= 0)
                            {
                                //decrease learning counter
                                learnintervalbody--;
                                //when learning at 0 frames
                                if (learnintervalbody == 0)
                                {
                                    //divide by number of frames
                                    recorderdHeight = recorderdHeight/100;
                                    //disable learning
                                    learnintervalbody = -1;
                                    //close height
                                    CloseHeight();
                                    //signal valid data
                                    okToGoBodyLearn = true;
                                    return;
                                }
                            }
                            //else if in recognition cycle(for every body in view)
                            else if (recintervalbody[i] >= 0)
                            {
                                //add height to the current person in the array
                                comparisionHeight[i] += height;
                                //decrease specific recognition counter
                                recintervalbody[i]--;

                                //when recognition at 0 frames for current body
                                if (recintervalbody[i] == 0)
                                {
                                    //divide by number of frames
                                    comparisionHeight[i] = comparisionHeight[i] / 100;
                                    comparisionHeight[i] = Math.Round(comparisionHeight[i], 2);
                                    //check the height against every user in the database
                                    foreach (var user in userlist)
                                    {
                                        //check against threshold
                                        if (Math.Abs(comparisionHeight[i] - user.Height) <= 0.012)
                                        {
                                            //user was recognised
                                            bodyResult += comparisionHeight[i]+" Hi, " + user.Name + " !";
                                            userlist.Remove(user);//avoid duplicate recognition
                                            userNotRec = false;
                                            break;
                                        }
                                    }
                                    //if any user was not recognised, send notification
                                    if (userNotRec==true)
                                    {
                                        bodyResult +=comparisionHeight[i]+" User not Recognised";
                                        NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://voldemtrial2-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=5gnvGy9hmygDZDy5HeofpYGP8GmVzYeakykypntS81s=", "finaltrial1");

                                        var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Unknown Person inside your home, please take action.</text></binding></visual></toast>";

                                        hub.SendWindowsNativeNotificationAsync(toast).Wait();
                                    }
                                    //signal valid data for current body
                                    okToGoBody[i] = true;
                                    for (int k = 0; k < okToGoBody.Length; k++)
                                    {
                                        //count how many bodies in view have valid data
                                        if (okToGoBody[k] == true)
                                            okToGoBodyGlobalCounter++;
                                    }
                                    //if all bodies in view have valid data
                                    if (okToGoBodyGlobalCounter == bodiesInSight)
                                    {
                                        //signal global valid
                                        okToGoBodyGlobal = true;
                                        CloseHeight();
                                    }
                                    else
                                        okToGoBodyGlobalCounter = 0;
                                   
                                }
                            }
                        }//end tracking if
                    }
                }//end for each
            }
        }
        //calculate height
        private static double GetHeight(Body bo)
        {

            var headpoint = bo.Joints[JointType.Head].Position;
            var leftFoot = bo.Joints[JointType.FootLeft].Position;
            var rightFoot = bo.Joints[JointType.FootRight].Position;

            CameraSpacePoint feetMiddlePoint = new CameraSpacePoint()
            {
                X = (leftFoot.X + rightFoot.X) / 2,
                Y = (leftFoot.Y + rightFoot.Y) / 2,
                Z = (leftFoot.Z + rightFoot.Z) / 2
            };
            double res = headpoint.Y - feetMiddlePoint.Y;
            return res;
        }
        //initialize face variables
        public static void Face()
        {
            bodySource = kinectSensor.BodyFrameSource;
            //open body reader  
            bodyReader = bodySource.OpenReader();
            
            //add frame arrived event for body
            bodyReader.FrameArrived += BodyReader_FrameArrived;
            //initialise bodies array
            bodies = new Body[kinectSensor.BodyFrameSource.BodyCount];

            //open face frame reader
            highDefinitionFaceFrameSource = new HighDefinitionFaceFrameSource(kinectSensor); 
            highDefinitionFaceFrameReader = highDefinitionFaceFrameSource.OpenReader();
              
            //add frame arrived event for face
            highDefinitionFaceFrameReader.FrameArrived += HdFaceReader_FrameArrived;
            
            // initialise variables
            currentFaceModel = new FaceModel();
            currentFaceAlignment = new FaceAlignment();
            vertices = null;
            colorPoint = new CameraSpacePoint[18];
            angles = new double[12];
            Array.Clear(angles, 0, 12);
            recintervalface = 100;
            okToGoface = false;
            //create console window for face alignment
            CreateHeadAlignmentWindow();
        }
        private static ulong CurrentTrackingId
        {
            get
            {
                return currentTrackingId;
            }

            set
            {
                currentTrackingId = value;

            }
        }
        //used to determine the distance of a body from the kinect
        private static double VectorLength(CameraSpacePoint point)
        {
            var result = Math.Pow(point.X, 2) + Math.Pow(point.Y, 2) + Math.Pow(point.Z, 2);
            result = Math.Sqrt(result);
            return result;
        }
        //find the closest body to the kinect
        private static Body FindClosestBody(BodyFrame bodyFrame)
        {
            Body result = null;
            double closestBodyDistance = double.MaxValue;

            bodyFrame.GetAndRefreshBodyData(bodies);

            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                    var currentLocation = body.Joints[JointType.SpineBase].Position;

                    var currentDistance = VectorLength(currentLocation);

                    if (result == null || currentDistance < closestBodyDistance)
                    {
                        result = body;
                        closestBodyDistance = currentDistance;
                    }
                }
            }

            return result;
        }
        //get the body with a certain tracking id
        private static Body FindBodyWithTrackingId(BodyFrame bodyFrame, ulong trackingId)
        {
            Body result = null;


            bodyFrame.GetAndRefreshBodyData(bodies);

            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                    if (body.TrackingId == trackingId)
                    {
                        result = body;
                        break;
                    }
                }
            }

            return result;
        }
        //extract head rotation
        private static void ExtractFaceRotationInDegrees(Vector4 rotQuaternion, out int pitch, out int yaw, out int roll)
        {
            double x = rotQuaternion.X;
            double y = rotQuaternion.Y;
            double z = rotQuaternion.Z;
            double w = rotQuaternion.W;
            //here starts "stuff"-note study more maths because rocket science
            // convert face rotation quaternion to Euler angles in degrees
            double yawD, pitchD, rollD;
            pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
            yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
            rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

            // clamp the values to a multiple of the specified increment to control the refresh rate
            double increment = FaceRotationIncrementInDegrees;
            pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
            yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
            roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
            //here ends "stuff"
        }
        private static void HdFaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
        {
            //use the received frame
            using (var frame = e.FrameReference.AcquireFrame())
            {

                //dispose of invalid frames or frames where the face isn't tracked
                if (frame == null || !frame.IsFaceTracked)
                {
                    return;
                }
                //refresh the face vertex data
                frame.GetAndRefreshFaceAlignmentResult(currentFaceAlignment);
                //check if having valid face orientation
                if (currentFaceAlignment.FaceOrientation != null)
                {
                    int pitch, yaw, roll;
                    ExtractFaceRotationInDegrees(currentFaceAlignment.FaceOrientation, out pitch, out yaw, out roll);
                    //check face rotation against 5 degrees threshold
                    if (Math.Abs(yaw) > 5 || Math.Abs(pitch) > 5 || Math.Abs(roll) > 5)//stuff for proper face orientation
                    {
                        temporaryConsole.Text += "Head not aligned\n";
                        temporaryConsole.ScrollToEnd();
                        //ignore frames with head rotation above the threshold
                        return;
                    }
                }
                else
                    return;
                //signal user the head orientation is alright
                temporaryConsole.Text += "OK\n";
                temporaryConsole.ScrollToEnd();
                //if in learn cycle(100 frames)
                if (learnintervalface >= 0)
                {
                    //call learn method for admin
                    LearnOrRecognize(angles,0);
                    //decrease learn counter
                    learnintervalface--;
                    //when learning at 0 frames
                    if (learnintervalface == 0)
                    {
                        //open writer to record data in a text file
                        using (StreamWriter sw = new StreamWriter(Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\Desktop\FaceReads.txt"))
                            for (int i = 0; i < angles.Length/2; i++)
                            {
                                //divide values by number of frames
                                angles[i] = angles[i] / 100;
                                sw.WriteLine(angles[i]);
                            }
                        //disable learning
                        learnintervalface = -1;
                        recintervalface = -1;
                        okToGoface = true;
                        headAlignmentWindow.Close();
                        CloseFace();
                    }
                }
                    //else if in recognition cycle(100 frames)
                else
                    if (recintervalface >= 0)
                    {
                        //if the admin angles array is empty
                        if (angles[0] == 0)
                        {
                            //open reader to load up the admin values
                            using (StreamReader sr = new StreamReader(Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\Desktop\FaceReads.txt"))
                                for (int i = 0; i < angles.Length/2; i++)
                                    angles[i] = Convert.ToDouble(sr.ReadLine());
                        }
                        //call recognition method
                        LearnOrRecognize(angles,6);
                        //decrease recognition interval
                        recintervalface--;
                        //when at 0 frames
                        if (recintervalface == 0)
                        {
                            faceResult = true;
                            for (int i = angles.Length/2; i < angles.Length; i++)
                            {
                                //divide values by number of frames
                                angles[i] = angles[i] / 100;
                                //signal admin not recognized if difference above threshold
                                if (Math.Abs(angles[i] - angles[i-6]) > 0.07)
                                    faceResult = false;
                            }
                            //empty the recognition half of the array
                            Array.Clear(angles,6,6);
                            //signal valid face data
                            okToGoface = true;
                            headAlignmentWindow.Close();
                            CloseFace();
                        }
                    }
            }
        }
        private static void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame == null)
                {
                    return;
                }

                if (currentTrackedBody != null)
                {
                    currentTrackedBody = FindBodyWithTrackingId(frame, CurrentTrackingId);

                    if (currentTrackedBody != null)
                    {
                        return;
                    }
                }

                Body selectedBody = FindClosestBody(frame);

                if (selectedBody == null)
                {
                    return;
                }

                currentTrackedBody = selectedBody;
                CurrentTrackingId = selectedBody.TrackingId;

                highDefinitionFaceFrameSource.TrackingId = CurrentTrackingId;
            }
        }
        //get distance between two points
        private static double Distance(CameraSpacePoint A, CameraSpacePoint B)
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
        }
        //get angle between 3 points
        private static double Angle(CameraSpacePoint A, CameraSpacePoint B, CameraSpacePoint C)
        {
            return Math.Acos((Math.Pow(Distance(A, B), 2) + Math.Pow(Distance(C, B), 2) - Math.Pow(Distance(A, C), 2)) / (2 * Distance(A, B) * Distance(C, B)));
        }
        //maps important vertices from the 2000 incoming for easier loop
        private static void MapPoints()
        {
            colorPoint[0] = vertices[28];//forehead center
            colorPoint[1] = vertices[458];//left cheek bone
            colorPoint[2] = vertices[4];//chin center

            colorPoint[3] = vertices[469];//outer left eye
            colorPoint[4] = vertices[28];//forehead center
            colorPoint[5] = vertices[1117];//outer right eye

            colorPoint[6] = vertices[210];//inner left eye
            colorPoint[7] = vertices[28];//forehead center
            colorPoint[8] = vertices[843];//inner right eye

            colorPoint[9] = vertices[412];//left cheek
            colorPoint[10] = vertices[4];//chin center
            colorPoint[11] = vertices[933];//right cheek

            colorPoint[12] = vertices[1307];//left jaw
            colorPoint[13] = vertices[4];//chin
            colorPoint[14] = vertices[1327];//right jaw

            colorPoint[15] = vertices[24];//nose top
            colorPoint[16] = vertices[674];//cheek bone right
            colorPoint[17] = vertices[14];//nose bottom
        }
        //learning/recognition method
        private static void LearnOrRecognize(double[] array,byte indexStart)
        {
            //get the 2000 3D vertices
            vertices = currentFaceModel.CalculateVerticesForAlignment(currentFaceAlignment);
            //map the points for easier access
            MapPoints();
            //determine if working on admin or user angles
            int k = 0+indexStart;
            //calculate the angles between every 3 points
            for (int i = 2; i < colorPoint.Length; i += 3)
                array[k++] += Angle(colorPoint[i - 2], colorPoint[i - 1], colorPoint[i]);

        }
        //close readers
        async public static void CloseFace()
        {
            if (bodyReader != null)
            {
                bodyReader.FrameArrived -= BodyReader_FrameArrived;
                bodyReader.Dispose();
                bodyReader = null;
                bodySource = null;
            }
            if (highDefinitionFaceFrameReader != null)
            {
                highDefinitionFaceFrameReader.FrameArrived -= HdFaceReader_FrameArrived;
                highDefinitionFaceFrameReader.Dispose();
                highDefinitionFaceFrameReader = null;
                highDefinitionFaceFrameSource = null;
            }
            kinectSensor.Close();
            await Task.Delay(5000);
            kinectSensor.Open();
        }
        //close readers
        public static void CloseHeight()
        {
            if (bodyFrameReader != null)
            {
                bodyFrameReader.FrameArrived -= Reader_FrameArrived;
                bodyFrameReader.Dispose();
                bodyFrameReader = null;               
            }
        }

        private static KinectAudioStream convertStream = null;

        private static SpeechRecognitionEngine speechEngine = null;

        public static void InitilaizeSpeech(string choice)
        {
            if (kinectSensor != null)
            {
                // open the sensor
               

                // grab the audio stream
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // create the convert stream
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // on failure, set the status text


            }

            RecognizerInfo ri = TryGetKinectRecognizer();

            if (null != ri)
            {


                speechEngine = new SpeechRecognitionEngine(ri.Id);
                Dictionary<string, string> commands = new Dictionary<string, string>();

                //grab the list of commands for the specific page 
                commands = setCurrentCommands(choice);
                var words = new Choices();

                //for every key value pair in commands add it to the words array
                foreach (var item in commands)
                {
                    
                    words.Add(new SemanticResultValue(item.Key, item.Value));
                }



                
                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(words);

                var g = new Grammar(gb);
                //load the words into 
                speechEngine.LoadGrammar(g);


                //fire the seppech recognised event
                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected +=
                    SpeechRejected;

                // let the convertStream know speech is going active
                convertStream.SpeechActive = true;
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);


                speechEngine.SetInputToAudioStream(
                convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {

            }


        }

        private static
            void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            const double ConfidenceThreshold = 0.7;


            //if the confidence of what has been said is within greater than the threshold
            if (e.Result.Confidence >= ConfidenceThreshold)
            {

                if (SpeechFound != null)
                    //fire the speech event
                    SpeechFound(e.Result.Semantics.Value.ToString());


            }
        }

        //close down steams and readers
        public static void WindowClosing()
        {
            if (null != convertStream)
            {
                convertStream.SpeechActive = false;
            }

            if (null != speechEngine)
            {
                speechEngine.SpeechRecognized -= SpeechRecognized;
                speechEngine.SpeechRecognitionRejected -= SpeechRejected;
                speechEngine.RecognizeAsyncStop();
            }

            
        }

        public static event SpeechEvent SpeechFound;

        //method to set commands required for the individual pages of the ui, based on paramter passed in
        public static Dictionary<string, string> setCurrentCommands(string choice)
        {
            Dictionary<string, string> commands =
        new Dictionary<string, string>();
            commands.Clear();


            switch (choice)
            {
                case "AddUser":
                    commands.Add("Illuminate Go Home", "HOME");
                    commands.Add("illuminate Go to RoomView", "LIVE");
                    commands.Add("illuminate Add A Family Member", "ADD");
                    commands.Add("illuminate Go To Admin", "ADMIN");
                    commands.Add("illuminate Go To List Of Users", "LISTUSER");
                    break;
                case "RoomView":
                    commands.Add("Illuminate Go Home", "HOME");
                    commands.Add("Illuminate Go To Add User", "ADDUSER");
                    commands.Add("illuminate Go To Admin", "ADMIN");
                    commands.Add("illuminate Go To List Of Users", "LISTUSER");
                    break;
                case "ListOfUsers":
                    commands.Add("Illuminate Go Home", "HOME");
                    commands.Add("Illuminate Go To Add User", "ADDUSER");
                    commands.Add("illuminate Go To Admin", "ADMIN");
                    commands.Add("illuminate Go To RoomView", "LIVE");
                    break;
                case "StartPage":
                    commands.Add("Illuminate Go To Add User", "ADDUSER");
                    commands.Add("illuminate Go To Admin", "ADMIN");
                    commands.Add("illuminate Go To List Of Users", "LISTUSER");
                    commands.Add("illuminate Go To RoomView", "LIVE");
                    

                    break;
                case "AddAdmin":
                    commands.Add("Illuminate Go Home", "HOME");
                    commands.Add("Illuminate Go To Add User", "ADDUSER");
                    commands.Add("illuminate Go To List Of Users", "LISTUSER");
                    commands.Add("illuminate Go To RoomView", "LIVE");
                    break;
                case "PasswordPage":
                    commands.Add("Illuminate Send Help", "HELP");
                    break;

            }

            commands.Add("Illuminate Log Out", "LOG");
            

            return commands;
        }

        public static void CloseSpeech()
        {
            if (kinectSensor != null)
            {
                kinectSensor.Close();
            }
        }

        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }
   
    
    }
}
