using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsGame
    {
        private List<string> mPlayers;
        private int mDealer;


        public EventArgsGame(List<string> pPlayers, int pDealer)
        {
            this.mPlayers = pPlayers;
            this.mDealer = pDealer;
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
