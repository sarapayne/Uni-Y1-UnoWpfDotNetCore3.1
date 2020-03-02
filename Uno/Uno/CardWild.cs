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

        public CardWild (Image pImage, int pCardsToDraw): base (pImage)
        {
            this.mCardsToDraw = pCardsToDraw;
        }
    }
}
