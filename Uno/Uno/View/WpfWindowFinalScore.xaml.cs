using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventPublisher.RaiseFinalScore += RaiseFinalScore;
            EventPublisher.RaiseHideGuiWindows += HideWindow;
        }

        private void UnsubscribeEvents()
        {
            EventPublisher.RaiseFinalScore -= RaiseFinalScore;
            EventPublisher.RaiseHideGuiWindows += HideWindow;
        }

        private void HideWindow(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// hides this window
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="eventArgs">unused</param>


        /// <summary>
        /// Updates the player name and score display based on the passed EventArgs
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsFinalScore">Player Object, full details</param>
        private void RaiseFinalScore(object sender, EventArgsFinalScore eventArgsFinalScore)
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
            EventPublisher.GameOver();
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
        private void CloseWindow(object sender, EventArgs eventArgs)
        {
            UnsubscribeEvents();
            this.Hide();
            this.Close();
        }
    }
}
