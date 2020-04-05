using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventPublisher.RaiseMainMenu += RaiseMainMenu;
            EventPublisher.RaiseReturnToGame += ReturnToGame;
        }

        private void UnSubscribeEvents()
        {
            EventPublisher.RaiseMainMenu -= RaiseMainMenu;
            EventPublisher.RaiseReturnToGame -= ReturnToGame;
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            EventPublisher.ShutDownRoutine();
        }

        /// <summary>
        /// hides this window
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        private void ReturnToGame(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        /// <summary>
        /// hides this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void RaiseMainMenu(object sender, EventArgs eventArgs)
        {
            this.Show();
        }

        /// <summary>
        /// creates and shows the new game options window, then hides this window
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">always null</param>
        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.SetupNewGame();
            this.Hide();
        }

        /// <summary>
        /// opens a save dialog, then passes then path and file name to the save method via an event
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
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

        /// <summary>
        /// opens a load dialog, then passes the path and file name to the load method via an event
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
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

        /// <summary>
        /// When the return to game button is presses, checks to ensure there is a game to return to
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">always null</param>
        private void ButtonCurrentGame_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.CheckForActiveGame();
        }

        /// <summary>
        /// when the new tournament button is pressed, generates an event to trigger it
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">always null</param>
        private void ButtonNewTournament_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.NewTournament();
        }

        /// <summary>
        /// opens an open file dialog, then passes the path and file name to the load tournament method via an event
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
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

        /// <summary>
        /// opens a save dialog then passes the path and file name to the save tournament event via an event
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
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

        /// <summary>
        /// closes this window to avoid potential errors when closing the whole application
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">always null</param>
        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.ShutDownRoutine();
        }
    }
}
