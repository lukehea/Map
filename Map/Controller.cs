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
        }

        public void MouseDragged(Point start, Point finish)
        {

        }

        public void MouseWheel(int direction)
        {

        }

        private void UpdateLabels()
        {
            view.UpdateLabels(map_builder.Latitude, map_builder.Longitude, map_builder.Zoom);
        }
    }
}
