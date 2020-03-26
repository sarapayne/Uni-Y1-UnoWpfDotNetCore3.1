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
using Uno.GUI_Custom_Elements;

namespace Uno.View
{
    /// <summary>
    /// Interaction logic for WpfWindowConsequences.xaml
    /// </summary>
    public partial class WpfWindowConsequences : Window
    {
        public WpfWindowConsequences()
        {
            InitializeComponent();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventPublisher.RaiseHideGuiWindows += HideGuiWindow;
            EventPublisher.RaiseMainMenu += HideGuiWindow;
            EventPublisher.RaiseCloseWindow += CloseWindow;
        }

        private void UnsubscribeEvents()
        {
            EventPublisher.RaiseHideGuiWindows -= HideGuiWindow;
            EventPublisher.RaiseMainMenu -= HideGuiWindow;
        }

        private void HideGuiWindow(object sender, EventArgs eventArgs)
        {
            this.Hide();
        }

        private void CloseWindow(object sender, EventArgs eventArgs)
        {
            UnsubscribeEvents();
            this.Hide();
            this.Close();
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            
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
        /// Removes all player cards and then adds them fresh based on the new list.
        /// This both adds/remove cards and displays them in a sorted fashion.
        /// </summary>
        /// <param name="pUpdateData"></param>
        private void UpdateDisplay(EventArgsGuiUpdate pUpdateData)
        {
            ClearCards();
            AddPlayerCards(pUpdateData.ThisPlayer.Cards);
            UpdateDrawCard(pUpdateData.ThisDeck.DiscardPile);
            labelSecondTitle.Content = pUpdateData.ThisPlayer.Name;
            this.Show();
        }

        /// <summary>
        /// sets to appropriate image for the discard pile
        /// </summary>
        /// <param name="pDiscardPile"></param>
        private void UpdateDrawCard(List<Card> pDiscardPile)
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
        /// loops through the player cards passed from the main program,
        /// adding one gui element with embeded card objects for each one found. 
        /// </summary>
        /// <param name="pPlayerCards">list of cards held by the player</param>
        private void AddPlayerCards(List<Card> pPlayerCards)
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
        /// takes a passed name and turns it into a URI in the resources. 
        /// </summary>
        /// <param name="resouceNane"></param>
        /// <returns></returns>
        protected virtual Uri GetResourceUri(string resouceNane)
        {
            Uri resoureUri = new Uri("pack://application:,,,/Resources/" + resouceNane + ".png", UriKind.RelativeOrAbsolute);
            return resoureUri;
        }

    }
}
