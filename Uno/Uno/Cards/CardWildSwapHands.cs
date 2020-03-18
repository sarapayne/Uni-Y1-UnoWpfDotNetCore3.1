using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Cards
{
    [Serializable]
    class CardWildSwapHands: CardWild
    {
        

        public CardWildSwapHands(string pImageName, int pCardsToDraw) : base(pImageName, pCardsToDraw)
        {
            this.mCardsToDraw = pCardsToDraw;
            this.mNextSuit = Suit.Unused;
        }

    }
}
