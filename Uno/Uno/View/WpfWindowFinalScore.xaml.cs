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
        }

        private void WpfWindowFinalScore_RaiseMainMenu(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void WpfWindowFinalScore_RaiseFinalScore(object sender, EventArgsFinalScore eventArgsFinalScore)
        {
            string playerName = eventArgsFinalScore.Winner.Name;
            int finalScore = eventArgsFinalScore.Winner.FinalScore;
            labelPlayerName.Content = playerName + "won this game";
            labelScore.Content = "Final Score: " + finalScore.ToString();
            mPlayer = eventArgsFinalScore.Winner;
            this.Show();
        }

        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.MainMenu();
        }

        private void buttonTournament_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.AddToTournament(mPlayer);
        }
    }
}
