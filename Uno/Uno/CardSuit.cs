using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    class CardSuit : Card
    {
        enum SuitType
        {
            Number, Draw, Skip, Reverse
        }

        private SuitType mSuitType;
        private Suit mSuit;
    }
}
