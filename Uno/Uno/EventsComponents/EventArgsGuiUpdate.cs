using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Uno.EventsComponents
{
    public class EventArgsGuiUpdate
    {
        private Player mPlayer;
        private Deck mDeck;
        private string mExtra;

        public EventArgsGuiUpdate(Player pPlayer, Deck pDeck, string pExtras)
        {
            this.mPlayer = pPlayer;
            this.mDeck = pDeck;
            if (pExtras == null)
            {
                this.mExtra = new string("");
            }
            else this.mExtra = pExtras;
        }
            
        public Player ThisPlayer
        {
            get { return this.mPlayer; }
        }

        public Deck ThisDeck
        {
            get { return this.mDeck; }
        }

        public string ExtraInstructions
        {
            get { return this.mExtra; }
        }
    }
}
