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

        public Player (int pPlayerNumber, string pPlayerName)
        {
            mNumber = pPlayerNumber;
            mName = pPlayerName;
            mCards = new List<Card>();
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
            
    }
}
