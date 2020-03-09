using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Uno.EventsComponents;

namespace Uno
{
    static class EventPublisher
    {
        public static event EventHandler<EventArgsGameButtonClick> RaiseGameButtonClick; 
        public static event EventHandler<EventArgsColourPick> RaiseColourPick;
        public static event EventHandler RaiseNextPlayerButtonClick;
        public static event EventHandler RaiseUpdateGUI;

        public static void ColourPick(Suit pSuit)
        {
            if (RaiseColourPick != null)
            {
                EventPublisher.RaiseColourPick(null, new EventArgsColourPick(pSuit));
            }
        }

        public static void UpdateGUI()
        {
            if (RaiseUpdateGUI != null)
            {
                EventPublisher.RaiseUpdateGUI(null, null);
            }   
        }

        public static void GameButtonClick(Card pCard)
        {
            if (RaiseGameButtonClick != null)
            {
                EventPublisher.RaiseGameButtonClick(null, new EventArgsGameButtonClick(pCard));
            }
        }

        public static void NextPlayerButtonClick()
        {
            if (RaiseNextPlayerButtonClick != null)
            {
                EventPublisher.RaiseNextPlayerButtonClick(null, null);
            }
                
        }
    }
}
