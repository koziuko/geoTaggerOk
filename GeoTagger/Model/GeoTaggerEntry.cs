using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace GeoTagger.Model
{
    public class GeoTaggerEntry: TableEntity
    {
        public GeoTaggerEntry()
        {
            
        }

        public GeoTaggerEntry(string Text, string Link, double Lat, double Long)
        {
            string key = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();

            this.PartitionKey = key;
            this.RowKey = key;
            this.Text = Text;
            this.Link = Link;
            this.Lat = Lat.ToString();
            this.Long = Long.ToString();
        }

        public string Text { get; set; }
        public string Link { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}
