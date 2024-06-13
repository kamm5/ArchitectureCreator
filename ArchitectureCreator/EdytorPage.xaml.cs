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
        private double _currentAngle = 0;
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
                StartDragging(position);
            }
            else if (!IsPositionOccupied(position))
            {
                CreateAndAddElement(position);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _selectedElement != null)
            {
                DragElement(e.GetPosition(DrawingCanvas));
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void WorkspaceCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_isDragging && _selectedElement != null)
            {
                RotateElement(e.Delta);
            }
            else
            {
                ZoomCanvas(e.Delta);
            }
        }

        private void StartDragging(Point position)
        {
            _isDragging = true;
            _clickPosition = position;
            DrawingCanvas.CaptureMouse();
        }

        private void StopDragging()
        {
            _isDragging = false;
            _selectedElement = null;
            DrawingCanvas.ReleaseMouseCapture();
        }

        private void DragElement(Point position)
        {
            double offsetX = position.X - _clickPosition.X;
            double offsetY = position.Y - _clickPosition.Y;

            Canvas.SetLeft(_selectedElement, Canvas.GetLeft(_selectedElement) + offsetX);
            Canvas.SetTop(_selectedElement, Canvas.GetTop(_selectedElement) + offsetY);

            _clickPosition = position;
        }

        private void RotateElement(int delta)
        {
            _currentAngle += delta > 0 ? 5 : -5;
            _selectedElement.RenderTransform = new RotateTransform(_currentAngle, _selectedElement.Width / 2, _selectedElement.Height / 2);
        }

        private void ZoomCanvas(int delta)
        {
            var scaleTransform = (ScaleTransform)DrawingCanvas.LayoutTransform;
            double zoom = delta > 0 ? 1.1 : 1 / 1.1;
            scaleTransform.ScaleX *= zoom;
            scaleTransform.ScaleY *= zoom;
        }

        private Canvas GetElementAtPosition(Point position)
        {
            return DrawingCanvas.Children
                .OfType<Canvas>()
                .FirstOrDefault(canvas => IsPointInsideElement(position, canvas));
        }

        private bool IsPositionOccupied(Point position)
        {
            return DrawingCanvas.Children
                .OfType<Canvas>()
                .Any(canvas => IsPointInsideElement(position, canvas));
        }

        private bool IsPointInsideElement(Point point, UIElement element)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            double right = left + element.RenderSize.Width;
            double bottom = top + element.RenderSize.Height;

            return point.X >= left && point.X <= right && point.Y >= top && point.Y <= bottom;
        }

        private void CreateAndAddElement(Point position)
        {
            var newElement = CreateRectangleWithTriangle(position);
            DrawingCanvas.Children.Add(newElement);
        }

        private Canvas CreateRectangleWithTriangle(Point position)
        {
            Canvas container = new Canvas
            {
                Width = 100,
                Height = 130 // Wysokość większa, aby pomieścić trójkąt
            };

            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black
            };
            Canvas.SetTop(rectangle, 30);

            Polygon triangle = new Polygon
            {
                Points = new PointCollection(new Point[]
                {
                    new Point(0, 30),
                    new Point(50, 0),
                    new Point(100, 30)
                }),
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black
            };

            container.Children.Add(rectangle);
            container.Children.Add(triangle);

            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
}
