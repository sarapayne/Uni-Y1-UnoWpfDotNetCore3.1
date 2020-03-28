using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno.Game
{
    [Serializable]
    class UnoGameHouse2: UnoGameHouse1
    {
        public UnoGameHouse2(List<string> pPlayerNames, int pdealer, int pNumOfSwapHands) : base(pPlayerNames, pdealer, pNumOfSwapHands)
        {

        }            

        /// <summary>
        /// Compared to the base class this removes the stop event after a player has discared a card.
        /// 
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="eventArgs">Card object played</param>
        protected override void UnoGame_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            bool cardPlayable = CheckIfCardCanBePlayed(card, 0);
            if (mPlayerHasPicked)
            {
                cardPlayable = CheckIfDrawnCard(card);
            }
            if (!cardPlayable) MessageBox.Show("Sorry but this card can not be played", "Card not playable");
            else
            {
                EventPublisher.PlayCard(card);
            }
        }

        
    }
}
