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
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventPublisher.RaiseSwapHandsPlayerChoose += WpfChooseSwapPlayer_SwapHandsPlayerChoose;
            EventPublisher.RaiseMainMenu += WpfChooseSwapHands_MainMenu;
            EventPublisher.RaiseCloseWindow += CloseWindow;
        }

        private void UnsubscribeEvents()
        {
            EventPublisher.RaiseSwapHandsPlayerChoose -= WpfChooseSwapPlayer_SwapHandsPlayerChoose;
            EventPublisher.RaiseMainMenu -= WpfChooseSwapHands_MainMenu;
            EventPublisher.RaiseCloseWindow -= CloseWindow;
        }

        private void CloseWindow(object sender, EventArgs eventArgs)
        {
            UnsubscribeEvents();
            this.Hide();
            this.Close();
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            EventPublisher.ShutDownRoutine();
        }

        private void WpfChooseSwapPlayer_SwapHandsPlayerChoose(object sender, EventArgsPlayers eventArgsPlayers)
        {
            mPlayers = eventArgsPlayers.Players;
            comboboxPlayers.ItemsSource = mPlayers;
            labelPlayerName.Content = eventArgsPlayers.CurrentPlayer.Name;
            comboboxPlayers.Items.Refresh();
            buttonSubmit.IsEnabled = false;
            this.Show();
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.SwapHandsPlayerChosen(mPlayers[mSelectedIndex]);
            this.Hide();
        }

        private void WpfChooseSwapHands_MainMenu(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void comboboxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mSelectedIndex = comboboxPlayers.SelectedIndex;
            buttonSubmit.IsEnabled = true;
        }
    }
}
