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
            //***Tempory Lists For Testing Purposes***
            List<string> players2 = new List<string> { "Player1", "Player2"};
            List<string> players5 = new List<string> {"Player1", "Player2", "Player3", "Player4", "Player5"};
            List<string> players10 = new List<string> { "Player1", "Player2", "Player3", "Player4", "Player5", "Player6", "Player7", "Player8", "Player9", "Player10" };
            //***Tempory Lists For Testing End***
            List<string> playerNames = players5; //this will eventually come from a GUI entry
            GameRulesType gameRulesType = GameRulesType.Standard;
            int dealer = 0; ///this will eventually be set in the GUI. 
            UnoMain.NewGame(playerNames, dealer, gameRulesType);
            this.Hide();
            this.Close();
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

        private void ButtonCurrentGame_Click(object sender, RoutedEventArgs e)
        {
            //will return to the current game if one exists
        }
    }
}
