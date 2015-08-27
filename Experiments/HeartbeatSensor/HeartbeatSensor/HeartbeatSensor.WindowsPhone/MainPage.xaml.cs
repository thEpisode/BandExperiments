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
using Windows.UI;
using Windows.UI.ViewManagement;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HeartbeatSensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private IBandClient bandClient;
        private StatusBar statusBar;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            HeartAppear.Begin();
            statusBar.ForegroundColor = Colors.White;
            statusBar.BackgroundOpacity = 0;
        }


        private async Task<bool> HasConsentToUse()
        {
            if (bandClient != null)
            {
                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    // user has not consented, request it
                    await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            Debug.WriteLine("Failed to trying to connect Band");
            return false;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            StartListening();
            StopAppear.Begin();
        }



        public async void StartListening()
        {
            statusBar.ProgressIndicator.Text = "acquiring data...";
            await statusBar.ProgressIndicator.ShowAsync();

            await ConnectToBand();
            if (await HasConsentToUse())
            {
                
                if (bandClient != null)
                {
                    var sensor = bandClient.SensorManager.HeartRate;
                    sensor.ReadingChanged += SensorReadingChanged;
                    await sensor.StartReadingsAsync();
                }
            }
            else
            {
                StartListening();
            }
        }

        private async Task ConnectToBand()
        {
            var pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Any())
            {
                var band = pairedBands.First();
                bandClient = await BandClientManager.Instance.ConnectAsync(band);                
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

                        StatusBar.GetForCurrentView().ProgressIndicator.Text = "";
                        StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
                    });

                    

                    
                    
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
    