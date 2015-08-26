using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HeartbeatSensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            HeartAppear.Begin();
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            StartListening();
            StopAppear.Begin();
        }


        private IBandClient bandClient;

        public async void StartListening()
        {
            var pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Any())
            {                
                var band = pairedBands.First();
                bandClient = await BandClientManager.Instance.ConnectAsync(band);
                var sensor = bandClient.SensorManager.HeartRate;
                sensor.ReadingChanged += SensorReadingChanged;
                await sensor.StartReadingsAsync();
            }
        }

        public async void StopListening()
        {
            HeartbeatAnimation.Stop();
            var sensor = bandClient.SensorManager.HeartRate;
            sensor.ReadingChanged -= SensorReadingChanged;
            await sensor.StopReadingsAsync();
            bandClient.Dispose();
            StartAppear.Begin();
            HeartbeatOutput.Text = "0";
        }

        async void SensorReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            
            try
            {
                if (e.SensorReading != null)
                {
                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        HeartbeatAnimation.Begin();
                        HeartbeatOutput.Text = e.SensorReading.HeartRate.ToString();
                    });
                    
                    Debug.WriteLine("Sending update, pulserate = {0}", e.SensorReading.HeartRate);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading/sending data: {0}", ex);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopListening();
        }
    }
}
    