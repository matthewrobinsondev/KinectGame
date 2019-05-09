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
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Fusion;
using Microsoft.Kinect.Toolkit.Interaction;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Level1 : Page
    {
        private KinectSensorChooser sensorChooser;
        public Level1()
        {

            InitializeComponent();
            this.Loaded += OnLoadedLevel;

        }

        private void OnLoadedLevel(object sender, RoutedEventArgs routedEventArgs)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;

            this.sensorChooser.Start();
        }
        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            bool error = false;
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // E.g.: sensor might be abruptly unplugged.
                    error = true;

                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();
                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode,
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                        error = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    // E.g.: sensor might be abruptly unplugged.
                    error = true;
                }
                if (!error)
                    kinectRegion.KinectSensor = args.NewSensor;
            }
        }
        private void ButtonOnOption1(object sender, RoutedEventArgs e)
        {
            Option1ANS.Visibility = System.Windows.Visibility.Visible;
            Option2ANS.Visibility = System.Windows.Visibility.Hidden;

        }
        private void ButtonOnOption2(object sender, RoutedEventArgs e)
        {
            Option2ANS.Visibility = System.Windows.Visibility.Visible;
            Option1ANS.Visibility = System.Windows.Visibility.Hidden;
        }
        private void ButtonOnOption3(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Level2());
        }

        private void ButtonOnConfirm(object sender, RoutedEventArgs e)
        {
            if (Option1ANS.Visibility != Visibility.Visible)

                MessageBox.Show("Incorrect Answer");

            else

                MessageBox.Show("Correct Answer");



        }
    }
}

