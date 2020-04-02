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
        protected override void DiscardCardButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            bool cardPlayable = CheckIfCardCanBePlayed(card, 0);//0 is the offset from the last discared card, 
            bool cardInDrawnList = CheckIfDrawnCard(card);
            bool allowPlay;
            if (mPlayerHasPicked && cardInDrawnList && cardPlayable)
            {
                allowPlay = true;

            }
            else if (!mPlayerHasPicked && cardPlayable)
            {
                allowPlay = true;
            }
            else
            {
                allowPlay = false;
            }
            if (!allowPlay) MessageBox.Show("Sorry but this card can not be played", "Card not playable");
            else
            {
                EventPublisher.PlayCard(card);
            }
        }  
    }
}
