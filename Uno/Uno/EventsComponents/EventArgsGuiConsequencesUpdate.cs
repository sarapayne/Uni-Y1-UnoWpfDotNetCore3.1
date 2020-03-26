using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsGuiConsequencesUpdate
    {
        private string mPlayerName;
        private List<Card> mPlayableCards;

        public EventArgsGuiConsequencesUpdate (string pPlayerName, List<Card> pPlayableCards)
        {
            this.mPlayableCards = pPlayableCards;
            this.mPlayerName = pPlayerName;
        }

        public string PlayerName
        {
            get { return this.mPlayerName; }
        }

        public List<Card> PlayableCards
        {
            get { return this.mPlayableCards; }
        }
    }
}
