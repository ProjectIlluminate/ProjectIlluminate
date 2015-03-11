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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            App.WindowClosing();
            App.InitilaizeSpeech("AddAdmin");
            App.SpeechFound += App_SpeechFound;
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
                case "LIVE":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new RoomViewPage());

                    break;
                case "LISTUSER":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new ListOfUsersPage());
                    break;


            }
        }

        //Button click to return to previous page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new StartPage());
        }

        private async void btnAddAdmin_Click(object sender, RoutedEventArgs e)
        {
            //App.learnintervalface = 100;//take out when points are already saved in a file and reading works

            //scanBox.Text = "Please wait while the Kinect runs facial recognition";

            ////initialize face recognition
            //App.Face();
            //while (App.okToGoface == false)
            //{
            //    await Task.Delay(10000);
            //}
            scanBox.Text = "Scan has completed. Administrator has been changed.";
        }
    }
}