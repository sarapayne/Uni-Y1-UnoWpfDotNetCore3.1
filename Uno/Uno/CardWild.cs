using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Uno
{
    [Serializable()]
    class CardWild : Card
    {
        private int mCardsToDraw;
        private Suit mNextSuit;

        public CardWild (string pImageName, ImageSource pImage,int pCardsToDraw): base (pImageName, pImage)
        {
            this.mCardsToDraw = pCardsToDraw;
        }
    }
}
