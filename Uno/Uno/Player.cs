using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    class Player
    {
        private string mName;
        private List<Card> mCards;
        private int mNumber;
        private int mFinalScore;

        public Player (int pPlayerNumber, string pPlayerName)
        {
            mNumber = pPlayerNumber;
            mName = pPlayerName;
            mCards = new List<Card>();
            mFinalScore = 0;
        }

        public int FinalScore
        {
            get { return this.mFinalScore; }
            set { this.mFinalScore = value; }
        }

        public List <Card> Cards
        {
            get { return mCards; }
            set { mCards = value; }
        }

        public string Name
        {
            get { return this.mName; }
        }

        public void SortPlayerCards()
        {
            for (int outIndex = 0; outIndex < mCards.Count; outIndex++)
            {
                bool swapped = false;
                for (int inIndex = 0; inIndex < (mCards.Count - outIndex - 1); inIndex++)
                {
                    if (mCards[inIndex].UniqueIdentifier > mCards[inIndex+1].UniqueIdentifier)
                    {
                        Card tempCard = mCards[inIndex];
                        mCards[inIndex] = mCards[inIndex+1];
                        mCards[inIndex+1] = tempCard;
                        swapped = true;
                    }
                }
                if (!swapped) break;
            }
        }      
    }
}
