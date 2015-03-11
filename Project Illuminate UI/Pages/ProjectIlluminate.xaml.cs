using Microsoft.ServiceBus.Notifications;
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

namespace Project_Illuminate_UI.Pages
{
    public partial class ProjectIlluminate : Page
    {
        bool rec;
        public ProjectIlluminate()
        {
            //signal recognition cycle
            rec = true;
            InitializeComponent();
            //open the kinect sensor
            App.OpenKinect();
            //start body recognition
            //Recognition();
            App.InitilaizeSpeech("PasswordPage");
            //App.SpeechFound += App_SpeechFound;
        }

        void App_SpeechFound(string command)
        {
            switch (command)
            {
                case "HELP":
                    App.WindowClosing();
                    App.SpeechFound -= App_SpeechFound;
                    NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://voldemtrial2-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=5gnvGy9hmygDZDy5HeofpYGP8GmVzYeakykypntS81s=", "finaltrial1");

                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Unknown Person inside your home, please take action.</text></binding></visual></toast>";

                    hub.SendWindowsNativeNotificationAsync(toast).Wait();


                    break;

            }
        }
        public async void Recognition()
        {
            //while in recognition cycle
            while (rec == true)
            {
                //signal to avoid learning
                App.learnintervalbody = -1;
                //initialize the body
                App.InitializeBody();
                //wait for valid data
                while (App.okToGoBodyGlobal == false)
                {
                    await Task.Delay(10000);//kinect makes it complicated to wait for body method instead of set time :/
                }
                //display the body result
                hiBox.Text = App.bodyResult;
            }
        }

        private async void GoToPasswordPg(object sender, RoutedEventArgs e)
        {
            //close the heigh
            App.CloseHeight();
            //exit recognition cycle
            rec = false;
            //avoid learning for face
            App.learnintervalface = -1;//take out when points are already saved in a file and reading works
            //initialize face recognition
            scanBox.Text = "Please wait while the Kinect runs facial recognition";
            App.Face();
            //wait for valid data
            while (App.okToGoface == false)
            {
                await Task.Delay(10000);
            }

            if (App.NavigationFrame != null)
            {
                //display the result of face recognition
                hiBox.Text = App.faceResult.ToString();
                //if user recognised as administrator allow navigation to password page
                if (App.faceResult == true || App.faceResult == false)//just for testing speed we ignore the face login result
                {
                    scanBox.Text = "";
                    App.NavigationFrame.Navigate(new PasswordPage());
                }
                else
                {
                      scanBox.Text = "";
                    //go back to body recognition cycle
                    rec = true;
                    Recognition();
                }
            }
        }
    }
}
