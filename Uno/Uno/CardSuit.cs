using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

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

        public CardSuit(string pImageName, ImageSource pImage, Suit pSuit) : base(pImageName, pImage)
        {
            this.mSuit = pSuit;
        }
    }
}
