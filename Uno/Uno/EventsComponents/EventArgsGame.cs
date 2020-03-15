using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsGame
    {
        private List<string> mPlayers;
        private int mDealer;
        private RulesType mRulesType;
        private int mNumOfSwapHands;

        public EventArgsGame(List<string> pPlayers, int pDealer, RulesType pRulesType, int pNumOfSwapHands)
        {
            this.mPlayers = pPlayers;
            this.mDealer = pDealer;
            this.mRulesType = pRulesType;
            this.mNumOfSwapHands = pNumOfSwapHands;
        }

        public List<string> Players
        {
            get { return this.mPlayers; }
        }

        public int Dealer
        {
            get { return this.mDealer; }
        }

        public RulesType GameRulesType
        {
            get { return this.mRulesType; }
        }

        public int NumOfSwapHands
        {
            get { return this.mNumOfSwapHands; }
        }
    }
}
