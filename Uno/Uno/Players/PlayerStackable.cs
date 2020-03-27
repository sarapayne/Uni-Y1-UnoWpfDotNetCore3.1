using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Players
{
    class PlayerStackable : Player
    {
        protected int mTurnsToSkip;

        public PlayerStackable(int pPlayerNumber, string pPlayerName) : base (pPlayerNumber, pPlayerName)
        {
            this.mTurnsToSkip = 0;
        }

        public int TurnsToSkip
        {
            get { return this.mTurnsToSkip; }
            set { this.mTurnsToSkip = value; }
        }
            
    }
}
