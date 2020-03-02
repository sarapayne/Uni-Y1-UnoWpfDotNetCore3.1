using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    class Player
    {
        private int mNumber;
        private string mName;
        private List <int> mCards;

        public Player (int pPlayerNumber, string pPlayerName)
        {
            mNumber = pPlayerNumber;
            mName = pPlayerName;
            mCards = new List<int>();
        }

        public List <int> Cards
        {
            get { return mCards; }
            set { mCards = value; }
        }
            
    }
}
