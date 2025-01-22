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
            // converts the update_val from the 0 -> 100 system range to the 0 -> 22 maps API range
            zoom += (double) update_val * 22/100;

            // limits the zoom value to within the 0 -> 22 range
            if (zoom < 0)
                zoom = 0;
            else if (zoom > 22)
                zoom = 22;
        }


        public void UpdateLngLat(Point start, Point finish)
        {
            Point start_latlng = WebMercatorToLatLng(ZoomOffset(start));
            Point finish_latlng = WebMercatorToLatLng(ZoomOffset(finish));

            // generates the vector that transforms the start point to the finish point
            Vector start_to_finish = finish_latlng - start_latlng;

            // uses the vector to transform centre point to the new latitude/longitude
            latLng += start_to_finish;

            FixLatLng();
        }

        // transforms a web mercator point in the 1x1 zoomed map centred on latLng to a point on the complete world map
        private Point ZoomOffset(Point p){
            // gets the x y mercator coordinates of the current centre latitude/longitude            
            Point centre_mercator = LatLngToWebMercator(latLng);

            // gets the offset from the centre of the map to p
            Vector offset = p - new Point(0.5,0.5);

            return centre_mercator + offset;
        }

        private Point WebMercatorToLatLng(Point p)
        {
            Point return_val = new Point();

            double length = Math.Pow(2, zoom);

            return_val.X = (360 * p.X / length) - 180;

            return_val.Y = 360 * Math.Atan(Math.Exp(((length / 2) - p.Y) * (2 * Math.PI) / length)) / Math.PI - 90;

            return return_val;
        }

        private Point LatLngToWebMercator(Point p){
            Point return_val = new Point();

            double length = Math.Pow(2, zoom);

            return_val.X = (p.X + 180) * (length / 360);

            return_val.Y = length * (1 - Math.Log(Math.Tan((Math.PI / 4) + (p.Y * Math.PI / 360))) / Math.PI) / 2;

            return return_val;
        }

        // fixs the latitude/longitude to be within bounds
        // longitude should be in -180 -> 180 range, otherwise remove full cycles, then cycle to the other end of the range
        // latitude should be such that the visible bottom of the map is no more/less than +/-85.05112877980659 degrees (edge of web mercator projection)
        private void FixLatLng(){
            // removes multiples of 360 (full cycles)
            latLng.X = latLng.X % 360;

            // implements wraparound from 180 to -180
            if(latLng.X > 180)
                latLng.X = latLng.X % 180 - 180;
            else if(latLng.X < -180)
                latLng.X = latLng.X % 180 + 180;

            

            
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
