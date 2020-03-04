using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    class CardNumber: CardSuit
    {
        private int mNumber;

        public CardNumber(string pImageName, Image pImage, Suit pSuit, int pNumber) : base(pImageName, pImage, pSuit)
        {
            this.mNumber = pNumber;
        }
    }

    
}
