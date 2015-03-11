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
using Project_Illuminate_UI.Pages;//enables connection to other pages

namespace Project_Illuminate_UI.Pages
{
    /// <summary>
    /// Interaction logic for PasswordPage.xaml
    /// </summary>
    /// 

    public partial class PasswordPage : Page
    {
        string password = "007";
        string inputtedPassword = "";

        public PasswordPage()
        {
            InitializeComponent();        
        }

        private void TypePassword(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                switch (btn.Content.ToString())
                {
                    case "1":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "2":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "3":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "4":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "5":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "6":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "7":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "8":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "9":
                        inputtedPassword += btn.Content.ToString();
                        break;
                    case "0":
                        inputtedPassword += btn.Content.ToString();
                        break;
                }
            }
        }//end TypePassword

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (inputtedPassword == password)//If inputted password matches the stored password
            {
                App.NavigationFrame.Navigate(new StartPage());//Navigate to Start page
            }
            else
                lblMessageAlert.Content = String.Format("Password incorrect. Please try again");
        }//end button enter click


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            inputtedPassword = "";
            lblMessageAlert.Content = "";
        }//end btnClear

    }
}
