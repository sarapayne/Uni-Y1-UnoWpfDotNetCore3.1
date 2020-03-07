using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    static class EventPublisher
    {
        public static event EventHandler<EventArgsGameButtonClick> RaiseGameButtonClick; //comented code attempting to fix double event listening
        public static event EventHandler RaiseNextPlayerButtonClick;
        public static event EventHandler RaiseUpdateGUI;

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
