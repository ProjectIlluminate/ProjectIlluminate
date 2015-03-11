using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;//new namespace for Observable collection
using System.IO;//new namespace for writing to text file
using System.Linq;
using System.Text;
using System.Threading;//new namespace for writing to XML
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
using System.Xml;

namespace Project_Illuminate_UI.Pages
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        Entities db = new Entities();

        public AddUserPage()
        {
            InitializeComponent();
            ////Loads XML into an XMLDocument object and saves it out to a file
            ////Create the XML document
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml("<item><name>User Name</name></item>");
            App.WindowClosing();
            

            App.InitilaizeSpeech("AddUser");
            App.SpeechFound += App_SpeechFound;
            ////Adds a height element
            //XmlElement newElement = doc.CreateElement("Height");
            //newElement.InnerText = "166cm";
            //doc.DocumentElement.AppendChild(newElement);

            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;
            ////Save the document to a file and auto-indent the output
            //XmlWriter writer = XmlWriter.Create("Userdata.xml", settings);
            //doc.Save(writer);
        }

        void App_SpeechFound(string command)
        {


            switch (command)
            {
                case "HOME":
                    
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new StartPage());

                    break;

                case "LIVE":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new RoomViewPage());
                    break;
                case "ADD":

                    break;
                case "ADMIN":
                   
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new AdminPage());

                    break;
                case "USER":
                    
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new ListOfUsersPage());
                    break;


            }
        }

        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
        }

        
        private ObservableCollection<User> Users = new ObservableCollection<User>();
        private async void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            tblkSaved.Text = "";

            tbHeight.Text = "";
            App.learnintervalbody = 100;
            App.InitializeBody();
            while (App.okToGoBodyLearn == false)
            {
                await Task.Delay(10000);
            }
            tbHeight.Text = App.recorderdHeight.ToString();
        }

        private void btnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            User newUser = new User();
            newUser.Name = tbName.Text;
            newUser.Height = (float)Convert.ToDecimal(tbHeight.Text);

            db.Users.Add(newUser);
            db.SaveChanges();

            tblkSaved.Text = ("User details have been saved");
        }

        private void tbName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tbName.Text != "")
                btnAddUser.IsEnabled = true;
        }

        //Button click to return to previous page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new StartPage());
        }

        private void tbHeight_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tbHeight.Text != "")
                btnSaveUser.IsEnabled = true;
        }
    }
}
