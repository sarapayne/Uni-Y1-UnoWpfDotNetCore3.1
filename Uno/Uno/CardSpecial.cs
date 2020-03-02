using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

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

        public CardSpecial(string pImage, Suit pSuit, SpecialType pSecialType): base (pImage, pSuit)
        {
            this.mSpecialType = pSecialType;
        }
    }
}
