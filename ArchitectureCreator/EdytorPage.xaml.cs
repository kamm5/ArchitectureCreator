using Newtonsoft.Json;
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
using System.Xml.Linq;
using System.IO;

namespace ArchitectureCreator
{
    /// <summary>
    /// Logika interakcji dla klasy EdytorPage.xaml
    /// </summary>
    public class SaveProjectClass
    {
        public float width { get; set; }
        public float height { get; set; }
        public List<CanvasElement> canvasElements { get; set; }

        public SaveProjectClass(float roomWidth, float roomHeight)
        {
            width = roomWidth;
            height = roomHeight;
            canvasElements = new List<CanvasElement>();
        }
    }

    public class CanvasElement
    {
        public Element etype { get; set; }
        public Point position { get; set; }
        public double angle { get; set; }
        public CanvasElement(Element etype, Point position, double angle)
        {
            this.etype = etype;
            this.position = position;
            this.angle = angle;
        }
    }

    public partial class EdytorPage : Page
    {
        private Canvas _selectedElement;
        private Point _clickPosition;
        private bool _isDragging = false;
        private double _currentAngle = 0;
        public List<Element> elements { get; set; }
        public List<CanvasElement> canvasElements = new List<CanvasElement>();
        public Element selectedElement;
        public SaveProjectClass saveProjectClass;

        private void SetCanvas(float roomWidthfloat, float roomHeightfloat)
        {
            DrawingCanvas.Width = roomWidthfloat * 100;
            DrawingCanvas.Height = roomHeightfloat * 100;
            CanvasBorder.Width = roomWidthfloat * 100;
            CanvasBorder.Height = roomHeightfloat * 100;
            saveProjectClass = new SaveProjectClass(roomWidthfloat, roomHeightfloat);
        }

        public EdytorPage(float roomWidthfloat, float roomHeightfloat)
        {
            InitializeComponent();
            elements = FileManager.LoadElements();
            DataContext = this;
            SetCanvas(roomWidthfloat, roomHeightfloat);
            this.KeyDown += MainWindow_KeyDown;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                selectedElement = e.AddedItems[0] as Element;
            }
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
                if (selectedElement != null)
                {
                    var newElement = selectedElement.CreateShape(position);
                    DrawingCanvas.Children.Add(newElement);
                    saveProjectClass.canvasElements.Add(new CanvasElement(selectedElement, position, 0));
                }
                else
                {
                    MessageBox.Show("Najpierw wybierz element");
                }
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
            Point pointTemp = new Point(Canvas.GetLeft(_selectedElement), Canvas.GetTop(_selectedElement));
            int j = 0;
            for (int i = 0; i < saveProjectClass.canvasElements.Count; i++)
            {
                if (saveProjectClass.canvasElements[i].position == pointTemp)
                {
                    j = i;
                }
            }
            pointTemp = new Point(Canvas.GetLeft(_selectedElement) + offsetX, Canvas.GetTop(_selectedElement) + offsetY);
            Canvas.SetLeft(_selectedElement, pointTemp.X);
            Canvas.SetTop(_selectedElement, pointTemp.Y);

            saveProjectClass.canvasElements[j].position = pointTemp;
            _clickPosition = position;
        }

        private void RotateElement(int delta)
        {
            Point pointTemp = new Point(Canvas.GetLeft(_selectedElement), Canvas.GetTop(_selectedElement));
            int j = 0;
            for (int i = 0; i < saveProjectClass.canvasElements.Count; i++)
            {
                if (saveProjectClass.canvasElements[i].position == pointTemp)
                {
                    j = i;
                }
            }
            _currentAngle += delta > 0 ? 5 : -5;
            saveProjectClass.canvasElements[j].angle= _currentAngle;
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
            return DrawingCanvas.Children.OfType<Canvas>().FirstOrDefault(canvas => IsPointInsideElement(position, canvas));
        }

        private bool IsPositionOccupied(Point position)
        {
            return DrawingCanvas.Children.OfType<Canvas>().Any(canvas => IsPointInsideElement(position, canvas));
        }

        private bool IsPointInsideElement(Point point, UIElement element)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            double right = left + element.RenderSize.Width;
            double bottom = top + element.RenderSize.Height;

            return point.X >= left && point.X <= right && point.Y >= top && point.Y <= bottom;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isDragging && _selectedElement != null)
            {
                if (e.Key == Key.Back || e.Key == Key.Delete)
                {
                    DrawingCanvas.Children.Remove(_selectedElement);
                    _selectedElement = null;
                    _isDragging = false;
                }
            }
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            /*var elements = new List<CanvasElement>();
            foreach (UIElement element in DrawingCanvas.Children)
            {
                MessageBox.Show(element.ToString());
                if (element is Canvas canvasElement)
                {
                    // Zapisz tylko lokalizację i kąt obrotu
                    double left = Canvas.GetLeft(canvasElement);
                    double top = Canvas.GetTop(canvasElement);
                    double angle = ((RotateTransform)canvasElement.RenderTransform)?.Angle ?? 0;

                    elements.Add(new CanvasElement
                    {
                        Left = left,
                        Top = top,
                        Angle = angle
                    });
                }
            }

            var json = JsonConvert.SerializeObject(elements, Formatting.Indented);
            File.WriteAllText("canvasProject.json", json);*/
            /*List<Canvas> elements1 = new List<Canvas>();
            foreach (UIElement element in DrawingCanvas.Children)
            {
                if (element is Canvas canvasElement)
                {
                    elements1.Add(canvasElement);
                    MessageBox.Show(elements1.ToString());
                }
            }*/
            string json = JsonConvert.SerializeObject(saveProjectClass, Formatting.Indented);
            File.WriteAllText("canvasProject.json", json);
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("canvasProject.json"))
            {
                string json = File.ReadAllText("canvasProject.json");
                SaveProjectClass savedProjekt = JsonConvert.DeserializeObject<SaveProjectClass>(json);
                var garbage = DrawingCanvas.Children.OfType<UIElement>().ToList();
                foreach (UIElement garbageElement in garbage)
                {
                    if (!(garbageElement is Border))
                        DrawingCanvas.Children.Remove(garbageElement);
                }

                DrawingCanvas.Width = savedProjekt.width * 100;
                DrawingCanvas.Height = savedProjekt.height * 100;
                CanvasBorder.Width = savedProjekt.width * 100;
                CanvasBorder.Height = savedProjekt.height * 100;
                saveProjectClass = null;
                saveProjectClass = new SaveProjectClass(savedProjekt.width, savedProjekt.height);
                List<CanvasElement> savedProjektCanvas = FileManager.SorterElements(savedProjekt.canvasElements);
                foreach (CanvasElement loadElement in savedProjektCanvas)
                {
                    Canvas newElement = loadElement.etype.CreateShape(loadElement.position);
                    newElement.RenderTransform = new RotateTransform(loadElement.angle, newElement.Width / 2, newElement.Height / 2);
                    DrawingCanvas.Children.Add(newElement);
                    saveProjectClass.canvasElements.Add(new CanvasElement(loadElement.etype, loadElement.position, loadElement.angle));
                }
            }
        }
    }
}
