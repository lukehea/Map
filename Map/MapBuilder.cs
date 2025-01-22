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
using System.Collections;
using static Map.Utilities.Exceptions;

namespace Map
{
    internal class MapBuilder
    {
        private static string API_key = "";
        private static HttpClient http = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };
        private string sessionID;
        private double zoom; // zoom level (0 -> 22)
        private Point latLng; // current latitude/longitude of the centre of the map
        public MapBuilder() {
            sessionID = "none";
            zoom = 0;
            latLng = new Point(0, 0);
        }

        public double Latitude => latLng.Y;

        public double Longitude => latLng.X;

        // converts from 0 -> 22 range (for maps tile api) to 0 -> 100 range
        public int Zoom => (int) (zoom * 100/22);

        //public async void getMapTile()
        //{
        //    if (sessionID == "none") ;
        //sessionID = await http.PostAsync(
        //    $"'{{\"mapType\": \"streetview\", \"language\": \"en-US\", \"region\": \"CA\"}}' " +
        //    "- H 'Content-Type: application/json " +
        //    "https://tile.googleapis.com/v1/createSession?key={API_key}"
        //);
        //}

        public void UpdateZoom(int update_val)
        {
            zoom += (double) update_val * 22/100;
            if (zoom < 0)
                zoom = 0;
            else if (zoom > 22)
                zoom = 22;
        }


        public void UpdateLngLat(Point start, Point finish)
        {
            // throws an error if coordinates have not been normalized to a 1x1 plane
            if(start.X > 1 || start.X < 0 ||
                start.Y > 1 || start.Y < 0 ||
                finish.X > 1 || finish.X < 0 ||
                finish.Y > 1 || finish.Y < 0)
            {
                throw new NonUnitPointException();
            }

            Point start_latlng = PointToLatLng(start);
            Point finish_latlng = PointToLatLng(finish);

            Vector start_to_centre = start_latlng - latLng;

            latLng = finish_latlng + start_to_centre;
        }

        private Point PointToLatLng(Point p)
        {
            Point return_val = new Point();

            double length = Math.Pow(2, zoom);

            return_val.X = (360 * p.X / length) - 180;

            double mercN = ((length / 2) - p.Y) * (2 * Math.PI) / length;
            double latRad = 2 * (Math.Atan(Math.Pow(Math.E, mercN)) - (Math.PI / 4));
            return_val.Y = (180 * latRad) / Math.PI;

            return return_val;
        }

        public ImageSource GetMapTile(int x, int y)
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
