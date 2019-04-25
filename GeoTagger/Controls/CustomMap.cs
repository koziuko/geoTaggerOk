using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace GeoTagger.Controls
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
