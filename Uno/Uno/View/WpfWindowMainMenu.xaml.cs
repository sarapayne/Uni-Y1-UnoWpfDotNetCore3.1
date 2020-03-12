using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
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
            EventPublisher.RaiseMainMenu += WpfWindowMainMenu_RaiseMainMenu;
            EventPublisher.RaiseReturnToGame += WpfWindowMainMenu_RaiseReturnToGame;
        }

        private void WpfWindowMainMenu_RaiseReturnToGame(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void WpfWindowMainMenu_RaiseMainMenu(object sender, EventArgs eventArgs)
        {
            this.Show();
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            //***Tempory Lists For Testing Purposes***
            List<string> players2 = new List<string> { "Player1", "Player2"};
            List<string> players3 = new List<string> { "Player1", "Player2", "Player3" };
            List<string> players5 = new List<string> {"Player1", "Player2", "Player3", "Player4", "Player5"};
            List<string> players10 = new List<string> { "Player1", "Player2", "Player3", "Player4", "Player5", "Player6", "Player7", "Player8", "Player9", "Player10" };
            //***Tempory Lists For Testing End***
            List<string> playerNames = players5; //this will eventually come from a GUI entry
            int dealer = 0; ///this will eventually be set in the GUI. 
            EventPublisher.NewGame(playerNames, dealer);
            this.Hide();
            //will load a new page with 10 spaces for player names
        }

        private void ButtonLoadGame_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "UnoGameFiles(*.unogame)|*.unogame";
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFile.ShowDialog() == true)
            {
                EventPublisher.LoadGame(openFile.FileName, "");
            }
            ////***Tempory Strings For Testing, eventually this will come from the GUI
            //string gameToLoad1 = "game1";
            //string gameToLoad2 = "game2";
            //string gameToLoad3 = "game3";
            //string gameToLoad4 = "game4";
            //string gameToLoad5 = "game5";
            ////***Tempory Strings For Testing, eventually this will come from the GUI
            //string gameToLoad = gameToLoad1;//this will come from the GUI eventually
            //EventPublisher.LoadGame(gameToLoad, "");
        }

        private void ButtonSaveGame_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "UnoGameFiles(*.unogame)|*.unogame";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFile.ShowDialog() == true)
            {
                EventPublisher.SaveGame(saveFile.FileName, "");
            }
        }

        private void ButtonCurrentGame_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.CheckForActiveGame();
        }

        private void ButtonNewTournament_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonLoadTournament_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSaveTournament_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
