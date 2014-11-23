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
    /// Interaction logic for MessagingServicePage.xaml
    /// </summary>
    public partial class MessagingServicePage : Page
    {
        public MessagingServicePage()
        {
            InitializeComponent();
        }

        ////Button click to return to previous page
        //private void btnGoBack_Click(object sender, RoutedEventArgs e)
        //{
        //    if (App.NavigationFrame != null)
        //        App.NavigationFrame.Navigate(new StartPage());
        //}
    }
}
