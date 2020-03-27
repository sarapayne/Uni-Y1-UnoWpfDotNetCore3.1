using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Uno.Players;

namespace Uno.EventsComponents
{
    class EventArgsAddToTournament
    {
        private Player mPplayer;

        public EventArgsAddToTournament(Player pPlayer)
        {
            this.mPplayer = pPlayer;
        }

        public Player WinningPlayer
        {
            get { return this.mPplayer; }
        }
    }
}
