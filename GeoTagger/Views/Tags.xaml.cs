using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoTagger.Controls;
using GeoTagger.Model;
using GeoTagger.Services;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoTagger.Views
{
    public partial class Tags : ContentPage
    {
        public Tags(double lat, double longitud)
        {
            InitializeComponent();

            MyMap.CustomPins = new List<CustomPin>();

            Device.BeginInvokeOnMainThread(async()=>{
                EntryService entryService = new EntryService();

                List<GeoTaggerEntry> entries = await entryService.GetLastEntries();

                foreach (GeoTaggerEntry entry in entries)
                {
                    var thisPosition = new Position(double.Parse(entry.Lat), double.Parse(entry.Long)); // Latitude, Longitude
                    var pin = new CustomPin
                    {
                        Type = PinType.Generic,
                        Position = thisPosition,
                        Label = entry.Text,
                        Url = entry.Link
                    };
                    MyMap.CustomPins.Add(pin);
                    MyMap.Pins.Add(pin);
                }
            });

            var mapPosition = new Position(-34, -58);
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition,
                        Distance.FromMiles(1)));
        }
    }
}
