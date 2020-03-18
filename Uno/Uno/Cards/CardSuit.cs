using System;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    enum Suit
    {
        Red, Green, Blue, Yellow, Unused
    }

    [Serializable()]
    abstract class CardSuit : Card
    {
        private Suit mSuit;

        public CardSuit(string pImageName, Suit pSuit ) : base(pImageName)
        {
            this.mSuit = pSuit;
        }

        public Suit Csuit
        {
            get { return this.mSuit; }
        }
    }
}
