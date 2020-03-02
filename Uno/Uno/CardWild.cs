using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Uno
{
    [Serializable()]
    class CardWild : Card
    {
        private int mCardsToDraw;
        private Suit mNextSuit;

        public CardWild (string pImage, int pCardsToDraw): base (pImage)
        {
            this.mCardsToDraw = pCardsToDraw;
        }
    }
}
