﻿using Microsoft.VisualBasic.CompilerServices;
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
            EventPublisher.RaiseUpdateGUI += WpfWindowGame_RaiseUpdateGUI;
            bool checkPlus4Button = true;
            UpdateDisplay(checkPlus4Button);
        }
        private void WpfWindowGame_RaiseUpdateGUI(object sender, EventArgs eventArgs)
        {
            bool checkPlus4Button = false;
            UpdateDisplay(checkPlus4Button);
        }

        private void RaiseUpdateGui()
        {
            bool checkPlus4Button = false;
            UpdateDisplay(checkPlus4Button);
        }

        private void UpdateDisplay(bool pCheckPlus4Button)
        {
            ClearCards();
            AddPlayerCards();
            UpdateDrawCard(pCheckPlus4Button);
            labelSecondTitle.Content = UnoMain.UnoGame.Players[UnoMain.UnoGame.CurrentPlayer].Name;
        }

        private void GameButtonClickHandler(object sender, EventArgs e)
        {   //This code is temporary, later it will be replaced by a broadcast saying card play attempted, sending the card in question. 
            ImgCardControl playerCard = sender as ImgCardControl;
            Card selectedCard = playerCard.Card;
            //string cardName = selectedCard.ImageName;
            //MessageBox.Show( "Debug: Triggered in form: " + "Player selected " + cardName, "player selected a card");
            EventPublisher.GameButtonClick(selectedCard);
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
            this.Hide();
            this.Close();
        }

        private void UpdateDrawCard(bool pCheckPlus4Button)
        {
            buttonDraw4Challenge.IsEnabled = false;
            buttonDraw4Challenge.Visibility = Visibility.Hidden;
            //List<Card> discardPile = UnoMain.UnoGame.Deck.DiscardPile;
            Card card = UnoMain.UnoGame.Deck.DiscardPile[UnoMain.UnoGame.Deck.DiscardPile.Count - 1];
            Uri imageUri = null;
            if (card is CardWild)
            {
                CardWild wildCard = card as CardWild;
                if (pCheckPlus4Button)
                {
                    if (wildCard.CardsToDraw > 0)
                    {
                        buttonDraw4Challenge.IsEnabled = true;
                        buttonDraw4Challenge.Visibility = Visibility.Visible;
                    }
                }
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
    }
}
