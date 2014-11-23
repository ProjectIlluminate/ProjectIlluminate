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
// added using statements...
using MahApps.Metro.Controls;
using Microsoft.Kinect;//
using Project_Illuminate_UI.Pages;//
using Microsoft.Kinect.Wpf.Controls;//connection to other pages 

namespace Project_Illuminate_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            App.NavigationFrame = this.NavigationFrame;
            App.NavigationFrame.Navigate(new ProjectIlluminate());

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            // Use the default sensor
            this.kinectRegion.KinectSensor = KinectSensor.GetDefault();
        }
        
        //Control
        public static Grid UserGrid { get; set; }
        public void Grid()
        {
            //USER VIEWER CODE
            //Get UserViewer up and running
            KinectRegion kinectRegion = new KinectRegion();
            KinectUserViewer kinectUserViewer = new KinectUserViewer()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 100,
                Width = 121,
            };
            Grid grid = new Grid();


            //grid.Children.Add(kinectRegion);
            //grid.Children.Add(kinectUserViewer);
            ////kinectRegion.Content = rootFrame;
            ////place the frame in the current window
            //App.UserGrid = grid;
            ////* end new code
        }
    }
}
