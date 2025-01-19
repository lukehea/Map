using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics.Eventing.Reader;

namespace Map
{
    internal class MapBuilder
    {
        private static string API_key = "AIzaSyCcuduMGTk27vlyVhTUJsUdcHKqlVUZSU8";
        private static HttpClient http = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };
        private string sessionID;
        private int zoom;
        private Point lngLat;
        public MapBuilder() {
            sessionID = "none";
            zoom = 0;
            lngLat = new Point(0, 0);
        }

        public double getLatitude()
        {
            return lngLat.Y;
        }

        public double getLongitude()
        {
            return lngLat.X;
        }

        public int getZoom()
        {
            return zoom;
        }

        //public async void getMapTile()
        //{
        //    if (sessionID == "none") ;
                //sessionID = await http.PostAsync(
                //    $"'{{\"mapType\": \"streetview\", \"language\": \"en-US\", \"region\": \"CA\"}}' " +
                //    "- H 'Content-Type: application/json " +
                //    "https://tile.googleapis.com/v1/createSession?key={API_key}"
                //);
        //}

        public void updateZoom(int update_val)
        {
            zoom += update_val;
            if (zoom < 0)
                zoom = 0;
            else if (zoom > 22)
                zoom = 22;
        }

        public void updateLngLat(Point distance)
        {
            double lat_modifier = distance.Y * 360 / (800 * (Math.Pow(2, zoom)));
            double lng_modifier = -distance.X * 360 / (800 * (Math.Pow(2, zoom)));
            lngLat.X += lng_modifier;
            lngLat.Y += lat_modifier;
            lngLat.X = lngLat.X % 180;
            lngLat.Y = lngLat.Y % 180;

        }

        public ImageSource getMapTile(int x, int y)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            if (zoom % 2 == 0)
                bi3.UriSource = new Uri("\"Images\\Image1.png\"", UriKind.Relative);
            else
                bi3.UriSource = new Uri("\"Images\\image2.png\"", UriKind.Relative);
            bi3.EndInit();

            return bi3;
        }
    }
}
