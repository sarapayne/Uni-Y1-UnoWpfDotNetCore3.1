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

        private void AddPlayerCards()
        {
            int currentPlayer = UnoMain.UnoGame.CurrentPlayer;
            List<Card> playersCards = UnoMain.UnoGame.Players[currentPlayer].Cards;
            foreach(Card card in playersCards)
            {
                ImgCardControl playerCard = new ImgCardControl(card);

                
            }
            
        }
    }
}
