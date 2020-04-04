using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Uno
{
    [Serializable()]
    class CardWild : Card
    {
        protected int mCardsToDraw;
        protected Suit mNextSuit;

        [NonSerialized]
        protected BitmapImage mNextSuitImage;

        public CardWild (string pImageName, int pCardsToDraw): base (pImageName)
        {
            this.mCardsToDraw = pCardsToDraw;
            this.mNextSuit = Suit.Unused;
            this.mNextSuitImage = Utilities.GetPlayerCardImage("card_back");
        }

        public int CardsToDraw
        {
            get { return this.mCardsToDraw; }
        }

        public Suit NextSuit 
        {
            get { return this.mNextSuit; }
            set { this.mNextSuit = value; }
        }

        public BitmapImage NextSuitImage
        {
            get { return this.mNextSuitImage; }
        }

        /// <summary>
        /// sets the next suit image based on the value of mNextSuit
        /// </summary>
        public virtual void SetNextSuitImage()
        {
            string imageName = "";
            switch (mNextSuit)
            {
                case Suit.Red:
                    imageName = "card_front_wild_red";
                    break;
                case Suit.Green:
                    imageName = "card_front_wild_green";
                    break;
                case Suit.Blue:
                    imageName = "card_front_wild_blue";
                    break;
                case Suit.Yellow:
                    imageName = "card_front_wild_yellow";
                    break;
                default:
                    imageName = "card_back";
                    break;
            }
            mNextSuitImage = Utilities.GetPlayerCardImage(imageName);
        }

        /// <summary>
        /// Adds setting the next suit image to the base class. 
        /// </summary>
        public override void RestoreCardImage()
        {
            base.RestoreCardImage();
            SetNextSuitImage();
        }
    }
}
