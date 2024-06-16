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
        public double width { get; set; }
        public double height { get; set; }

        public Element(string category, string name, string description, string image, double width, double height)
        {
            this.category = category;
            this.name = name;
            this.description = description;
            this.image = image;
            this.width = width;
            this.height = height;
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

    public class Chair : Element
    {
        public Chair(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            double legSize = 10; // stały rozmiar nóg krzesła
            double seatMargin = 20; // margines od krawędzi dla siedziska krzesła
            double backrestHeight = 20; // wysokość oparcia krzesła
            double seatWidth = width - 2 * seatMargin; // szerokość siedziska krzesła
            double seatHeight = height - backrestHeight - seatMargin; // wysokość siedziska krzesła

            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width,
                Height = height
            };

            // Siedzisko krzesła
            Rectangle seat = new Rectangle
            {
                Width = seatWidth,
                Height = seatHeight,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(seat, seatMargin); // środek kontenera, z uwzględnieniem nóg
            Canvas.SetTop(seat, backrestHeight);  // środek kontenera, z uwzględnieniem oparcia
            container.Children.Add(seat);

            // Nogi krzesła (lewa górna)
            Rectangle leg1 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg1, seatMargin - legSize / 2); // lewy margines
            Canvas.SetTop(leg1, backrestHeight - legSize / 2); // górny margines
            container.Children.Add(leg1);

            // Nogi krzesła (prawa górna)
            Rectangle leg2 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg2, seatMargin + seatWidth - legSize / 2); // prawy margines
            Canvas.SetTop(leg2, backrestHeight - legSize / 2); // górny margines
            container.Children.Add(leg2);

            // Nogi krzesła (lewa dolna)
            Rectangle leg3 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg3, seatMargin - legSize / 2); // lewy margines
            Canvas.SetTop(leg3, backrestHeight + seatHeight - legSize / 2); // dolny margines
            container.Children.Add(leg3);

            // Nogi krzesła (prawa dolna)
            Rectangle leg4 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg4, seatMargin + seatWidth - legSize / 2); // prawy margines
            Canvas.SetTop(leg4, backrestHeight + seatHeight - legSize / 2); // dolny margines
            container.Children.Add(leg4);

            // Oparcie krzesła
            Rectangle backrest = new Rectangle
            {
                Width = seatWidth,
                Height = backrestHeight,
                Fill = Brushes.DarkGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(backrest, seatMargin); // środek kontenera, z uwzględnieniem nóg
            Canvas.SetTop(backrest, 0); // górny margines, nad siedziskiem
            container.Children.Add(backrest);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Desk : Element
    {
        public Desk(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            double legSize = 10; // stały rozmiar nóg biurka
            double topMargin = 10; // margines od góry dla blatu biurka i nóg
            double sideMargin = 10; // margines od boku dla blatu biurka i nóg
            double deskTopWidth = width - 2 * sideMargin; // szerokość blatu biurka
            double deskTopHeight = height - 2 * topMargin; // wysokość blatu biurka

            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width,
                Height = height
            };

            // Blat biurka
            Rectangle deskTop = new Rectangle
            {
                Width = deskTopWidth,
                Height = deskTopHeight,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(deskTop, sideMargin); // środek kontenera, z uwzględnieniem nóg
            Canvas.SetTop(deskTop, topMargin);   // środek kontenera, z uwzględnieniem nóg
            container.Children.Add(deskTop);

            // Nogi biurka (lewa górna)
            Rectangle leg1 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg1, sideMargin - legSize / 2); // lewy margines
            Canvas.SetTop(leg1, topMargin - legSize / 2);   // górny margines
            container.Children.Add(leg1);

            // Nogi biurka (prawa górna)
            Rectangle leg2 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg2, sideMargin + deskTopWidth - legSize / 2); // prawy margines
            Canvas.SetTop(leg2, topMargin - legSize / 2);                  // górny margines
            container.Children.Add(leg2);

            // Nogi biurka (lewa dolna)
            Rectangle leg3 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg3, sideMargin - legSize / 2); // lewy margines
            Canvas.SetTop(leg3, topMargin + deskTopHeight - legSize / 2); // dolny margines
            container.Children.Add(leg3);

            // Nogi biurka (prawa dolna)
            Rectangle leg4 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg4, sideMargin + deskTopWidth - legSize / 2); // prawy margines
            Canvas.SetTop(leg4, topMargin + deskTopHeight - legSize / 2);  // dolny margines
            container.Children.Add(leg4);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Door : Element
    {
        public Door(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width + height,
                Height = height
            };

            // Drzwi
            Rectangle door = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(door, 0); // ustawienie drzwi na krawędzi kontenera
            Canvas.SetTop(door, 0);
            container.Children.Add(door);

            // Łuk otwierania drzwi
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(width, 0)
            };
            ArcSegment arcSegment = new ArcSegment
            {
                Size = new Size(height, height),
                SweepDirection = SweepDirection.Clockwise,
                Point = new Point(width + height, height)
            };
            pathFigure.Segments.Add(arcSegment);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 1,
                Data = pathGeometry
            };
            Canvas.SetLeft(path, 0);
            Canvas.SetTop(path, 0);
            container.Children.Add(path);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class WindowO : Element
    {
        public WindowO(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width + 20,  // dodanie marginesów
                Height = height + 20 // dodanie marginesów
            };

            // Rama okna
            Rectangle windowFrame = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.WhiteSmoke,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(windowFrame, 10); // margines od lewej
            Canvas.SetTop(windowFrame, 10);  // margines od góry
            container.Children.Add(windowFrame);

            // Szyby okna (pozioma linia)
            Line horizontalLine = new Line
            {
                X1 = 10,
                Y1 = height / 2 + 10,
                X2 = width + 10,
                Y2 = height / 2 + 10,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            container.Children.Add(horizontalLine);

            // Szyby okna (pionowa linia)
            Line verticalLine = new Line
            {
                X1 = width / 2 + 10,
                Y1 = 10,
                X2 = width / 2 + 10,
                Y2 = height + 10,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            container.Children.Add(verticalLine);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Dresser : Element
    {
        public Dresser(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            double legSize = 10; // stały rozmiar nóg komody
            double bodyMargin = 10; // margines wokół korpusu komody
            double bodyWidth = width - 2 * bodyMargin; // szerokość korpusu komody
            double bodyHeight = height - legSize - bodyMargin; // wysokość korpusu komody
            double drawerHeight = (bodyHeight - 3 * bodyMargin) / 3; // wysokość każdej szuflady

            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width,
                Height = height
            };

            // Korpus komody
            Rectangle dresserBody = new Rectangle
            {
                Width = bodyWidth,
                Height = bodyHeight,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(dresserBody, bodyMargin); // margines od lewej
            Canvas.SetTop(dresserBody, bodyMargin);  // margines od góry
            container.Children.Add(dresserBody);

            // Szuflada 1
            Rectangle drawer1 = new Rectangle
            {
                Width = bodyWidth - 2 * bodyMargin,
                Height = drawerHeight,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(drawer1, 2 * bodyMargin); // margines od lewej
            Canvas.SetTop(drawer1, bodyMargin); // górna część korpusu
            container.Children.Add(drawer1);

            // Szuflada 2
            Rectangle drawer2 = new Rectangle
            {
                Width = bodyWidth - 2 * bodyMargin,
                Height = drawerHeight,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(drawer2, 2 * bodyMargin); // margines od lewej
            Canvas.SetTop(drawer2, 2 * bodyMargin + drawerHeight); // środkowa część korpusu
            container.Children.Add(drawer2);

            // Szuflada 3
            Rectangle drawer3 = new Rectangle
            {
                Width = bodyWidth - 2 * bodyMargin,
                Height = drawerHeight,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(drawer3, 2 * bodyMargin); // margines od lewej
            Canvas.SetTop(drawer3, 3 * bodyMargin + 2 * drawerHeight); // dolna część korpusu
            container.Children.Add(drawer3);

            // Nogi komody (lewa dolna)
            Rectangle leg3 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg3, bodyMargin); // lewy margines
            Canvas.SetTop(leg3, height - legSize); // dolny margines przylegający do korpusu
            container.Children.Add(leg3);

            // Nogi komody (prawa dolna)
            Rectangle leg4 = new Rectangle
            {
                Width = legSize,
                Height = legSize,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leg4, width - bodyMargin - legSize); // prawy margines
            Canvas.SetTop(leg4, height - legSize); // dolny margines przylegający do korpusu
            container.Children.Add(leg4);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Wardrobe : Element
    {
        public Wardrobe(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            double partWidth = (width - 20) / 2; // szerokość każdej z głównych części szafy
            double partHeight = height - 20; // wysokość każdej z głównych części szafy
            double doorWidth = partWidth - 10; // szerokość drzwi szafy
            double doorHeight = partHeight - 10; // wysokość drzwi szafy

            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width,
                Height = height
            };

            // Główna część szafy (lewa strona)
            Rectangle leftWardrobePart = new Rectangle
            {
                Width = partWidth,
                Height = partHeight,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leftWardrobePart, 10); // lewy margines
            Canvas.SetTop(leftWardrobePart, 10);  // górny margines
            container.Children.Add(leftWardrobePart);

            // Główna część szafy (prawa strona)
            Rectangle rightWardrobePart = new Rectangle
            {
                Width = partWidth,
                Height = partHeight,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(rightWardrobePart, 10 + partWidth); // lewy margines dla prawej części
            Canvas.SetTop(rightWardrobePart, 10);   // górny margines
            container.Children.Add(rightWardrobePart);

            // Drzwi szafy (lewa strona)
            Rectangle leftDoor = new Rectangle
            {
                Width = doorWidth,
                Height = doorHeight,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(leftDoor, 15); // wewnętrzny margines od lewej części
            Canvas.SetTop(leftDoor, 15);  // wewnętrzny margines od góry
            container.Children.Add(leftDoor);

            // Drzwi szafy (prawa strona)
            Rectangle rightDoor = new Rectangle
            {
                Width = doorWidth,
                Height = doorHeight,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(rightDoor, 15 + partWidth); // wewnętrzny margines od prawej części
            Canvas.SetTop(rightDoor, 15);   // wewnętrzny margines od góry
            container.Children.Add(rightDoor);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
    public class Bed : Element
    {
        public Bed(string category, string name, string description, string image, double width, double height)
            : base(category, name, description, image, width, height) { }

        public override Canvas CreateShape(Point position)
        {
            // Tworzenie kontenera o odpowiednim rozmiarze
            Canvas container = new Canvas
            {
                Width = width + 20,  // dodanie marginesów
                Height = height + 20 // dodanie marginesów
            };

            // Rama łóżka
            Rectangle bedFrame = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(bedFrame, 10); // margines od lewej
            Canvas.SetTop(bedFrame, 10);  // margines od góry
            container.Children.Add(bedFrame);

            // Materac
            Rectangle mattress = new Rectangle
            {
                Width = width - 20, // marginesy wewnętrzne
                Height = height - 20, // marginesy wewnętrzne
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(mattress, 20); // margines wewnętrzny od lewej ramy
            Canvas.SetTop(mattress, 20);  // margines wewnętrzny od góry ramy
            container.Children.Add(mattress);

            // Poduszka
            Rectangle pillow = new Rectangle
            {
                Width = width - 40, // poduszka powinna być węższa niż materac
                Height = 20,
                Fill = Brushes.White,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(pillow, 30); // centrowanie poduszki na materacu
            Canvas.SetTop(pillow, 20);   // umieszczenie poduszki na górnej części materaca
            container.Children.Add(pillow);

            // Kołdra
            Rectangle blanket = new Rectangle
            {
                Width = width - 20,
                Height = height - 60, // kołdra powinna zakrywać większość materaca
                Fill = Brushes.DarkGray,
                Stroke = Brushes.Black
            };
            Canvas.SetLeft(blanket, 20); // margines wewnętrzny od lewej ramy
            Canvas.SetTop(blanket, 40);  // umieszczenie kołdry poniżej poduszki
            container.Children.Add(blanket);

            // Ustawienie pozycji kontenera
            Canvas.SetLeft(container, position.X);
            Canvas.SetTop(container, position.Y);

            return container;
        }
    }
}
