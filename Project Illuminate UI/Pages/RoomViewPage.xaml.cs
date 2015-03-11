using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;//
using Project_Illuminate_UI.Pages;//
using Microsoft.Kinect.Wpf.Controls;//connection to other pages 

namespace Project_Illuminate_UI.Pages
{
    /// <summary>
    /// Interaction logic for RoomViewPage.xaml
    /// </summary>
    /// 
    public partial class RoomViewPage : Page
    {

        private ColorFrameReader colorFrameReader = null;
        private WriteableBitmap colorBitmap = null;
        private KinectSensor kinectSensor = null;


        TimeSpan start = new TimeSpan(09, 0, 0); //9am
        TimeSpan end = new TimeSpan(21, 0, 0); //9pm
        TimeSpan now = DateTime.Now.TimeOfDay;

        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;

        /// <summary>
        /// The value by which the infrared source data will be scaled
        /// </summary>
        private const float InfraredSourceScale = 0.75f;

        /// <summary>
        /// Smallest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMinimum = 0.01f;

        /// <summary>
        /// Largest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMaximum = 1.0f;

        /// <summary>
        /// Reader for infrared frames
        /// </summary>
        private InfraredFrameReader infraredFrameReader = null;

        /// <summary>
        /// Description (width, height, etc) of the infrared frame data
        /// </summary>
        private FrameDescription infraredFrameDescription = null;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap infraredBitmap = null;



        //Button click to return to previous page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new StartPage());
        }


        public RoomViewPage()
        {
            InitializeComponent();

            App.WindowClosing();

            App.InitilaizeSpeech("RoomView");
            App.SpeechFound += App_SpeechFound;
            this.kinectSensor = KinectSensor.GetDefault();

            if (now >= start && now <= end)
            {
                // open the reader for the color frames
                this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

                // wire handler for frame arrival
                this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

                // create the colorFrameDescription from the ColorFrameSource using Bgra format
                FrameDescription colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

                // create the bitmap to display
                this.colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
            }
            else
            {
                // open the reader for the depth frames
                this.infraredFrameReader = this.kinectSensor.InfraredFrameSource.OpenReader();

                // wire handler for frame arrival
                this.infraredFrameReader.FrameArrived += this.Reader_InfraredFrameArrived;

                // get FrameDescription from InfraredFrameSource
                this.infraredFrameDescription = this.kinectSensor.InfraredFrameSource.FrameDescription;

                // create the bitmap to display
                this.infraredBitmap = new WriteableBitmap(this.infraredFrameDescription.Width, this.infraredFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray32Float, null);
            }

            // open the sensor
            this.kinectSensor.Open();
            this.DataContext = this;
        }

        public ImageSource ImageSource
        {
            get
            {
                if (now >= start && now <= end)
                {
                    return this.colorBitmap;
                }
                else
                {
                    return this.infraredBitmap;
                }
            }
        }

        void App_SpeechFound(string command)
        {

            switch (command)
            {
                case "HOME":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new StartPage());

                    break;

                case "ADDUSER":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new AddUserPage());

                    break;
                case "ADMIN":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new AdminPage());

                    break;
                case "LISTUSER":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new ListOfUsersPage());
                    break;


            }
        }

        private void Reader_InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            // InfraredFrame is IDisposable
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    // the fastest way to process the infrared frame data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer())
                    {
                        // verify data and write the new infrared frame data to the display bitmap
                        if (((this.infraredFrameDescription.Width * this.infraredFrameDescription.Height) == (infraredBuffer.Size / this.infraredFrameDescription.BytesPerPixel)) &&
                            (this.infraredFrameDescription.Width == this.infraredBitmap.PixelWidth) && (this.infraredFrameDescription.Height == this.infraredBitmap.PixelHeight))
                        {
                            this.ProcessInfraredFrameData(infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                        }
                    }
                }
            }
        }


        private unsafe void ProcessInfraredFrameData(IntPtr infraredFrameData, uint infraredFrameDataSize)
        {
            // infrared frame data is a 16 bit value
            ushort* frameData = (ushort*)infraredFrameData;

            // lock the target bitmap
            this.infraredBitmap.Lock();

            // get the pointer to the bitmap's back buffer
            float* backBuffer = (float*)this.infraredBitmap.BackBuffer;

            // process the infrared data
            for (int i = 0; i < (int)(infraredFrameDataSize / this.infraredFrameDescription.BytesPerPixel); ++i)
            {
                // since we are displaying the image as a normalized grey scale image, we need to convert from
                // the ushort data (as provided by the InfraredFrame) to a value from [InfraredOutputValueMinimum, InfraredOutputValueMaximum]
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }

            // mark the entire bitmap as needing to be drawn
            this.infraredBitmap.AddDirtyRect(new Int32Rect(0, 0, this.infraredBitmap.PixelWidth, this.infraredBitmap.PixelHeight));

            // unlock the bitmap
            this.infraredBitmap.Unlock();
        }


        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.colorBitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }
                        this.colorBitmap.Unlock();
                    }
                }
            }
        }
        private void Closing()
        {
            if (this.colorFrameReader != null)
            {
                // ColorFrameReder is IDisposable
                this.colorFrameReader.Dispose();
                this.colorFrameReader = null;
            }
            if (this.infraredFrameReader != null)
            {
                // InfraredFrameReader is IDisposable
                this.infraredFrameReader.Dispose();
                this.infraredFrameReader = null;
            }
            if (this.kinectSensor != null)
            {
                this.kinectSensor = null;
            }
        }
        ////Control
        //public static Grid UserGrid { get; set; }

        //public void Grid()
        //{
        //    //*Get UserViewer up and running
        //    KinectRegion kinectRegion = new KinectRegion();
        //    KinectUserViewer kinectUserViewer = new KinectUserViewer()
        //    {
        //        HorizontalAlignment = HorizontalAlignment.Center,
        //        VerticalAlignment = VerticalAlignment.Top,
        //        Height = 100,
        //        Width = 121,
        //    };

        //    Grid grid = new Grid();

        //    grid.Children.Add(kinectRegion);
        //    grid.Children.Add(kinectUserViewer);
        //    //kinectRegion.Content = rootFrame;
        //    kinectRegion.Content = UserGrid;

        //    //place the frame in the current window
        //    //App.Current.Content = grid;
        //    //App.UserGrid = Grid;
        //}
    }
}
