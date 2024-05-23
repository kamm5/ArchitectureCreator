using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace ArchitectureCreator
{
    /// <summary>
    /// Logika interakcji dla klasy EdytorWindow.xaml
    /// </summary>
    public class ImageItem
    {
        public string ImagePath { get; set; }
        public string Description { get; set; }

        public ImageItem(string imagePath, string description)
        {
            ImagePath = imagePath;
            Description = description;
        }
    }
    public partial class EdytorWindow : Window
    {
        public List<ImageItem> ImageItems { get; set; }
        public EdytorWindow()
        {
            InitializeComponent();
            ImageItems = new List<ImageItem>
            {
                new ImageItem(System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "image1.jpg"), "Description 1"),
                new ImageItem(System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "image1.jpg"), "Description 2"),
                new ImageItem(System.IO.Path.Combine(Environment.CurrentDirectory, "Images", "image1.jpg"), "Description 3")
            };
            DataContext = this;
        }
    }
}
