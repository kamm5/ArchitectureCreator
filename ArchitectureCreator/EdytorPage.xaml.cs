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
        private bool isDragging = false;
        private Point clickPosition;
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
            isDragging = true;
            clickPosition = e.GetPosition(DrawingCanvas);

            Rectangle rect = new Rectangle
            {
                Width = 100,
                Height = 50,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.LightGray
            };

            Canvas.SetLeft(rect, clickPosition.X);
            Canvas.SetTop(rect, clickPosition.Y);
            DrawingCanvas.Children.Add(rect);

            rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            Rectangle rect = sender as Rectangle;
            clickPosition = e.GetPosition(rect);
            rect.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(DrawingCanvas);
                FrameworkElement element = e.OriginalSource as FrameworkElement;

                if (element != null)
                {
                    double left = Canvas.GetLeft(element);
                    double top = Canvas.GetTop(element);

                    Canvas.SetLeft(element, left + (currentPosition.X - clickPosition.X));
                    Canvas.SetTop(element, top + (currentPosition.Y - clickPosition.Y));

                    clickPosition = currentPosition;
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            FrameworkElement element = e.OriginalSource as FrameworkElement;
            if (element != null)
            {
                element.ReleaseMouseCapture();
            }
        }
    }
}
