using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Uno
{
    [Serializable()]
    class CardNumber: CardSuit
    {
        private int mNumber;

        public CardNumber(string pImage, Suit pSuit, int pNumber) : base(pImage, pSuit)
        {
            this.mNumber = pNumber;
        }
    }

    
}
