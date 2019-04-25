using System;
using System.Collections.Generic;
using GeoTagger.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace GeoTagger.Views
{
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small
            });

            if (photo != null)
            {
                Position position = await CrossGeolocator.Current.GetPositionAsync();
                EntryService entryService = new EntryService();

                var imageName = await entryService.SaveImage(photo.GetStream());
                await entryService.SaveEntryAsync(new Model.GeoTaggerEntry(FotoText.Text, imageName,
                                                           position.Latitude, position.Longitude));

                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            }
        }

        private async void ShowTags(object sender, EventArgs e)
        {
            Position position = await CrossGeolocator.Current.GetPositionAsync();
            await Navigation.PushAsync(new Tags(position.Latitude,position.Longitude));
        }
    }
}
