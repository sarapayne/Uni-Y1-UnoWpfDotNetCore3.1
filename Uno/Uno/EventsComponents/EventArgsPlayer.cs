using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsPlayer
    {
        Player mPlayer;

        EventArgsPlayer(Player pPlayer)
        {
            mPlayer = pPlayer;
        }

        public Player ChosenPlayer
        {
            get { return this.mPlayer; }
        }
    }
}
