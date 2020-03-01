using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Uno
{
    class CardWild : Card
    {
        private int mCardsToDraw;
        private Suit mNextSuit;

        CardWild (Image pImage, Suit pSuite): base (pImage)
        {
            this.mNextSuit = pSuite;
        }
    }
}
