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
        }

        private void GameButtonClickHandler(object sender, EventArgs e)
        {
            ImgCardControl playerCard = sender as ImgCardControl;
            Card selectedCard = playerCard.Card;
            string cardName = selectedCard.ImageName;
            MessageBox.Show("Player selected " + cardName, "player selected a card");
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
                Uri resoureUri = new Uri("pack://application:,,,/Resources/" + imageName + ".png", UriKind.RelativeOrAbsolute);
                playerCard.Source = new BitmapImage(resoureUri);
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
    } 
}
