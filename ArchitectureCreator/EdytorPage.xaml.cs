using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArchitectureCreator
{
    /// <summary>
    /// Logika interakcji dla klasy EdytorPage.xaml
    /// </summary>
    public partial class EdytorPage : Page
    {
        private Canvas _selectedElement;
        private Point _clickPosition;
        private bool _isDragging = false;
        public List<Element> elements { get; set; }
        public EdytorPage(float roomWidthfloat, float roomHeightfloat)
        {
            InitializeComponent();
            elements = FileManager.LoadElements();
            FileManager.AddImagePath(elements);
            DataContext = this;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(DrawingCanvas);
            _selectedElement = GetElementAtPosition(position);

            if (_selectedElement != null)
            {
                // Rozpocznij przesuwanie elementu
                _isDragging = true;
                _clickPosition = position;
                DrawingCanvas.CaptureMouse();
            }
            else if (!IsPositionOccupied(position))
            {
                // Utwórz nowy element (prostokąt z trójkątem)
                Canvas newElement = CreateRectangleWithTriangle(position);
                DrawingCanvas.Children.Add(newElement);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _selectedElement != null)
            {
                Point position = e.GetPosition(DrawingCanvas);
                double offsetX = position.X - _clickPosition.X;
                double offsetY = position.Y - _clickPosition.Y;

                double newLeft = Canvas.GetLeft(_selectedElement) + offsetX;
                double newTop = Canvas.GetTop(_selectedElement) + offsetY;

                // Aktualizuj pozycję elementu
                Canvas.SetLeft(_selectedElement, newLeft);
                Canvas.SetTop(_selectedElement, newTop);

                _clickPosition = position;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            _selectedElement = null;
            DrawingCanvas.ReleaseMouseCapture();
        }

        private Canvas GetElementAtPosition(Point position)
        {
            foreach (UIElement element in DrawingCanvas.Children)
            {
                if (element is Canvas canvasElement)
                {
                    double left = Canvas.GetLeft(canvasElement);
                    double top = Canvas.GetTop(canvasElement);
                    double right = left + canvasElement.Width;
                    double bottom = top + canvasElement.Height;

                    if (position.X >= left && position.X <= right && position.Y >= top && position.Y <= bottom)
                    {
                        return canvasElement;
                    }
                }
            }
            return null;
        }

        private bool IsPositionOccupied(Point position)
        {
            foreach (UIElement element in DrawingCanvas.Children)
            {
                if (element is Canvas canvasElement)
                {
                    double left = Canvas.GetLeft(canvasElement);
                    double top = Canvas.GetTop(canvasElement);
                    double right = left + canvasElement.Width;
                    double bottom = top + canvasElement.Height;

                    if (position.X >= left && position.X <= right && position.Y >= top && position.Y <= bottom)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Canvas CreateRectangleWithTriangle(Point position)
        {
            // Tworzenie kontenera Canvas
            Canvas container = new Canvas
            {
                Width = 100,
                Height = 130 // Wysokość większa, aby pomieścić trójkąt
            };

            // Tworzenie prostokąta
            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Blue
            };
            Canvas.SetTop(rectangle, 30); // Ustawienie prostokąta poniżej trójkąta

            // Tworzenie trójkąta
            Polygon triangle = new Polygon
            {
                Points = new PointCollection(new Point[]
                {
                    new Point(0, 30),
                    new Point(50, 0),
                    new Point(100, 30)
                }),
                Fill = Brushes.Green
            };

            // Dodanie prostokąta i trójkąta do kontenera
            container.Children.Add(rectangle);
            container.Children.Add(triangle);

            // Ustawienie pozycji kontenera na głównym Canvas
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }

        private void WorkspaceCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Zmiana skali w zależności od scrolla
            var scaleTransform = (ScaleTransform)DrawingCanvas.LayoutTransform;
            double zoom = e.Delta > 0 ? 1.1 : 1 / 1.1;
            scaleTransform.ScaleX *= zoom;
            scaleTransform.ScaleY *= zoom;
        }
    }
}
