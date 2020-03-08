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

        public CardSpecial(string pImageName, int pUniqueIdentifier, Suit pSuit, SpecialType pSecialType): base (pImageName, pUniqueIdentifier ,pSuit)
        {
            this.mSpecialType = pSecialType;
        }

        public SpecialType Type
        {
            get { return this.mSpecialType; }
        }
    }
}
