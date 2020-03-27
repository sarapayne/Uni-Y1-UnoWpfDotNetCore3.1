using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.Cards;
using Uno.EventsComponents;

namespace Uno.Game
{
    class UnoGameHouse3: UnoGame
    {
        protected int mNextPlayerInLine;
        
        protected bool mStackedCardsAccepted = false;
        protected int mNumNextPlayerDrawCards;

        public UnoGameHouse3(List<string> pPlayerNames, int pdealer, int pNumOfSwapHands) : base(pPlayerNames, pdealer, pNumOfSwapHands)
        {
            
        }

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
        }

        protected override void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseUnsubscribeEvents(sender, eventArgs);
        }

        protected override void FinishPlaceCard()
        {
            base.FinishPlaceCard();
        }

        /// <summary>
        /// Updates the next player in line, then sends the option to stack cards
        /// to that player. 
        /// </summary>
        protected virtual void SendStackCardsOption()
        {
            mNextPlayerInLine = NextPlayerWithoutSips(mNextPlayerInLine);
            List<Card> playableCards = GetStackableCards(mNextPlayerInLine);
            EventPublisher.GuiConsequencesUpdate(mPlayers[mNextPlayerInLine].Name, playableCards, mDeck.DiscardPile[mDeck.DiscardPile.Count-1]);
        }

        /// <summary>
        /// Overides the base class and adds a check to see if a stackable card is on the discard pile and if other players need to take action.
        /// Launces the WpfConsequencesWindow if they do. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void UnoGame_RaiseNextPlayerButtonClick(object sender, EventArgs eventArgs)
        {
            if (mPlayerHasDiscarded || mPlayerHasPicked)
            {
                Card lastDiscardCard = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1];
                if (lastDiscardCard is CardDraw || lastDiscardCard is CardSkip || (lastDiscardCard is CardWild && (lastDiscardCard as CardWild).CardsToDraw > 0))
                {
                    if (!mStackedCardsAccepted)
                    {
                        SendStackCardsOption();
                    }
                }
                int startPlayer = mCurrentPlayer;
                int nextPlayer = 0;//just initilise at this point. 
                for (int skips = 0; skips < mNextPlayersToSkipTotal + 1; skips++) //add one because the player always needs to change by at least one person.
                {
                    nextPlayer = NextPlayerWithoutSips(startPlayer);
                }
                mCurrentPlayer = nextPlayer;
                ResetTurnVariblesForNextPlayer();
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
        /// generates and returns a list of stackable cards from the porivided player number
        /// </summary>
        /// <param name="pNextPlayerInLine"></param>
        /// <returns></returns>
        protected virtual List<Card> GetStackableCards (int pNextPlayerInLine)
        {
            List<Card> stackableCards = new List<Card>();
            Card lastDiscardedCard = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1];
            foreach(Card card in mPlayers[pNextPlayerInLine].Cards)
            {
                switch (lastDiscardedCard)
                {
                    case CardWild discardCardWild:
                        if (card is CardWild && (card as CardWild).CardsToDraw > 0)
                        {
                            stackableCards.Add(card);
                        }
                        break;
                    case CardSkip discardedCardSkip:
                        if(card is CardSkip)
                        {
                            stackableCards.Add(card);
                        }
                        break;
                    case CardDraw discardedCardDraw:
                        if (card is CardDraw)
                        {
                            stackableCards.Add(card);
                        }
                        break;
                }
            }
            return stackableCards;
        }
            

        /// <summary>
        /// Overrides the base class so that the total to pick up is increased instead of passing strait to the next player now.
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected override void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            mNumNextPlayerDrawCards += 2;
            EventPublisher.SkipGo(); 
        }

        /// <summary>
        /// Calls the base class then resets a few member variables specific to this class. 
        /// </summary>
        protected override void ResetTurnVariblesForNextPlayer()
        {
            base.ResetTurnVariblesForNextPlayer();
            mStackedCardsAccepted = false;
            mNumNextPlayerDrawCards = 0;
            mNextPlayerInLine = mCurrentPlayer;//set to current player so it moves to the next natural in line when update is called. 
        }
            

        protected override void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            base.UnoGame_RaisePlayCard(sender, eventArgs);
        }

        /// <summary>
        /// Overrides the base class, instead of adding 4 cards to the next player, it now 
        /// increases the next player draw cards integer by 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            mNumNextPlayerDrawCards += 4;
            mPlayerHasDiscarded = true;//set this so the next player method doesn't refuse to work. and stops playing of more cards. 
            mPlayerHasPicked = true;
            EventPublisher.SkipGo();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// OverRides the base class by incrementing the number of cards the next player draws instead of giving them 
        /// strait away now. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            string message = mPlayers[NextPlayerWithoutSkips()].Name + "has challenged " + mPlayers[mCurrentPlayer].Name + "'s use of a +4 card";
            MessageBox.Show(message, "+4 challenge");
            bool hadPlayableCard = false;
            foreach (Card card in mPlayers[mCurrentPlayer].Cards)
            {
                bool playableCardFound = CheckIfCardCanBePlayed(card);
                if (playableCardFound)
                {
                    if (!(card is CardWild))
                    {
                        hadPlayableCard = true;
                        break;
                    }
                    else
                    {   //wild card found, test the type
                        CardWild cardWild = card as CardWild;
                        if (cardWild.CardsToDraw == 0)
                        {
                            hadPlayableCard = true;
                            break;
                        }
                    }
                }
            }
            if (hadPlayableCard)
            {
                message = mPlayers[NextPlayerWithoutSkips()].Name + " won the challenge, " + mPlayers[mCurrentPlayer].Name + " draws 4 cards";
                for (int num = 0; num < 4; num++)
                {
                    DrawCard(mCurrentPlayer);
                }
            }
            else
            {
                message = message = mPlayers[NextPlayerWithoutSkips()].Name + " lost the challenge and draws 6 cards";
                //over ride happens here, instead of the next player in line getting 6 cards now, number of cards to draw for the next player
                //is incremented by 6 cards. 
                mNumNextPlayerDrawCards += 6;
                EventPublisher.SkipGo();
            }
            MessageBox.Show(message, "challenge result");
            mPlayerHasPicked = true; //set to allow the change of player.
            mPlayerHasDiscarded = true; //set to block the playing of more cards.
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }
    }
}
