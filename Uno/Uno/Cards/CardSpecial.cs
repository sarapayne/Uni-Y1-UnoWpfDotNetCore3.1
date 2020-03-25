using System;
using Uno.Interfaces;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    enum SpecialType
    {
        Reverse, Skip, Draw
    }

    [Serializable()]
    class CardSpecial: CardSuit, IFeatureCard
    {
        private SpecialType mSpecialType;

        public CardSpecial(string pImageName, Suit pSuit, SpecialType pSecialType): base (pImageName, pSuit)
        {
            this.mSpecialType = pSecialType;
        }

        public SpecialType Type
        {
            get { return this.mSpecialType; }
        }

        public void CardFeatures()
        {
            if (mSpecialType == SpecialType.Draw)
            {
                EventPublisher.DrawTwoCards();
            }
            else if (mSpecialType == SpecialType.Reverse)
            {
                EventPublisher.ReverseDirection();
            }
            else if (mSpecialType == SpecialType.Skip)
            {
                EventPublisher.SkipGo();
            }
        }
    }
}
