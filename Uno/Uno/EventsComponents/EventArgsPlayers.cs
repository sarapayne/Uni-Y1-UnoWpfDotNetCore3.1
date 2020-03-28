using System;
using System.Collections.Generic;
using System.Text;
using Uno.Players;

namespace Uno.EventsComponents
{
    class EventArgsPlayers
    {
        List<Player> mPlayers;
        Player mCurrentPlayer;

        public EventArgsPlayers (List<Player> pPlayers, Player pCurrentPlayer)
        {
            this.mPlayers = pPlayers;
            this.mCurrentPlayer = pCurrentPlayer;
        }

        public List<Player> Players
        {
            get { return this.mPlayers; }
        }

        public Player CurrentPlayer
        {
            get { return this.mCurrentPlayer; }
        }
    }
}
