using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsPlayers
    {
        List<Player> mPlayers;

        public EventArgsPlayers (List<Player> pPlayers)
        {
            this.mPlayers = pPlayers;
        }

        public List<Player> Players
        {
            get { return this.mPlayers; }
        }
    }
}
