using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Uno
{
    [Serializable()]
    class CardNumber: CardSuit
    {
        private int mNumber;

        public CardNumber(string pImageName, ImageSource pImage, Suit pSuit, int pNumber) : base(pImageName, pImage, pSuit)
        {
            this.mNumber = pNumber;
        }
    }

    
}
