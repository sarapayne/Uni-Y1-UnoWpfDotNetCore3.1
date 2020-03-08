using System;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    class CardNumber: CardSuit
    {
        private int mNumber;

        public CardNumber(string pImageName, int pUniqueIdentifier, Suit pSuit, int pNumber) : base(pImageName, pUniqueIdentifier, pSuit)
        {
            this.mNumber = pNumber;
        }

        public int Number
        {
            get { return this.mNumber; }
        }
    }
}
