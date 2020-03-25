using System;
using System.Collections.Generic;
using System.Text;
using Uno.Interfaces;

namespace Uno.Cards
{
    [Serializable]
    class CardDraw : CardSpecial, IFeatureCard
    {
        public CardDraw (string pImageName, Suit pSuit) : base(pImageName, pSuit)
        {

        }

        public void CardFeatures()
        {
            EventPublisher.DrawTwoCards();
        }
    }
}
