using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsColourPick: EventArgs
    {
        public Suit mSuit;

        public EventArgsColourPick(Suit pSuit)
        {
            this.mSuit = pSuit;
        }

        public Suit NextSuit
        {
            get { return this.mSuit; }
        }
    }
}
