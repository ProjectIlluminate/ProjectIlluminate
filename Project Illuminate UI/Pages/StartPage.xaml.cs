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
            //KinectRegion.SetKinectRegion(this, kinectRegion);

            //App app = ((App)Application.Current);
            //app.KinectRegion = kinectRegion;

            //// Use the default sensor
            //this.kinectRegion.KinectSensor = KinectSensor.GetDefault();
        }

        //On button click, if its not empty, navigate to the following page
        private void btnRoomView_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new RoomViewPage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new AddUserPage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnMessagingService_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new MessagingServicePage());
        }

        //On button click, if its not empty, navigate to the following page
        private void btnListOfUsers_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new ListOfUsersPage());
        }

        //On button click, if its not empty, navigate back to the Password page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new PasswordPage());
        }


    }
}
