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
    public partial class RoomViewPage : Page
    {
        public RoomViewPage()
        {
            InitializeComponent();
        }


        //////Button click to return to previous page
        //private void btnGoBack_Click(object sender, RoutedEventArgs e)
        //{
        //    if (App.NavigationFrame != null)
        //        App.NavigationFrame.Navigate(new StartPage());
        //}


        //Control
        public static Grid UserGrid { get; set; }

        public void Grid()
        {
            //*Get UserViewer up and running
            KinectRegion kinectRegion = new KinectRegion();
            KinectUserViewer kinectUserViewer = new KinectUserViewer()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 100,
                Width = 121,
            };

            Grid grid = new Grid();

            grid.Children.Add(kinectRegion);
            grid.Children.Add(kinectUserViewer);
            //kinectRegion.Content = rootFrame;
            kinectRegion.Content = UserGrid;

            //place the frame in the current window
            //App.Current.Content = grid;
            //App.UserGrid = Grid;
        }
    }
}
