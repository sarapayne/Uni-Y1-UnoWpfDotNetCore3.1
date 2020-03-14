using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Cards
{
    class CardWildSwapHands: CardWild
    {
        

        public CardWildSwapHands(string pImageName, int pCardsToDraw) : base(pImageName, pCardsToDraw)
        {
            this.mCardsToDraw = pCardsToDraw;
            this.mNextSuit = Suit.Unused;
        }

        public override void RunCardSpecialFeatures()
        {
            base.RunCardSpecialFeatures();
        }
    }
}
