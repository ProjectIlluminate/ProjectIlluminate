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
        Entities db = new Entities();
        ComboBox combobox = new ComboBox();

        public ListOfUsersPage()
        {
            InitializeComponent();

            App.WindowClosing();
            App.InitilaizeSpeech("ListOfUsers");
            App.SpeechFound += App_SpeechFound;
            CreateDynamicWPFGrid();
        }

        private void CreateDynamicWPFGrid()
        {
            // Create the Grid programatically
            Grid DynamicGrid = new Grid();
            //DynamicGrid.ShowGridLines = true;

            // Create Columns
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            ColumnDefinition gridCol4 = new ColumnDefinition();
            ColumnDefinition gridCol5 = new ColumnDefinition();
            DynamicGrid.ColumnDefinitions.Add(gridCol1);
            DynamicGrid.ColumnDefinitions.Add(gridCol2);
            DynamicGrid.ColumnDefinitions.Add(gridCol3);
            DynamicGrid.ColumnDefinitions.Add(gridCol4);
            DynamicGrid.ColumnDefinitions.Add(gridCol5);

            // Create Rows
            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            RowDefinition gridRow3 = new RowDefinition();
            RowDefinition gridRow4 = new RowDefinition();
            DynamicGrid.RowDefinitions.Add(gridRow1);
            DynamicGrid.RowDefinitions.Add(gridRow2);
            DynamicGrid.RowDefinitions.Add(gridRow3);
            DynamicGrid.RowDefinitions.Add(gridRow4);

            Color[] color = new Color[] { Colors.LawnGreen, Colors.DodgerBlue, Colors.Crimson, Colors.SpringGreen, Colors.Violet, Colors.Yellow };

            //Dynamically display users in Database on this page
            int i = 1;//place on grid top row
            int j = 0;//colour
            int k = 1;//place on grid bottom row
            foreach (var user in db.Users)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Fill = new SolidColorBrush(color[j]);
                j++;
                rectangle.Margin = new Thickness(5);

                Image image = new Image();
                image.Margin = new Thickness(20);
                image.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //Display User image 
                string currentDirectory = Directory.GetCurrentDirectory();//get current directory
                DirectoryInfo di = Directory.GetParent(currentDirectory);//get parent of this
                di = Directory.GetParent(di.FullName); //get parent of this again
                var uriSource = new Uri(di.FullName + "\\Images\\" + "LogIn" + ".png");//append on image directory to 2 directorys up from start point directory
                image.Source = new BitmapImage(uriSource);//Display the user icon image

                //Display User details
                TextBlock textBlock = new TextBlock();
                //textBlock.Style = new System.Windows.Style.
                textBlock.FontWeight = FontWeights.Bold;
                textBlock.FontSize = 12;
                textBlock.Padding = new Thickness(10);
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                //textBlock.Margin = new Thickness(10);
                var name = user.Name;
                var height = user.Height;
                textBlock.Text = string.Format
                                ("Name: {0} Height: {1:f2} ", name, height);
                if (i <= 3)
                {
                    Grid.SetRow(image, 1);
                    Grid.SetRow(textBlock, 1);
                    Grid.SetRow(rectangle, 1);

                    Grid.SetColumn(image, i);
                    Grid.SetColumn(textBlock, i);
                    Grid.SetColumn(rectangle, i++);
                }
                else
                {
                    Grid.SetRow(image, 2);
                    Grid.SetRow(textBlock, 2);
                    Grid.SetRow(rectangle, 2);

                    Grid.SetColumn(image, k);
                    Grid.SetColumn(textBlock, k);
                    Grid.SetColumn(rectangle, k++);
                }
                DynamicGrid.Children.Add(rectangle);
                DynamicGrid.Children.Add(image);
                DynamicGrid.Children.Add(textBlock);
            }
            combobox.Name = "cbxValues";
            combobox.Margin = new Thickness(30);
            combobox.MaxHeight = 50;
            combobox.MinWidth = 200;
            combobox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            combobox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            Grid.SetRow(combobox, 5);
            Grid.SetColumn(combobox, 2);
            DynamicGrid.Children.Add(combobox);

            //Delete a User Button
            Button button = new Button();
            button.Content = "Delete a user";
            button.MaxHeight = 50;
            button.MinWidth = 200;
            button.FontWeight = FontWeights.Bold;
            button.FontSize = 12;
            button.Foreground = Brushes.White;
            button.Background = App.Current.Resources["myBrush"] as SolidColorBrush;
            button.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Grid.SetRow(button, 5);
            Grid.SetColumn(button, 2);
            DynamicGrid.Children.Add(button);

            button.Click += button_Click;

            foreach (var username in db.Users)
            {
                combobox.Items.Add(username.Name);
            }

            //Return to previous page Button
            Button buttonBack = new Button();
            buttonBack.Content = "GO BACK";
            buttonBack.MaxHeight = 50;
            buttonBack.MinWidth = 100;
            buttonBack.FontWeight = FontWeights.Bold;
            buttonBack.FontSize = 10;
            buttonBack.Foreground = Brushes.White;
            buttonBack.Background = App.Current.Resources["myBrush"] as SolidColorBrush;
            buttonBack.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            buttonBack.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Grid.SetRow(buttonBack, 0);
            Grid.SetColumn(buttonBack, 0);
            DynamicGrid.Children.Add(buttonBack);

            buttonBack.Click += buttonBack_Click;

            //Add Grid to ListOfUsers page
            LstUsersPg.Content = DynamicGrid;
        }


        void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (App.NavigationFrame != null)
                App.NavigationFrame.Navigate(new StartPage());
        }

        //Delete User
        void button_Click(object sender, RoutedEventArgs e)
        {
            User query = (from u in db.Users
                          where u.Name == combobox.SelectedValue.ToString()
                          select u).FirstOrDefault();

            if (query != null)
            {
                db.Users.Remove(query);
                db.SaveChanges();
                App.NavigationFrame.Navigate(new ListOfUsersPage());
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
                case "LIVE":
                    
                    App.SpeechFound -= App_SpeechFound;
                    App.NavigationFrame.Navigate(new RoomViewPage());
                    break;


            }
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
