using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace ArchitectureCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Accept(object sender, RoutedEventArgs e)
        {
            if (roomWidthBox.Text != "" && roomHeightBox.Text != "")
                MainFrame.Navigate(new EdytorPage(float.Parse(roomWidthBox.Text), float.Parse(roomHeightBox.Text)));
        }
        private void Button_Open(object sender, RoutedEventArgs e)
        {
            if (File.Exists("canvasProject.json"))
            {
                MainFrame.Navigate(new EdytorPage());
            }
            else
                MessageBox.Show("Brak zapisanego projektu");
        }
    }
}