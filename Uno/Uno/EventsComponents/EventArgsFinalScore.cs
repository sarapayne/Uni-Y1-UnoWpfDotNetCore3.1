using System;
using System.Collections.Generic;
using System.Text;
using Uno.Players;

namespace Uno.EventsComponents
{
    class EventArgsFinalScore
    {
        Player mPlayer;

        public EventArgsFinalScore(Player pPlayer)
        {
            this.mPlayer = pPlayer;
        }
            
        public Player Winner
        {
            get { return this.mPlayer; }
        }
            
    }
}
