using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Reflection;
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
using Uno.GUI_Custom_Elements;

namespace Uno
{
    /// <summary>
    /// Interaction logic for WpfWindowGame.xaml
    /// </summary>
    public partial class WpfWindowGame : Window
    {
        protected List<Button> mColourPickButtons;

        public WpfWindowGame()
        {
            InitializeComponent();
            mColourPickButtons = new List<Button> { buttonRed, buttonGren, buttonBlue, buttonYellow };
            EventPublisher.RaiseGuiUpdate += WpfWindowGame_RaiseGuiUpdate;
            EventPublisher.RaiseMainMenu += WpfWindowGame_RaiseHideGuiWindow;
            EventPublisher.RaiseCloseWindow += WpfWindowGame_CloseWindow;
            EventPublisher.RaiseHideGuiWindows += WpfWindowGame_RaiseHideGuiWindow;
        }

        protected virtual void WpfWindowGame_RaiseHideGuiWindow(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        /// <summary>
        /// Updates the GUI based on the information in the EventArgs
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsGuiUpdate">
        /// Full Player Information(including player cards)
        /// Full Deck (draw and discard)
        /// Extra Instructions
        /// </param>
        protected virtual void WpfWindowGame_RaiseGuiUpdate(object sender, EventArgsGuiUpdate eventArgsGuiUpdate)
        {
            if (eventArgsGuiUpdate.ExtraInstructions == "ChooseColour")
            {   //sets up the window for wild card colour selection
                DisableGuiComponents();
                DisableChallengePlus4();
                EnableColourPick();
                buttonEndTurn.IsEnabled = true;
                this.Show();
            }
            else if (eventArgsGuiUpdate.ExtraInstructions == "Challenge+4")
            {   //sets up the window so the player can accept or challenge +4 use
                DisableColourPick();
                DisableGuiComponents();
                EnableChallengePlus4();
                labelSecondTitle.Content = eventArgsGuiUpdate.ThisPlayer.Name;
                this.Show();
            }
            else if (eventArgsGuiUpdate.ExtraInstructions == "GameOver")
            {   //ensures nothing can be click to avoid erroneous clicks and hides the window.
                DisableGuiComponents();
                this.Hide();
            }
            else
            {   //sets up the GUI for normal game play based on the updated information.
                DisableColourPick();
                DisableChallengePlus4();
                EnableCoreGameButtons();
                UpdateDisplay(eventArgsGuiUpdate);
            }
        }

        /// <summary>
        /// Enables the main game play buttons other than playing cards.
        /// </summary>
        protected virtual void EnableCoreGameButtons()
        {
            buttonEndTurn.IsEnabled = true;
            buttonMainMenu.IsEnabled = true;
            imageDrawPile.IsEnabled = true;
            imageDiscardPile.IsEnabled = true;
        }

        /// <summary>
        /// Enables the colour pick buttons and shows them.
        /// </summary>
        protected virtual void EnableColourPick()
        {
            foreach (Button button in mColourPickButtons)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// disables and hides the challenge plus 4 buttons
        /// </summary>
        protected virtual void DisableChallengePlus4()
        {
            buttonDraw4Challenge.Visibility = Visibility.Hidden;
            buttonDraw4Challenge.IsEnabled = false;
            buttonAccept.Visibility = Visibility.Hidden;
            buttonAccept.IsEnabled = false;
        }

        /// <summary>
        /// Enables and Shows the challenge +4 controls
        /// </summary>
        protected virtual void EnableChallengePlus4()
        {
            buttonDraw4Challenge.Visibility = Visibility.Visible;
            buttonDraw4Challenge.IsEnabled = true;
            buttonAccept.Visibility = Visibility.Visible;
            buttonAccept.IsEnabled = true;
        }

        /// <summary>
        /// Disables the colour pick buttons and hides them
        /// </summary>
        protected virtual void DisableColourPick()
        {
            foreach (Button button in mColourPickButtons)
            {
                button.IsEnabled = false;
                button.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Disables all GUI components
        /// </summary>
        protected virtual void DisableGuiComponents()
        {
            foreach (UIElement uIElement in MainGrid.Children)
            {
                uIElement.IsEnabled = false;
            }
        }

        /// <summary>
        /// Removes all player cards and then adds them fresh based on the new list.
        /// This both adds/remove cards and displays them in a sorted fashion.
        /// </summary>
        /// <param name="pUpdateData"></param>
        protected virtual void UpdateDisplay(EventArgsGuiUpdate pUpdateData)
        {
            ClearCards();
            AddPlayerCards(pUpdateData.ThisPlayer.Cards);
            UpdateDrawCard(pUpdateData.ThisDeck.DiscardPile);
            labelSecondTitle.Content = pUpdateData.ThisPlayer.Name;
            this.Show();
        }

        /// <summary>
        /// Each GUI card has the related card as a property. When clicked this
        /// card reference is passed to the main program to be played. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">Card Object</param>
        protected virtual void GameButtonClickHandler(object sender, EventArgs e)
        {
            ImgCardControl playerCard = sender as ImgCardControl;
            Card selectedCard = playerCard.Card;
            EventPublisher.GameButtonClick(selectedCard);
        }

        /// <summary>
        /// sets the suit colour based on the button clicked, then passes to an event which sends to the main program. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonRed_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Red;
            TriggerEvent(suit);
        }

        /// <summary>
        /// sets the suit colour based on the button clicked, then passes to an event which sends to the main program. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonGren_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Green;
            TriggerEvent(suit);
        }

        /// <summary>
        /// sets the suit colour based on the button clicked, then passes to an event which sends to the main program. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonBlue_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Blue;
            TriggerEvent(suit);
        }

        /// <summary>
        /// sets the suit colour based on the button clicked, then passes to an event which sends to the main program. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonYellow_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Yellow;
            TriggerEvent(suit);
        }

