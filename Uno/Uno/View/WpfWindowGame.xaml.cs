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
        private List<Button> mColourPickButtons;

        public WpfWindowGame()
        {
            InitializeComponent();
            mColourPickButtons = new List<Button> { buttonRed, buttonGren, buttonBlue, buttonYellow };
            EventPublisher.RaiseGuiUpdate += WpfWindowGame_RaiseGuiUpdate;
        }
 
        private void WpfWindowGame_RaiseGuiUpdate(object sender, EventArgsGuiUpdate eventArgsGuiUpdate)
        {
            if (eventArgsGuiUpdate.ExtraInstructions == "ChooseColour")
            {
                DisableGuiComponents();
                DisableChallengePlus4();
                EnableColourPick();
                this.Show();
            }
            else if (eventArgsGuiUpdate.ExtraInstructions == "Challenge+4")
            {
                DisableColourPick();
                DisableGuiComponents();
                EnableChallengePlus4();
                this.Show();
            }
            else
            {
                DisableColourPick();
                DisableChallengePlus4();
                EnableCoreGameButtons();
                UpdateDisplay(eventArgsGuiUpdate);
            }
        }

        private void EnableCoreGameButtons()
        {
            buttonEndTurn.IsEnabled = true;
            buttonMainMenu.IsEnabled = true;
            imageDrawPile.IsEnabled = true;
            imageDiscardPile.IsEnabled = true;
        }

        private void EnableColourPick()
        {
            foreach(Button button in mColourPickButtons)
            {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
            }
        }

        private void DisableChallengePlus4()
        {
            buttonDraw4Challenge.Visibility = Visibility.Hidden;
            buttonDraw4Challenge.IsEnabled = false;
            buttonAcceptDraw4.Visibility = Visibility.Hidden;
            buttonAcceptDraw4.IsEnabled = false;
        }

        private void EnableChallengePlus4()
        {
            buttonDraw4Challenge.Visibility = Visibility.Visible;
            buttonDraw4Challenge.IsEnabled = true;
            buttonAcceptDraw4.Visibility = Visibility.Visible;
            buttonAcceptDraw4.IsEnabled = true;
        }

        private void DisableColourPick()
        {
            foreach (Button button in mColourPickButtons)
            {
                button.IsEnabled = false;
                button.Visibility = Visibility.Hidden;
            }
        }

        private void DisableGuiComponents()
        {
            foreach (UIElement uIElement in MainGrid.Children)
            {
                uIElement.IsEnabled = false;
            }
        }

        private void UpdateDisplay(EventArgsGuiUpdate pUpdateData)
        {
            ClearCards();
            AddPlayerCards(pUpdateData.ThisPlayer.Cards);
            UpdateDrawCard(pUpdateData.ThisDeck.DiscardPile);
            labelSecondTitle.Content = UnoMain.UnoGame.Players[UnoMain.UnoGame.CurrentPlayer].Name;
            this.Show();
        }

        private void GameButtonClickHandler(object sender, EventArgs e)
        {   
            ImgCardControl playerCard = sender as ImgCardControl;
            Card selectedCard = playerCard.Card;
            EventPublisher.GameButtonClick(selectedCard);
        }

        private void buttonRed_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Red;
            TriggerEvent(suit);
        }

        private void buttonGren_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Green;
            TriggerEvent(suit);
        }

        private void buttonBlue_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Blue;
            TriggerEvent(suit);
        }

        private void buttonYellow_Click(object sender, RoutedEventArgs e)
        {
            Suit suit = Suit.Yellow;
            TriggerEvent(suit);
        }

        private void TriggerEvent(Suit pSuit)
        {
            EventPublisher.ColourPick(pSuit);
        }

        private void ClearCards()
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

        private void AddPlayerCards(List<Card> pPlayerCards)
        {
            for (int playerCardIndex = 0; playerCardIndex < pPlayerCards.Count; playerCardIndex++)
            {
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

        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            WpfWindowMainMenu wpfWindowMainMenu = new WpfWindowMainMenu();
            wpfWindowMainMenu.Show();
            this.Hide();
            this.Close();
        }

        private void buttonEndTurn_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.NextPlayerButtonClick();
        }

        private void UpdateDrawCard(List<Card> pDiscardPile)
        {
            Card card = pDiscardPile[pDiscardPile.Count - 1];
            Uri imageUri = null;
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
            imageDiscardPile.Source = new BitmapImage(imageUri);
        }

        private Uri GetResourceUri(string resouceNane)
        {
            Uri resoureUri = new Uri("pack://application:,,,/Resources/" + resouceNane + ".png", UriKind.RelativeOrAbsolute);
            return resoureUri;
        }

        private void imageDrawPile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UnoMain.UnoGame.DrawCard();
        }

        private void buttonDraw4Challenge_Click(object sender, RoutedEventArgs e)
        {
            EventPublisher.Plus4Challenge();
            buttonDraw4Challenge.Visibility = Visibility.Hidden;
            buttonDraw4Challenge.IsEnabled = false;
        }

        private void buttonAcceptDraw4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
