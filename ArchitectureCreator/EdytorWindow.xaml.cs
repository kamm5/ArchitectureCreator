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
    public partial class EdytorWindow : Window
    {
        public List<Element> elements { get; set; }
        public EdytorWindow()
        {
            InitializeComponent();
            elements = FileManager.LoadElements();
            FileManager.AddImagePath(elements);
            DataContext = this;
        }
    }
}
