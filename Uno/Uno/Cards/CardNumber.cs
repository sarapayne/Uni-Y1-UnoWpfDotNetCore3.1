using System;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    class CardNumber: CardSuit
    {
        private int mNumber;

        public CardNumber(string pImageName, Suit pSuit, int pNumber) : base(pImageName, pSuit)
        {
            this.mNumber = pNumber;
        }

        public int Number
        {
            get { return this.mNumber; }
        }

        public override void RunCardSpecialFeatures()
        {
            base.RunCardSpecialFeatures();
        }
    }
}
