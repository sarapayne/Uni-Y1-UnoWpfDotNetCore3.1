using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    static class EventPublisher
    {
        public static event EventHandler<EventArgsGameButtonClick> RaiseGameButtonClick;
        public static event EventHandler RaiseNextPlayerButtonClick;
        public static event EventHandler RaiseUpdateGUI;

        public static void UpdateGUI()
        {
            EventPublisher.RaiseUpdateGUI(null, null);
        }

        public static void GameButtonClick(Card pCard)
        {
            EventPublisher.RaiseGameButtonClick(null, new EventArgsGameButtonClick(pCard));
        }

        public static void NextPlayerButtonClick()
        {
            EventPublisher.RaiseNextPlayerButtonClick(null,null);
        }
    }
}
