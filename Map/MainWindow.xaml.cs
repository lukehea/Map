using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Map
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MapBuilder map_builder;
        private Point mouse_down_loc;
        private ObservableCollection<ObservableCollection<Image>> image_grid;

        public MainWindow()
        {

            InitializeComponent();

            map_builder = new MapBuilder();

            image_grid = new ObservableCollection<ObservableCollection<Image>>();
            for(int i = 0; i < 4; ++i)
            {
                image_grid.Add(new ObservableCollection<Image>());
                for(int j = 0; j < 4; ++j)
                {
                    image_grid[i].Add(new Image());
                    Grid.SetColumn(image_grid[i][j], i);
                    Grid.SetRow(image_grid[i][j], j);

                   mapGrid.RegisterName($"image_{i}{j}", image_grid[i][j]);
                   mapGrid.Children.Add(image_grid[i][j]);
                }
            }

            updateMap();

            updateLabels();
        }

        private void updateLabels()
        {
            latitudeLabel.Content = $"Latitude: {map_builder.getLatitude()}";
            longitudeLabel.Content = $"Longitude: {map_builder.getLongitude()}";
            zoomLabel.Content = $"Zoom: {map_builder.getZoom()}";
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            map_builder.updateZoom((e.Delta > 0) ? 1 : -1);
            updateMap();
            updateLabels();
        }

        private void mapGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point drag_distance = (Point)((Point) e.GetPosition(this) - mouse_down_loc);
            if (Math.Abs(drag_distance.X) + Math.Abs(drag_distance.Y) > 10)
            {
                map_builder.updateLngLat(drag_distance);
                //MessageBox.Show($"Dragged {drag_distance.ToString()}");
            }
            updateMap();
            updateLabels();
        }

        private void mapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouse_down_loc = e.GetPosition(this);
        }

        private void updateMap()
        {
            for (int i = 0; i < image_grid.Count; ++i)
            {
                for (int j = 0; j < image_grid[i].Count; ++j)
                {
                    image_grid[i][j].Source = map_builder.getMapTile(i, j);
                }
            }
        }
    }
}