using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.GUI_Custom_Elements;

namespace Uno
{
    [Serializable()]
    enum GameRulesType
    {
        Standard, House1, House2, House3
    }

    [Serializable()]
    class GameRules
    {
        public GameRules ()
        {
            //-= lines are attempts to fix the double listener events.
            //EventPublisher.RaiseGameButtonClick -= GameRules_RaiseGameButtonClick;
            EventPublisher.RaiseGameButtonClick += GameRules_RaiseGameButtonClick;
            //EventPublisher.RaiseNextPlayerButtonClick -= GameRules_RaiseNextPlayerButtonClick;
            EventPublisher.RaiseNextPlayerButtonClick += GameRules_RaiseNextPlayerButtonClick;
        }

        private void GameRules_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            MessageBox.Show("Degug:GameRules:CardName:" + card.ImageName, "GameButton click");
        }

        private void GameRules_RaiseNextPlayerButtonClick (object sender, EventArgs eventArgs)
        {
            MessageBox.Show("Debug:GameRules:NextPlayerButtonClickDetected", "next player button");
        }


    }
}
