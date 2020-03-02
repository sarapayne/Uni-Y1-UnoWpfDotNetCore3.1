using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Uno
{
    [Serializable()]
    enum Suit
    {
        Red, Green, Blue, Yellow
    }

    [Serializable()]
    class CardSuit : Card
    {
        private Suit mSuit;

        public CardSuit(string image, Suit pSuit) : base(image)
        {
            this.mSuit = pSuit;
        }
    }
}
