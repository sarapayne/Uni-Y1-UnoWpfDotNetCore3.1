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
        public UnoGameHouse1 (List<string> pPlayerNames, int pdealer): base(pPlayerNames, pdealer)
        {

        }

        protected override void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            base.UnsubscribeFromEvents();
        }

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
        }

        protected override void CalculateFinalScores()
        {
            base.CalculateFinalScores();
        }

        protected override void UnoGame_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {   //Alterate rule
            base.UnoGame_RaiseGameButtonClick(sender, eventArgs);
        }

        protected override bool CheckIfDrawnCard(Card pCard)
        {
            return base.CheckIfDrawnCard(pCard);
        }

        protected override void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaisePlus4Challenge(sender, eventArgs);
        }

        protected override void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            base.UnoGame_AcceptDraw4(sender, eventArgs);
        }

        protected override void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            base.UnoGame_RaisePlayCard(sender, eventArgs);
        }

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
                for (int cardsToDraw = 0; cardsToDraw < mNextPlayerPickupTotal; cardsToDraw++)
                {
                    DrawCard(mCurrentPlayer);
                }
                mNextPlayerPickupTotal = 0;
                mNextPlayersToSkipTotal = 0;
                mPlayerHasPicked = false;
                mPlayerHasDiscarded = false;
                mPlayers[mCurrentPlayer].SortPlayerCards();
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
            else
            {
                MessageBox.Show("Sorry you need to play a card before you pass the turn to the next player", "player change error");
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
        }

        protected override void UnoGame_RaiseSkipGo(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseSkipGo(sender, eventArgs);
        }

        protected override void UnoGame_RaiseReverseDirection(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseReverseDirection(sender, eventArgs);
        }

        protected override void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseDrawTwoCards(sender, eventArgs);
        }

        protected override void UnoGame_RaiseColourPick(object sender, EventArgsColourPick argsColourPick)
        {
            base.UnoGame_RaiseColourPick(sender, argsColourPick);
        }

        protected override void DealCards()
        {
            base.DealCards();
        }

        protected override void FinishPlaceCard()
        {
            base.FinishPlaceCard();
        }

        protected override int NextPlayerWithoutSkips()
        {
            return base.NextPlayerWithoutSkips();
        }

        protected override int FixOutOfBounds(int pIndex)
        {
            return base.FixOutOfBounds(pIndex);
        }

        protected override void UnoGame_DrawCard(object sender, EventArgs eventArgs)
        {
            base.UnoGame_DrawCard(sender, eventArgs);
        }

        protected override void DrawCard(int pPlayer)
        {
            base.DrawCard(pPlayer);
        }

        protected override void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            base.MoveCardFromDiscardToPlayer(pPlayer);
        }

        protected override void MoveCardFromDrawToPlayer(int pPlayer)
        {
            base.MoveCardFromDrawToPlayer(pPlayer);
        }

        protected override void UnoMain_RaiseReturnToGame(object sender, EventArgs eventArgs)
        {
            base.UnoMain_RaiseReturnToGame(sender, eventArgs);
        }

        protected override List<Player> GenerateNewPlayers(List<string> pPlayerNames)
        {
            return base.GenerateNewPlayers(pPlayerNames);
        }

        protected override bool CheckIfCardCanBePlayed(Card pCard)
        {
            return base.CheckIfCardCanBePlayed(pCard);
        }
    }
}
