using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
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
using Uno.GUI_Custom_Elements;

namespace Uno
{
    /// <summary>
    /// Interaction logic for WpfWindowGame.xaml
    /// </summary>
    public partial class WpfWindowGame : Window
    {
        public WpfWindowGame()
        {
            InitializeComponent();
            AddPlayerCards();
            UpdateDrawCard();
            labelSecondTitle.Content = UnoMain.UnoGame.Players[UnoMain.UnoGame.CurrentPlayer].Name;
        }

        private void GameButtonClickHandler(object sender, EventArgs e)
        {   //This code is temporary, later it will be replaced by a broadcast saying card play attempted, sending the card in question. 
            ImgCardControl playerCard = sender as ImgCardControl;
            Card selectedCard = playerCard.Card;
            string cardName = selectedCard.ImageName;
            MessageBox.Show("Player selected " + cardName, "player selected a card");
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
            foreach(UIElement uI in toRemove)
            {
                MainGrid.Children.Remove(uI);
            }
        }

        private void AddPlayerCards()
        {
            int currentPlayer = UnoMain.UnoGame.CurrentPlayer;
            List<Card> playersCards = UnoMain.UnoGame.Players[currentPlayer].Cards;
            for (int playerCardIndex = 0; playerCardIndex < playersCards.Count; playerCardIndex++)
            {
                Card card = playersCards[playerCardIndex];
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

        private void ButtonRemoveCards_Click_1(object sender, RoutedEventArgs e)
        {
            ClearCards();
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
            //add code here to broadcast end of turn message to main program. 
        }

        private void UpdateDrawCard()
        {
            List<Card> discardPile = UnoMain.UnoGame.Deck.DiscardPile;
            Uri imageUri = GetResourceUri(discardPile[discardPile.Count - 1].ImageName);
            imageDiscardPile.Source = new BitmapImage(imageUri);
        }

        private Uri GetResourceUri(string resouceNane)
        {
            Uri resoureUri = new Uri("pack://application:,,,/Resources/" + resouceNane + ".png", UriKind.RelativeOrAbsolute);
            return resoureUri;
        }
    } 
}
