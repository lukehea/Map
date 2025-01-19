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
        private Controller controller;
        private Point mouse_down_loc;
        private (double, double) map_dimensions;
        private ObservableCollection<ObservableCollection<Image>> image_grid;

        public MainWindow()
        {

            InitializeComponent();

            map_dimensions = (mapGrid.ActualHeight, mapGrid.ActualWidth);

            controller = new Controller(this);

            image_grid = new ObservableCollection<ObservableCollection<Image>>();
            for (int i = 0; i < 4; ++i)
            {
                image_grid.Add(new ObservableCollection<Image>());
                for (int j = 0; j < 4; ++j)
                {
                    image_grid[i].Add(new Image());
                    Grid.SetColumn(image_grid[i][j], i);
                    Grid.SetRow(image_grid[i][j], j);

                    mapGrid.RegisterName($"image_{i}{j}", image_grid[i][j]);
                    mapGrid.Children.Add(image_grid[i][j]);
                }
            }


        }

        public (double, double) MapDimensions => map_dimensions;

        public void UpdateLabels(double latitude, double longitude, float zoom)
        {
            latitudeLabel.Content = $"Latitude: {latitude}";
            longitudeLabel.Content = $"Longitude: {longitude}";
            zoomLabel.Content = $"Zoom: {zoom}";
        }

        private void MainWindow_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            controller.MouseWheel((e.Delta > 0) ? 1 : -1);
        }

        private void MapGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            controller.MouseDragged(mouse_down_loc, e.GetPosition(this));
        }

        private void MapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouse_down_loc = e.GetPosition(this);
        }

        // tiles is a list of image sources forming a square (throws an error if tiles.Count is not a perfect square)
        // center_image is the index in tiles of the image which should be in the center of the screen
        // center_point is the point in center_image which should be in the center of the screen
        public void UpdateMap(List<ImageSource> tiles, int center_image, float center_point)
        {
            if (Math.Sqrt(tiles.Count) % 1 != 0) {
                throw new NonSquareMapException();
            }

            int image_count = 0;

            for (int i = 0; i < image_grid.Count; ++i)
            {
                for (int j = 0; j < image_grid[i].Count; ++j)
                {
                    image_grid[i][j].Source = controller.getMapTile(i, j);
                }
            }
        }

        public class NonSquareMapException : Exception
        {
            public NonSquareMapException() : base() { }
        }
    }
}