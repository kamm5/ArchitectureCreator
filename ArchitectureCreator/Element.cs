using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArchitectureCreator
{
    public class Element
    {
        public string category { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }

        public Element(string categoryT, string nameT, string descriptionT, string imageT)
        {
            category = categoryT;
            name = nameT;
            description = descriptionT;
            image = imageT;
        }

        public virtual Canvas CreateShape(Point position)
        {
            Canvas container = new Canvas
            {
                Width = 100,
                Height = 30 // Wysokość większa, aby pomieścić trójkąt
            };

            Polygon triangle = new Polygon
            {
                Points = new PointCollection(new Point[]
                {
                    new Point(0, 30),
                    new Point(50, 0),
                    new Point(100, 30)
                }),
                Fill = Brushes.Red,
                Stroke = Brushes.Black
            };

            container.Children.Add(triangle);

            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }

    public class Stair : Element
    {
        public Stair(string category, string name, string description, string image)
            : base(category, name, description, image) { }

        public override Canvas CreateShape(Point position)
        {
            Canvas container = new Canvas
            {
                Width = 100,
                Height = 30 // Wysokość większa, aby pomieścić trójkąt
            };

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

            container.Children.Add(triangle);

            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Desk : Element
    {
        public Desk(string category, string name, string description, string image)
            : base(category, name, description, image) { }

        public override Canvas CreateShape(Point position)
        {
            Canvas container = new Canvas
            {
                Width = 100,
                Height = 30 // Wysokość większa, aby pomieścić trójkąt
            };

            Polygon triangle = new Polygon
            {
                Points = new PointCollection(new Point[]
                {
                    new Point(0, 30),
                    new Point(50, 0),
                    new Point(100, 30)
                }),
                Fill = Brushes.Blue,
                Stroke = Brushes.Black
            };

            container.Children.Add(triangle);

            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
}
