﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno.Game
{
    [Serializable]
    class UnoGameHouse1: UnoGame
    {
        public UnoGameHouse1 (List<string> pPlayerNames, int pdealer, int pNumOfSwapHands): base(pPlayerNames, pdealer, pNumOfSwapHands)
        {

        }

        protected override void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            base.UnoGame_RaisePlayCard(sender, eventArgs);
        }

        /// <summary>
        /// Compared with the base class this removes the next player allowance after a card has been draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void UnoGame_RaiseNextPlayerButtonClick(object sender, EventArgs eventArgs)
        {
            if (mPlayerHasDiscarded)
            {
                int nextPlayerWithoutSips = NextPlayerWithoutSkips();
                if (mforwards)
                {
                    nextPlayerWithoutSips += mNextPlayersToSkipTotal;
                }
                else
                {
                    nextPlayerWithoutSips -= mNextPlayersToSkipTotal;
                }
                mCurrentPlayer = FixOutOfBounds(nextPlayerWithoutSips);
                mNextPlayersToSkipTotal = 0;
                mPlayerHasPicked = false;
                mPlayerHasDiscarded = false;
                mCardsDrawnThisTurn.Clear();
                mPlayers[mCurrentPlayer].SortPlayerCards();
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
            else
            {
                MessageBox.Show("Sorry you need to either pickup or play a card before you pass the turn to the next player", "player change error");
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
        }

        
    }
}
