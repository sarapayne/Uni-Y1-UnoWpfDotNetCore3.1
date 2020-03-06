using System;
using Image = System.Windows.Controls.Image;

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

        public CardSpecial(string pImageName, Image pImage, Suit pSuit, SpecialType pSecialType): base (pImageName, pImage, pSuit)
        {
            this.mSpecialType = pSecialType;
        }
    }
}
