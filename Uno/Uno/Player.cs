using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    class Player
    {
        [NonSerialized]
        private string mName;

        [NonSerialized]
        private List<int> mCards;

        private int mNumber;

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
