using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Uno
{
    enum Suit
    {
        Red, Green, Blue, Yellow
    }

    class CardSuit : Card
    {
        private Suit mSuit;

        public CardSuit(string image, Suit pSuit) : base(image)
        {
            this.mSuit = pSuit;
        }
    }
}
