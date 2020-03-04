using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Uno
{
    [Serializable()]
    enum SpecialType
    {
        Reverse, Skip, Draw
    }

    [Serializable()]
    class CardSpecial: CardSuit
    {
        private SpecialType mSpecialType;

        public CardSpecial(string pImageName, ImageSource pImage, Suit pSuit, SpecialType pSecialType): base (pImageName, pImage, pSuit)
        {
            this.mSpecialType = pSecialType;
        }
    }
}
