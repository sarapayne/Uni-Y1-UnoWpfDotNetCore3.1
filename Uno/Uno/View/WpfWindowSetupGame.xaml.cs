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

namespace Uno.View
{
    /// <summary>
    /// Interaction logic for WpfWindowSetupGame.xaml
    /// </summary>
    public partial class WpfWindowSetupGame : Window
    {
        private List<string> mPlayers;
        private List<RadioButton> mNumberOfCardsCheck;
        private bool mIncSwapHands;
        

        public WpfWindowSetupGame()
        {
            InitializeComponent();
            textboxEnterName.IsEnabled = true;
            listboxNames.ItemsSource = mPlayers;
            mNumberOfCardsCheck = new List<RadioButton> { radio1Card, radio2Card, radio3Card, radio4Card };
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            EventPublisher.ShutDownRoutine();
        }

        /// <summary>
        /// If the textbox goes out of focus and its left empty, returns 3 dots so its clearly active.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void textboxEnterName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textboxEnterName.Text == "")
            {
                textboxEnterName.Text = "...";
            }
        }

        /// <summary>
        /// Enables and disables the add player button based on the text input. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void textboxEnterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (mPlayers == null)
            {
                mPlayers = new List<string>();
            }
            if (textboxEnterName.Text.Length > 4)
            {
                textboxEnterName.Foreground = Brushes.Black;
                if (mPlayers.Count < 10)
                { //if its ten its already full, don't try to add more. Shouldn't be possible if this is already disabled but saftey first
                    buttonAddPlayer.IsEnabled = true;
                }
                
            }
            else
            {
                textboxEnterName.Foreground = Brushes.Red;
                buttonAddPlayer.IsEnabled = false;
            }
        }

        /// <summary>
        /// Returns the user to the main menu and closes this menu. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">always null</param>
        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.MainMenu();
            this.Hide();
            this.Close();
        }

        /// <summary>
        /// Adds an entered player name to a list ready for sending to the main program.
        /// Stops further entry if 10 players are entered. Also checks and stops duplicate names
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void buttonAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (!mPlayers.Contains(textboxEnterName.Text))
            {
                mPlayers.Add(textboxEnterName.Text);
                if (mPlayers.Count == 10)
                {
                    textboxEnterName.IsEnabled = false;
                    buttonAddPlayer.IsEnabled = false;
                    MessageBox.Show("Max 10 players, no more can be added", "max players");
                }
                listboxNames.Items.Refresh();
                textboxEnterName.Text = "...";
                if (mPlayers.Count >= 2)
                {
                    buttonStart.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("That name already exists, please change it");
            }
        }

        /// <summary>
        /// Uses an event to save the new game players and settings to the main program.
        /// Dealer is left here so there is an option to add dealer selection to GUI easily.
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="e">list of players, randomly chosen dealer, game rules type</param>
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int dealer = random.Next(0, mPlayers.Count - 1);
            int numOfSwapHandCards = 0;
            if (mIncSwapHands)
            {
                if (radio1Card.IsChecked == true)
                {
                    numOfSwapHandCards = 1;
                }
                else if (radio2Card.IsChecked == true)
                {
                    numOfSwapHandCards = 2;
                }
                else if (radio3Card.IsChecked == true)
                {
                    numOfSwapHandCards = 3;
                }
                else if(radio4Card.IsChecked == true)
                {
                    numOfSwapHandCards = 4;
                }
            }
            RulesType rulesType = new RulesType();
            if (radioOfficialRules.IsChecked == true)
            {
                rulesType = RulesType.Standard;
            }
            else if (radioHouseRules1.IsChecked == true)
            {
                rulesType = RulesType.House1;
            }
            else if (radioHouseRules2.IsChecked == true)
            {
                rulesType = RulesType.House2;
            }
            else if (radioHouseRules3.IsChecked == true)
            {
                rulesType = RulesType.House3;
            }
            else
            {   //how did we end up here? did someone add too many options to the GUI?
                MessageBox.Show("Something has gone wrong, please contact support", "unknown rule set selected.");
            }
            EventPublisher.NewGame(mPlayers, dealer, rulesType, numOfSwapHandCards);
            this.Hide();
            this.Close();
        }

        /// <summary>
        /// Enables the radio buttons for choosing how many cards to add
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void checkBoxSwapHands_Checked(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton radioButton in mNumberOfCardsCheck)
            {
                radioButton.IsEnabled = true;
            }
            mIncSwapHands = true;
        }

        /// <summary>
        /// Disables the radio buttons for choosing how many cards to add
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void checkBoxSwapHands_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton radioButton in mNumberOfCardsCheck)
            {
                radioButton.IsEnabled = false;
            }
            mIncSwapHands = false;
        }
    }
}
