using System;
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

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            base.UnsubscribeFromEvents();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
        }

        /// <summary>
        /// refers to base class
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsPlayer">chosen Player object</param>
        protected override void UnoGame_SwapHandsPlayerChosen(object sender, EventArgsPlayer eventArgsPlayer)
        {
            base.UnoGame_SwapHandsPlayerChosen(sender, eventArgsPlayer);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void CalculateFinalScores()
        {
            base.CalculateFinalScores();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {   
            base.UnoGame_RaiseGameButtonClick(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override bool CheckIfDrawnCard(Card pCard)
        {
            return base.CheckIfDrawnCard(pCard);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaisePlus4Challenge(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            base.UnoGame_AcceptDraw4(sender, eventArgs);
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

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseSkipGo(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseSkipGo(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseReverseDirection(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseReverseDirection(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseDrawTwoCards(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_RaiseColourPick(object sender, EventArgsColourPick argsColourPick)
        {
            base.UnoGame_RaiseColourPick(sender, argsColourPick);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void DealCards()
        {
            base.DealCards();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void FinishPlaceCard()
        {
            base.FinishPlaceCard();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override int NextPlayerWithoutSkips()
        {
            return base.NextPlayerWithoutSkips();
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override int FixOutOfBounds(int pIndex)
        {
            return base.FixOutOfBounds(pIndex);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoGame_DrawCard(object sender, EventArgs eventArgs)
        {
            base.UnoGame_DrawCard(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void DrawCard(int pPlayer)
        {
            base.DrawCard(pPlayer);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            base.MoveCardFromDiscardToPlayer(pPlayer);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void MoveCardFromDrawToPlayer(int pPlayer)
        {
            base.MoveCardFromDrawToPlayer(pPlayer);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override void UnoMain_RaiseReturnToGame(object sender, EventArgs eventArgs)
        {
            base.UnoMain_RaiseReturnToGame(sender, eventArgs);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override List<Player> GenerateNewPlayers(List<string> pPlayerNames)
        {
            return base.GenerateNewPlayers(pPlayerNames);
        }

        /// <summary>
        /// Refers To Base Class
        /// </summary>
        protected override bool CheckIfCardCanBePlayed(Card pCard)
        {
            return base.CheckIfCardCanBePlayed(pCard);
        }
    }
}