        /// <summary>
        /// Sends the selected suit of a wild card to the main program via an event
        /// </summary>
        /// <param name="pSuit">enum suit</param>
        protected virtual void TriggerEvent(Suit pSuit)
        {
            EventPublisher.ColourPick(pSuit);
        }

        /// <summary>
        /// Removes all existing player cards from the GUI
        /// </summary>
        protected virtual void ClearCards()
        {   //surely there must be a better way than this! however it works for now. 
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement uIElement in MainGrid.Children)
            {
                if (uIElement is ImgCardControl)
                {
                    toRemove.Add(uIElement);
                }
            }
            foreach (UIElement uI in toRemove)
            {
                MainGrid.Children.Remove(uI);
            }
        }

        /// <summary>
        /// loops through the player cards passed from the main program,
        /// adding one gui element with embeded card objects for each one found. 
        /// </summary>
        /// <param name="pPlayerCards">list of cards held by the player</param>
        protected virtual void AddPlayerCards(List<Card> pPlayerCards)
        {
            int lastGuiElement = pPlayerCards.Count;
            if (lastGuiElement > 54)
            {   //According to a source I read it is basically impossible under normal game play for one player to have more than 35 cards unless
                //they are deliberately collecting cards. So instead of worrying about handling the eventuality I just stopped a crash if they hold 
                //more than half of the whole deck. 
                lastGuiElement = 54;
                MessageBox.Show("You have more than 54 cards, you can continue to play but only the first 54 cards will be show", "too many cards");
            }
            for (int playerCardIndex = 0; playerCardIndex < lastGuiElement; playerCardIndex++)
            {   //places upto 18 cards in 3 rows onto the GUI
                Card card = pPlayerCards[playerCardIndex];
                ImgCardControl playerCard = new ImgCardControl(card);
                string imageName = card.ImageName;
                Uri imageUri = GetResourceUri(imageName);
                playerCard.Source = new BitmapImage(imageUri);
                if (playerCardIndex < 18)
                {
                    Grid.SetColumn(playerCard, playerCardIndex + 1);
                    Grid.SetRow(playerCard, 3);
                }
                else if (playerCardIndex < 36)
                {
                    Grid.SetColumn(playerCard, playerCardIndex - 17); //18 cards in a row, and col number starts at 1
                    Grid.SetRow(playerCard, 4);
                }
                else
                {
                    Grid.SetColumn(playerCard, playerCardIndex - 35); //index - (2*18-1)
                    Grid.SetRow(playerCard, 5);
                }
                playerCard.MouseUp += new MouseButtonEventHandler(GameButtonClickHandler);
                MainGrid.Children.Add(playerCard);
            }
        }

        /// <summary>
        /// Returns user to the main menu
        /// </summary>
        /// <param name="sender">unusd</param>
        /// <param name="e">unused</param>
        protected virtual void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// Sends End turn request to the main program
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonEndTurn_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.NextPlayerButtonClick();
        }

        /// <summary>
        /// sets to appropriate image for the discard pile
        /// </summary>
        /// <param name="pDiscardPile"></param>
        protected virtual void UpdateDrawCard(List<Card> pDiscardPile)
        {
            Uri imageUri = null;
            if (pDiscardPile.Count == 0)
            {
                string emptyCardName = "card_empty";
                imageUri = GetResourceUri(emptyCardName);
            }
            else
            {
                Card card = pDiscardPile[pDiscardPile.Count - 1];
                if (card is CardWild)
                {
                    CardWild wildCard = card as CardWild;
                    string imageName = "";
                    switch (wildCard.NextSuit)
                    {
                        case Suit.Red:
                            imageName = "card_front_wild_red";
                            break;
                        case Suit.Green:
                            imageName = "card_front_wild_green";
                            break;
                        case Suit.Blue:
                            imageName = "card_front_wild_blue";
                            break;
                        case Suit.Yellow:
                            imageName = "card_front_wild_yellow";
                            break;
                    }
                    imageUri = GetResourceUri(imageName);
                }
                else
                {
                    imageUri = GetResourceUri(card.ImageName);
                }
            }
            imageDiscardPile.Source = new BitmapImage(imageUri);
        }

        /// <summary>
        /// takes a passed name and turns it into a URI in the resources. 
        /// </summary>
        /// <param name="resouceNane"></param>
        /// <returns></returns>
        protected virtual Uri GetResourceUri(string resouceNane)
        {
            Uri resoureUri = new Uri("pack://application:,,,/Resources/" + resouceNane + ".png", UriKind.RelativeOrAbsolute);
            return resoureUri;
        }

        /// <summary>
        /// sends a draw card instruction to the main program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void imageDrawPile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EventPublisher.DrawCard();
        }

        /// <summary>
        /// sends a challenge +4 event to the main program then hides and disables the button
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        protected virtual void buttonDraw4Challenge_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.Plus4Challenge();
            buttonDraw4Challenge.Visibility = Visibility.Hidden;
            buttonDraw4Challenge.IsEnabled = false;
        }

        /// <summary>
        /// hides and closes this window ready for a clean shutdown. 
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="eventArgs">unused</param>
        protected virtual void WpfWindowGame_CloseWindow(object sender, EventArgs eventArgs)
        {
            this.Hide();
            this.Close();
        }

        /// <summary>
        /// Accepts a +4, this is over ridden in some house games.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.AcceptDraw4();
        }
    }
}
