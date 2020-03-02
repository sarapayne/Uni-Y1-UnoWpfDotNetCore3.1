using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Uno
{
    /// <summary>
    /// Interaction logic for WpfWindowMainMenu.xaml
    /// </summary>
    public partial class WpfWindowMainMenu : Window
    {
        public WpfWindowMainMenu()
        {
            InitializeComponent();
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            //will load a new page with 10 spaces for player names
        }

        private void ButtonLoadGame_Click(object sender, RoutedEventArgs e)
        {
            //will display a new page showing a list of availible games to choose from. 
        }

        private void ButtonSaveGame_Click(object sender, RoutedEventArgs e)
        {
            //will dislay a new page asking for a file name
        }
    }
}
