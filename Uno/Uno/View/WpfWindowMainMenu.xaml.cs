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
using Uno.View;

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
            WpfWindowSetupGame wpfWindowSetupGame = new WpfWindowSetupGame();
            wpfWindowSetupGame.Show();
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
            EventPublisher.NewTournament();
        }

        private void ButtonLoadTournament_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "UnoTournamentFiles(*.unotourn)|*.unotourn";
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFile.ShowDialog() == true)
            {
                EventPublisher.LoadTournament(openFile.FileName, "");
            }
        }

        private void ButtonSaveTournament_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "UnoTournamentFiles(*.unotourn)|*.unotourn";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFile.ShowDialog() == true)
            {
                EventPublisher.SaveTournament(saveFile.FileName, "");
            }
        }

        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.ShutDownRoutine();
        }
    }
}
