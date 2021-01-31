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

namespace KółkoiKrzyżyk
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LocalGameButton_Click(object sender, RoutedEventArgs e)
        {
            SingleGameWindow singleGameWindow = new SingleGameWindow();
            singleGameWindow.Show();
            Close();
        }

        private void HostGameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Oczekuję na przeciwnika");
            HostGameButton.Content = "Oczekuję na przeciwnika";
            MultiPlayerWindow multiPlayerWindow = new MultiPlayerWindow(true, null);
            multiPlayerWindow.Show();
            Close();
        }

        private void JoinGameButton_Click(object sender, RoutedEventArgs e)
        {
            MultiPlayerWindow multiPlayerWindow = new MultiPlayerWindow(false, IPBOX.Text);
            multiPlayerWindow.Show();
            Close();
        }
    }
}
