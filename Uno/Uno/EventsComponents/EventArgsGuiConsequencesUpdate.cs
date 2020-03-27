using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsGuiConsequencesUpdate
    {
        private string mPlayerName;
        private List<Card> mPlayableCards;
        private Card mLastDisardedCard;

        public EventArgsGuiConsequencesUpdate (string pPlayerName, List<Card> pPlayableCards, Card pLastDiscardedCard)
        {
            this.mPlayableCards = pPlayableCards;
            this.mPlayerName = pPlayerName;
            this.mLastDisardedCard = pLastDiscardedCard;
        }

        public string PlayerName
        {
            get { return this.mPlayerName; }
        }

        public List<Card> PlayableCards
        {
            get { return this.mPlayableCards; }
        }

        public Card LastDiscarededCard
        {
            get { return this.mLastDisardedCard; }
        }
    }
}
