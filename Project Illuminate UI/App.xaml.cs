using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect.Wpf.Controls;//needed
using System.Windows.Controls;//

namespace Project_Illuminate_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame NavigationFrame { get; set; }
        internal KinectRegion KinectRegion { get; set; }
    }
}
