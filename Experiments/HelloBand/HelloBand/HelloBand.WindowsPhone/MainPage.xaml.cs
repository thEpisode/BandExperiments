using System;
using System.Collections;
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
using System.Threading.Tasks;
using Microsoft.Band.Tiles;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band.Tiles.Pages;
using Windows.UI;
using System.Diagnostics;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelloBand
{
    public sealed partial class MainPage : Page
    {
        IBandInfo band;
        bool canSetTile;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Used for trying connect to the first band found
        /// </summary>
        /// <returns>Band information</returns>
        private async Task<IBandInfo> GetConnectedBand()
        {
           return (await BandClientManager.Instance.GetBandsAsync()).FirstOrDefault();
        }

        private async Task<string[]> GetBandInformation()
        {
            using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(band))
            {
                try
                {
                    string[] bandInformation = new string[2];
                    // Getting firmware information
                    bandInformation[0] = await bandClient.GetFirmwareVersionAsync();
                    // Getting hardware information
                    bandInformation[1] = await bandClient.GetHardwareVersionAsync();
                    return bandInformation;
                }
                catch (BandException ex)
                {
                    string[] exception = new string[1];
                    exception[0] = ex.Message;
                    return exception;
                }
            }
        }

        private async Task<BandTile[]> GetBandTiles()
        {
            using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(band))
            {
                try
                {     // get the current set of tiles      
                    return (await bandClient.TileManager.GetTilesAsync()).ToArray();
                }
                catch (BandException ex)

                {     // handle a Band connection exception
                    return null;
                }
            }
        }

        private async Task<int> TileCapacity()
        {
            int tileCapacity = 0;
            using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(band))
            {
                try
                {
                    // determine the number of available tile slots on the Band     
                    tileCapacity = await bandClient.TileManager.GetRemainingTileCapacityAsync();
                    bandClient.Dispose();
                }
                catch (BandException ex)
                {
                    tileCapacity = 0;
                }
            }
            return tileCapacity;
        }

        private async Task<BandTile> CreateTile()
        {
            // create a new Guid for the tile 
            Guid tileGuid = new Guid("D781F673-6D05-4D69-BCFF-EA7E706C3418");
            // create a new tile with a new Guid 
            BandTile tile = new BandTile(tileGuid)
            {
                // enable badging (the count of unread messages)     
                IsBadgingEnabled = true,
                // set the name     
                Name = "HelloBand",
                // set the icons     
                SmallIcon = await LoadIcon("ms-appx:///Assets/Bat-Man24.png"),
                TileIcon = await LoadIcon("ms-appx:///Assets/Bat-Man46.png")
            };
            return tile;
        }

        private async Task<BandIcon> LoadIcon(string uri)
        {
            StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                WriteableBitmap bitmap = new WriteableBitmap(1, 1);
                await bitmap.SetSourceAsync(fileStream);
                return bitmap.ToBandIcon();
            }
        }

        private async Task<bool> CreateLayout(BandTile bandTile)
        {
            bool result = false;

            using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(band))
            {
                Microsoft.Band.Tiles.Pages.TextBlock myBandTextBlock = new Microsoft.Band.Tiles.Pages.TextBlock()
                {
                    ElementId = 1, // the Id of the TextBlock element; we'll use it later to set its text to "MY CARD"
                    Rect = new PageRect(0, 0, 200, 25)
                };

                FlowPanel panel = new FlowPanel(myBandTextBlock)
                {
                    Orientation = FlowPanelOrientation.Vertical,
                    Rect = new PageRect(60, 50, 250, 100)
                };
                
                bandTile.PageLayouts.Add(new PageLayout(panel));

                await bandClient.TileManager.RemoveTileAsync(bandTile.TileId);
                try
                {
                    // add the tile to the Band  
                    if (await bandClient.TileManager.AddTileAsync(bandTile))
                    {
                        // And create the page with the specified texts and values.
                        PageData page = new PageData(
                            Guid.NewGuid(), // the Id for the page
                            0, // the index of the layout to be used; we have only one layout in this sample app, but up to 5 layouts can be registered for a Tile
                            new TextBlockData(myBandTextBlock.ElementId.Value, "Hello Band!"));

                        await bandClient.TileManager.SetPagesAsync(bandTile.TileId, page);
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }
                return result;
            }
        }
                
        private async void AddTileButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (canSetTile)
            {
                BandTile tile = await CreateTile();
                if (await CreateLayout(tile))
                {
                    resultText.Text = "Tile added successfuly";
                }
                else
                {
                    resultText.Text = "Something wrong, debug this Hello App";
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            canSetTile = false;
            AddTileButton.Visibility = Visibility.Collapsed;
            SettingUp();
        }

        private async void SettingUp()
        {
            resultText.Text = "Retrieving information";
            band = await GetConnectedBand();
            if (band != null)
            {
                string[] bandInformation = await GetBandInformation();
                BandTile[] bandTiles = await GetBandTiles();
                
                try
                {
                    resultText.Text = "Retrieving capacity";
                    int capacity = await TileCapacity();
                    if (capacity > 0)
                    {
                        canSetTile = true;
                        resultText.Text = "Ok";

                        AddTileButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        canSetTile = true;
                        resultText.Text = "You haven't more spaces... but this app delete the Tile :P";
                        AddTileButton.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    canSetTile = false;
                    resultText.Text = ex.Message;
                }

            }
        }        
    }
}
