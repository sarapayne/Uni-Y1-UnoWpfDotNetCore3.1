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
    /// Interaction logic for WpfChooseSwapPlayer.xaml
    /// </summary>
    public partial class WpfChooseSwapPlayer : Window
    {
        List<Player> mPlayers;

        public WpfChooseSwapPlayer()
        {
            InitializeComponent();
            mPlayers = new List<Player>();
            comboboxPlayers.ItemsSource = mPlayers;
            EventPublisher.RaiseSwapHandsPlayerChoose += WpfChooseSwapPlayer_SwapHandsPlayerChoose;
        }

        private void WpfChooseSwapPlayer_SwapHandsPlayerChoose(object sender, EventArgsPlayers eventArgsPlayers)
        {
            mPlayers = eventArgsPlayers.Players;
            labelPlayerName.Content = eventArgsPlayers.CurrentPlayer.Name;
            this.Show();
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
