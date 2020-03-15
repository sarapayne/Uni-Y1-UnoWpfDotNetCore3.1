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
        int mSelectedIndex;

        public WpfChooseSwapPlayer()
        {
            InitializeComponent();
            mPlayers = new List<Player>();
            EventPublisher.RaiseSwapHandsPlayerChoose += WpfChooseSwapPlayer_SwapHandsPlayerChoose;
            EventPublisher.RaiseMainMenu += WpfChooseSwapHands_MainMenu;
            EventPublisher.RaiseShutDownRoutine += WpfChooseSwapPlayer_ShutDownRoutine;
        }

        private void WpfChooseSwapPlayer_SwapHandsPlayerChoose(object sender, EventArgsPlayers eventArgsPlayers)
        {
            mPlayers = eventArgsPlayers.Players;
            comboboxPlayers.ItemsSource = mPlayers;
            labelPlayerName.Content = eventArgsPlayers.CurrentPlayer.Name;
            comboboxPlayers.Items.Refresh();
            this.Show();
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.SwapHandsPlayerChosen(mPlayers[mSelectedIndex]);
            this.Hide();
        }

        private void comboboxPlayers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mSelectedIndex = comboboxPlayers.SelectedIndex;
        }

        private void WpfChooseSwapHands_MainMenu(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void WpfChooseSwapPlayer_ShutDownRoutine(object sender, EventArgs eventArgs)
        {
            this.Hide();
            this.Close();
        }
    }
}
