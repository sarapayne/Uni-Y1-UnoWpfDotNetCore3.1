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

namespace Uno.View
{
    /// <summary>
    /// Interaction logic for WpfWindowSetupGame.xaml
    /// </summary>
    public partial class WpfWindowSetupGame : Window
    {
        private List<string> mPlayers;

        public WpfWindowSetupGame()
        {
            InitializeComponent();
            mPlayers = new List<string>();
            textboxEnterName.IsEnabled = true;
        }

        private void textboxEnterName_GotFocus(object sender, RoutedEventArgs e)
        {
            textboxEnterName.Foreground = Brushes.Black;
        }

        private void textboxEnterName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textboxEnterName.Text == "")
            {
                textboxEnterName.Text = "Enter Name";
                textboxEnterName.Foreground = Brushes.Gray;
            }
        }

        private void textboxEnterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textboxEnterName.Text.Length > 4)
            {
                textboxEnterName.Foreground = Brushes.Black;
                if (mPlayers.Count < 10)
                { //if its ten its already full, don't try to add more. Shouldn't be possible if this is already disabled but saftey first!
                    buttonAddPlayer.IsEnabled = true;
                }
                
            }
            else
            {
                textboxEnterName.Foreground = Brushes.Red;
                buttonAddPlayer.IsEnabled = false;
            }
        }

        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.MainMenu();
            this.Hide();
            this.Close();
        }

        private void buttonAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            mPlayers.Add(textboxEnterName.Text);
            if (mPlayers.Count == 10)
            {
                textboxEnterName.IsEnabled = false;
                buttonAddPlayer.IsEnabled = false;
                MessageBox.Show("Max 10 players, no more can be added", "max players");
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int dealer = random.Next(0, mPlayers.Count - 1);
            RulesType rulesType = new RulesType();
            if (radioOfficialRules.IsChecked == true)
            {
                rulesType = RulesType.Standard;
            }
            else if (radioHouseRules1.IsChecked == true)
            {
                rulesType = RulesType.House1;
            }
            else
            {
                rulesType = RulesType.House2;
            }
            EventPublisher.NewGame(mPlayers, dealer, rulesType);
            this.Hide();
            this.Close();
        }
    }
}
