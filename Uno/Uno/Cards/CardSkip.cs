using System;
using System.Collections.Generic;
using System.Text;
using Uno.Interfaces;

namespace Uno.Cards
{
    [Serializable]
    class CardSkip: CardSpecial, IFeatureCard
    {
        public CardSkip(string pImageName, Suit pSuit) : base(pImageName, pSuit)
        {

        }

        public void CardFeatures()
        {
            EventPublisher.SkipGo();
        }
    }
}
