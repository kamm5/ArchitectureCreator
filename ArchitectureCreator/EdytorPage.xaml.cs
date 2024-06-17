using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private Canvas selectedCanvasElement;
        private Point clickPosition;
        private bool isDragging = false;
        private double currentAngle = 0;
        public List<Element> elements { get; set; }
        public List<CanvasElement> canvasElements = new List<CanvasElement>();
        private Element selectedElement;
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

        public EdytorPage()
        {
            InitializeComponent();
            elements = FileManager.LoadElements();
            DataContext = this;
            SetCanvas(100, 100);
            this.KeyDown += MainWindow_KeyDown;
            AsyncInitialization();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                selectedElement = e.AddedItems[0] as Element;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(DrawingCanvas);
            selectedCanvasElement = GetElementAtPosition(position);

            if (selectedCanvasElement != null)
                StartDragging(position);
            else if (!IsPositionOccupied(position))
            {
                if (selectedElement != null)
                {
                    var newElement = selectedElement.CreateShape(position);
                    DrawingCanvas.Children.Add(newElement);
                    saveProjectClass.canvasElements.Add(new CanvasElement(selectedElement, position, 0));
                }
                else
                    MessageBox.Show("Najpierw wybierz element");
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedCanvasElement != null)
                DragElement(e.GetPosition(DrawingCanvas));
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isDragging && selectedCanvasElement != null)
                RotateElement(e.Delta);
            else
                ZoomCanvas(e.Delta);
        }

        private void StartDragging(Point position)
        {
            isDragging = true;
            clickPosition = position;
            DrawingCanvas.CaptureMouse();
        }

        private void StopDragging()
        {
            isDragging = false;
            selectedCanvasElement = null;
            DrawingCanvas.ReleaseMouseCapture();
        }

        private void DragElement(Point position)
        {
            double offsetX = position.X - clickPosition.X;
            double offsetY = position.Y - clickPosition.Y;
            Point pointTemp = new Point(Canvas.GetLeft(selectedCanvasElement), Canvas.GetTop(selectedCanvasElement));
            int j = 0;
            for (int i = 0; i < saveProjectClass.canvasElements.Count; i++)
            {
                if (saveProjectClass.canvasElements[i].position == pointTemp)
                    j = i;
            }
            pointTemp = new Point(Canvas.GetLeft(selectedCanvasElement) + offsetX, Canvas.GetTop(selectedCanvasElement) + offsetY);
            Canvas.SetLeft(selectedCanvasElement, pointTemp.X);
            Canvas.SetTop(selectedCanvasElement, pointTemp.Y);

            saveProjectClass.canvasElements[j].position = pointTemp;
            clickPosition = position;
        }

        private void RotateElement(int delta)
        {
            Point pointTemp = new Point(Canvas.GetLeft(selectedCanvasElement), Canvas.GetTop(selectedCanvasElement));
            int j = FindElement();
            currentAngle += delta > 0 ? 5 : -5;
            saveProjectClass.canvasElements[j].angle = currentAngle;
            selectedCanvasElement.RenderTransform = new RotateTransform(currentAngle, selectedCanvasElement.Width / 2, selectedCanvasElement.Height / 2);
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
            if (isDragging && selectedCanvasElement != null)
                if (e.Key == Key.Back || e.Key == Key.Delete)
                {
                    int j = FindElement();
                    saveProjectClass.canvasElements.RemoveAt(j);
                    DrawingCanvas.Children.Remove(selectedCanvasElement);
                    selectedCanvasElement = null;
                    isDragging = false;
                }
        }

        private int FindElement()
        {
            Point pointTemp = new Point(Canvas.GetLeft(selectedCanvasElement), Canvas.GetTop(selectedCanvasElement));
            int j = 0;
            for (int i = 0; i < saveProjectClass.canvasElements.Count; i++)
            {
                if (saveProjectClass.canvasElements[i].position == pointTemp)
                    j = i;
            }

            return j;
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(saveProjectClass, Formatting.Indented);
            File.WriteAllText("canvasProject.json", json);
        }

        private async void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("canvasProject.json"))
            {
                await RenderOpenedProject();
            }
            else
                MessageBox.Show("Brak zapisanego projektu");
        }

        private async void AsyncInitialization()
        {
            await RenderOpenedProject();
        }

        private async Task RenderOpenedProject()
        {
            string json = await ReadFileAsync("canvasProject.json");
            SaveProjectClass savedProjekt = JsonConvert.DeserializeObject<SaveProjectClass>(json);

            await Task.Run(() =>
            {
                List<UIElement> garbage = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    garbage = DrawingCanvas.Children.OfType<UIElement>().Where(e => !(e is Border)).ToList();
                });

                Parallel.ForEach(garbage, garbageElement =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DrawingCanvas.Children.Remove(garbageElement);
                    });
                });
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                DrawingCanvas.Width = savedProjekt.width * 100;
                DrawingCanvas.Height = savedProjekt.height * 100;
                CanvasBorder.Width = savedProjekt.width * 100;
                CanvasBorder.Height = savedProjekt.height * 100;
                saveProjectClass = new SaveProjectClass(savedProjekt.width, savedProjekt.height);
            });

            List<CanvasElement> savedProjektCanvas = await Task.Run(() => FileManager.SorterElements(savedProjekt.canvasElements));

            await Task.Run(() =>
            {
                Parallel.ForEach(savedProjektCanvas, loadElement =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Canvas newElement = loadElement.etype.CreateShape(loadElement.position);
                        newElement.RenderTransform = new RotateTransform(loadElement.angle, newElement.Width / 2, newElement.Height / 2);
                        DrawingCanvas.Children.Add(newElement);
                        saveProjectClass.canvasElements.Add(new CanvasElement(loadElement.etype, loadElement.position, loadElement.angle));
                    });
                });
            });
        }

        private void ExportProject_Click(object sender, RoutedEventArgs e)
        {
            var originalTransform = DrawingCanvas.LayoutTransform;
            DrawingCanvas.LayoutTransform = null;

            Size size = new Size(DrawingCanvas.Width, DrawingCanvas.Height);
            DrawingCanvas.Measure(size);
            DrawingCanvas.Arrange(new Rect(size));

            var renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(DrawingCanvas);

            DrawingCanvas.LayoutTransform = originalTransform;
            DrawingCanvas.Measure(new Size(DrawingCanvas.ActualWidth, DrawingCanvas.ActualHeight));
            DrawingCanvas.Arrange(new Rect(new Point(0, 0), size));
            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var fs = File.OpenWrite("canvas.png"))
                pngEncoder.Save(fs);
        }

        private async Task<string> ReadFileAsync(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
                return await reader.ReadToEndAsync();
        }
    }
}
