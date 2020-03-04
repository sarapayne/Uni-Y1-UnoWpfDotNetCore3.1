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
    enum Suit
    {
        Red, Green, Blue, Yellow
    }

    [Serializable()]
    class CardSuit : Card
    {
        private Suit mSuit;

        public CardSuit(string pImageName, Image pImage, Suit pSuit) : base(pImageName, pImage)
        {
            this.mSuit = pSuit;
        }
    }
}
