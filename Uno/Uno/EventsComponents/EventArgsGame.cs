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


        public EventArgsGame(List<string> pPlayers, int pDealer, RulesType pRulesType)
        {
            this.mPlayers = pPlayers;
            this.mDealer = pDealer;
            this.mRulesType = pRulesType;
        }

        public List<string> Players
        {
            get { return this.mPlayers; }
        }

        public int Dealer
        {
            get { return this.mDealer; }
        }
    }
}
