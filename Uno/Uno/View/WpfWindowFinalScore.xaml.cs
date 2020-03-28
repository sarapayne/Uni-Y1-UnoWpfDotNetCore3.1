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
using Uno.EventsComponents;
using Uno.Players;

namespace Uno.View
{
    /// <summary>
    /// Interaction logic for WpfWindowFinalScore.xaml
    /// </summary>
    public partial class WpfWindowFinalScore : Window
    {
        private Player mPlayer;
        public WpfWindowFinalScore()
        {
            InitializeComponent();
            mPlayer = null;
            EventPublisher.RaiseFinalScore += WpfWindowFinalScore_RaiseFinalScore;
            EventPublisher.RaiseMainMenu += WpfWindowFinalScore_RaiseMainMenu;
            EventPublisher.RaiseCloseWindow += WpfWindowFinalScore_CloseWindow;
        }

        /// <summary>
        /// hides this window
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="eventArgs">unused</param>
        private void WpfWindowFinalScore_RaiseMainMenu(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        /// <summary>
        /// Updates the player name and score display based on the passed EventArgs
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsFinalScore">Player Object, full details</param>
        private void WpfWindowFinalScore_RaiseFinalScore(object sender, EventArgsFinalScore eventArgsFinalScore)
        {
            string playerName = eventArgsFinalScore.Winner.Name;
            int finalScore = eventArgsFinalScore.Winner.FinalScore;
            labelPlayerName.Content = playerName + "won this game";
            labelScore.Content = "Final Score: " + finalScore.ToString();
            mPlayer = eventArgsFinalScore.Winner;
            this.Show();
        }

        /// <summary>
        /// returns user to the main menu
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// Sends the current player details to the main program via an event to add to the tournament. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void buttonTournament_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.AddToTournament(mPlayer);
        }

        /// <summary>
        /// hides then closes this window ready for a clean shutdown. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="eventArgs">unused</param>
        private void WpfWindowFinalScore_CloseWindow(object sender, EventArgs eventArgs)
        {
            this.Hide();
            this.Close();
        }
    }
}
