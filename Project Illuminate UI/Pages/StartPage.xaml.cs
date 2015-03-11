using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
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
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {

        public StartPage()
        {
            InitializeComponent();
            //App.Face();
            //KinectRegion.SetKinectRegion(this, kinectRegion);
            App.WindowClosing();
            App.InitilaizeSpeech("StartPage");


            App.SpeechFound += App_SpeechFound;
            //App app = ((App)Application.Current);
            //app.KinectRegion = kinectRegion;
        }
            //// Use the default sensor
            //this.kinectRegion.KinectSensor = KinectSensor.GetDefault();
           void App_SpeechFound(string command)
        {
            switch (command)
            {
                case "ADMIN":
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new AdminPage());
                 
                    break;

                case "ADDUSER":
                    
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new AddUserPage());
                    break;

                case "LIVE":
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new RoomViewPage());
                    break;
                case "LISTUSER":

                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new ListOfUsersPage());
                    break;

                case "LOG":
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new ProjectIlluminate());
                    break;


            }
        }



        //On button click, if its not empty, navigate to the following page
        private void btnRoomView_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseFace();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new RoomViewPage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseFace();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new AddUserPage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseFace();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new AdminPage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnListOfUsers_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseFace();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new ListOfUsersPage());
        }

        //On button click, if its not empty, navigate back to the Password page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseHeight();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new PasswordPage());
        }

        //On button click, if its not empty, navigate back to the Password page
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            //App.CloseHeight();
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new ProjectIlluminate());
        }


    }
}
