using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno.Game
{
    [Serializable]
    class UnoGameHouse2: UnoGame
    {
        public UnoGameHouse2(List<string> pPlayerNames, int pdealer) : base(pPlayerNames, pdealer)
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
        /// Refers To Base Class
        /// </summary>
        protected override void CalculateFinalScores()
        {
            base.CalculateFinalScores();
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
            bool cardPlayable = CheckIfCardCanBePlayed(card);
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
        protected override void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            base.UnoGame_RaisePlayCard(sender, eventArgs);
        }

        /// <summary>
        /// Compared with the base class this removes the alloance of passing play after picking up a card.
        /// It also removes the option to challenge a +4 card. Associated methods removed entirely. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">Card object played</param>
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
            if (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] is CardWild)
            {
                CardWild cardWild = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild;
                cardWild.NextSuit = argsColourPick.NextSuit;
                FinishPlaceCard();
                if (cardWild.CardsToDraw > 0)
                {
                    for (int numToDraw = 0; numToDraw < 4; numToDraw++)
                    {
                        DrawCard(NextPlayerWithoutSkips());
                    }
                }

            }
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
