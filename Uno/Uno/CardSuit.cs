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

        enum Suit
        {
            Red, Gree, Blue, Yellow
        }

        SuitType mSuitType;
        Suit mSuit;
    }
}
