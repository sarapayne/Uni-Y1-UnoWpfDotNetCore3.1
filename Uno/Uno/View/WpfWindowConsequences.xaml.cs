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
            EventPublisher.RaiseGuiConsequencesUpdate += UpdateDisplay;
        }

        private void UnsubscribeEvents()
        {
            EventPublisher.RaiseHideGuiWindows -= HideGuiWindow;
            EventPublisher.RaiseMainMenu -= HideGuiWindow;
            EventPublisher.RaiseGuiConsequencesUpdate -= UpdateDisplay;
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
            EventPublisher.StackedCardButtonClick(selectedCard);
        }

        /// <summary>
        /// Removes all player cards and then adds them fresh based on the new list.
        /// This both adds/remove cards and displays them in a sorted fashion.
        /// </summary>
        /// <param name="pUpdateData"></param>
        private void UpdateDisplay(object sender, EventArgsGuiConsequencesUpdate pUpdateData)
        {
            ClearCards();
            AddPlayerCards(pUpdateData.PlayableCards, pUpdateData.LastDiscarededCard);
            UpdateDeckImage(pUpdateData.LastDiscarededCard);
            labelSecondTitle.Content = pUpdateData.PlayerName;
            this.Show();
        }

        /// <summary>
        /// sets to appropriate image for the discard pile
        /// </summary>
        /// <param name="pDiscardPile"></param>
        private void UpdateDeckImage(Card pLastDiscardedCard)
        {
            Uri imageUri = null;
            imageUri = GetResourceUri(pLastDiscardedCard.ImageName);
            imageDiscardPile.Source = new BitmapImage(imageUri);
        }

        /// <summary>
        /// loops through the player cards passed from the main program,
        /// adding one gui element with embeded card objects for each one found. 
        /// </summary>
        /// <param name="pPlayerCards">list of cards held by the player</param>
        private void AddPlayerCards(List<Card> pPlayerCards, Card pLastDiscardedCard)
        {
            for (int playerCardIndex = 0; playerCardIndex < pPlayerCards.Count; playerCardIndex++)
            {   //places upto 18 cards in 3 rows onto the GUI
                Card card = pPlayerCards[playerCardIndex];
                ImgCardControl playerCard = new ImgCardControl(card);
                string imageName = card.ImageName;
                Uri imageUri = GetResourceUri(imageName);
                playerCard.Source = new BitmapImage(imageUri);
                //unlike the main game window the largest number of cards possible is 8. So no need to manage rows. 
                Grid.SetColumn(playerCard, playerCardIndex + 1);
                Grid.SetRow(playerCard, 3);
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
