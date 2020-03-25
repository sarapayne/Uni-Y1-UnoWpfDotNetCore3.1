using System;
using System.Collections.Generic;
using System.Text;
using Uno.Interfaces;

namespace Uno.Cards
{
    [Serializable]
    class CardReverse:CardSpecial, IFeatureCard
    {
        public CardReverse(string pImageName, Suit pSuit) : base(pImageName, pSuit)
        {

        }

        public void CardFeatures()
        {
            EventPublisher.ReverseDirection();
        }
    }
}
