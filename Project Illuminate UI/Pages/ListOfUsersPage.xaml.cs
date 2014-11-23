using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ListOfUsersPage.xaml
    /// </summary>
    public partial class ListOfUsersPage : Page
    {
        public ListOfUsersPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //string[] lines = File.ReadAllLines("UserList.txt");

            //foreach (var line in lines)
            //{
            //    string[] userDetails = line.Split(',');
            //    User currentUsers = new User(userDetails[0], userDetails[1], userDetails[2], userDetails[3], userDetails[4]);
            //    lstBxUsersList.Items.Add(currentUsers);

            //}
        }

        //Button click to return to previous page
        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new StartPage());
        }

    }
}
