using System;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    class CardWild : Card
    {
        private int mCardsToDraw;
        private Suit mNextSuit;

        public CardWild (string pImageName, Image pImage,int pCardsToDraw): base (pImageName, pImage)
        {
            this.mCardsToDraw = pCardsToDraw;
        }

        public int CardsToDraw
        {
            get { return this.mCardsToDraw; }
        }

        public Suit NextSuit 
        {
            get { return this.mNextSuit; }
        }
    }
}
