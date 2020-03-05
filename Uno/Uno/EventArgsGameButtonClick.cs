using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    class EventArgsGameButtonClick : EventArgs
    {
        public Card mPlayingCard;

        public EventArgsGameButtonClick(Card pCard)
        {
            this.mPlayingCard = pCard;
        }


        public Card PlayingCard
        {
            get { return this.mPlayingCard; }
        }
    }
}
