using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Map
{
    internal class Controller
    {
        MapBuilder map_builder;
        MainWindow view;

        public Controller(MainWindow window)
        {
            map_builder = new MapBuilder();
            view = window;

            UpdateLabels();
        }

        public void MouseDragged(Point start, Point end)
        {
            // checks that the drag started and ended in the bounds of the map
            if (start.Y < view.MapLocation.Y || start.Y > view.MapDimensions.Y + view.MapLocation.Y ||
                    end.Y < view.MapLocation.Y || end.Y > view.MapDimensions.Y + view.MapLocation.Y ||
                    start.X < view.MapLocation.X || start.X > view.MapDimensions.X + view.MapLocation.X ||
                    end.X < view.MapLocation.X || end.X > view.MapDimensions.X + view.MapLocation.X
                )
            {
                return;
            }

            // updates the start and end points from the map dimensions to a 1x1 plane
            start.X = (start.X - view.MapLocation.X) / view.MapDimensions.X;
            start.Y = (start.Y - view.MapLocation.Y) / view.MapDimensions.Y;

            end.X = (end.X - view.MapLocation.X) / view.MapDimensions.X;
            end.Y = (end.Y - view.MapLocation.Y) / view.MapDimensions.Y;

            // latitude and longitude are only updated if total movement is more than 3% of map length
            if (Math.Abs((start - end).X) + Math.Abs((start - end).Y) > 0.03)
            {
                // start and end points are reversed since the map should shift opposite to the drag direction
                map_builder.UpdateLngLat(end, start);

                view.UpdateLabels(map_builder.Latitude, map_builder.Longitude, map_builder.Zoom);
            }
                
        }

        public void MouseWheel(int direction)
        {
            map_builder.UpdateZoom(direction);
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            view.UpdateLabels(map_builder.Latitude, map_builder.Longitude, map_builder.Zoom);
        }
    }
}
