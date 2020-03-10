using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsPlayCard
    {
        Card mCard;

        public EventArgsPlayCard(Card pcard)
        {
            this.mCard = pcard;
        }

        public Card UnoCard
        {
            get { return this.mCard; }
        }
         
    }
}
