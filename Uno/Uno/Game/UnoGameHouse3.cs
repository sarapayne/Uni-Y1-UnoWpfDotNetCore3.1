using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.Cards;
using Uno.EventsComponents;
using Uno.Players;

namespace Uno.Game
{
    [Serializable]
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
            EventPublisher.RaiseStackedCardButtonClick += StackedCardButtonClick;
        }

        protected override void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseUnsubscribeEvents(sender, eventArgs);
            EventPublisher.RaiseStackedCardButtonClick -= StackedCardButtonClick;
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
                    if (!mStackedCardsAccepted && mNextPlayersToSkipTotal != 0)
                    {   //only come here if a possibility to stack cards exists. All stackable cards add skips. 
                        //So if skips is set to 0, this means either its a new game, or the end of a stack was reached. 
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
                ApplyStackedConsequences();
                ResetTurnVariblesForNextPlayer();
                mPlayers[mCurrentPlayer].SortPlayerCards();
                if (    (mPlayers[mCurrentPlayer] as PlayerStackable).TurnsToSkip == 0 )
                {
                    EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
                }
                else
                {
                    MessageBox.Show(mPlayers[mCurrentPlayer].ToString() + "skips their turn");
                    mPlayerHasDiscarded = true;//set to allow change of player in all circumstances
                    mPlayerHasPicked = true; //set to allow change of player all circumstances
                    (mPlayers[mCurrentPlayer] as PlayerStackable).TurnsToSkip--;
                    EventPublisher.NextPlayerButtonClick();
                }
                
            }
            else
            {
                MessageBox.Show("Sorry you need to either pickup or play a card before you pass the turn to the next player", "player change error");
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
        }

        /// <summary>
        /// applies the stacked consequences to the player including drawing cards and skips go
        /// </summary>
        protected virtual void ApplyStackedConsequences()
        {
            (mPlayers[mCurrentPlayer] as PlayerStackable).TurnsToSkip += mNextPlayersToSkipTotal; //give al the skips to this player if any exist.
            for(int count = 0; count < mNumNextPlayerDrawCards; count++)
            {   //draw a cared how ever many times is needed. 
                DrawCard(mCurrentPlayer);
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
            
        /// <summary>
        /// process a stacked played card, adding skips and draws to the total for the next person
        /// then deals with actually moving the cards from the players hand to the draw pile
        /// if its a wild ensures the changed suit matches the first in the stacked cards.
        /// calls next player when finished. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgsPlayCard"></param>
        protected virtual void StackedCardButtonClick(object sender, EventArgsPlayCard eventArgsPlayCard)
        {
            //add code here to process the played card. 
            Card playedCard = eventArgsPlayCard.UnoCard;
            if (playedCard is CardDraw)
            {   //if this card is played, the top of the discard pile MUST be a wild+4 so no need to test.
                mNumNextPlayerDrawCards += 4;
                (playedCard as CardWild).NextSuit = (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild).NextSuit;
            }
            else if (playedCard is CardDraw)
            {
                mNumNextPlayerDrawCards += 2;
            }
            EventPublisher.SkipGo();
            mDeck.DiscardPile.Add(playedCard);
            mPlayers[mNextPlayerInLine].Cards.Remove(playedCard);
            EventPublisher.NextPlayerButtonClick();
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

        /// <summary>
        /// Overrides the base class. Changes to the if current player to allow for stacked cards.
        /// </summary>
        /// <param name="pPlayer"></param>
        protected override void DrawCard(int pPlayer)
        {
            if (mDeck.DiscardPile.Count == 0 && mDeck.DrawPile.Count == 0)
            {
                MessageBox.Show("Sorry there are no cards left to draw", "no cards left");
                mPlayerHasPicked = true;//set this to allow play to continue.
            }
            else if (mDeck.DrawPile.Count == 0 && mDeck.DiscardPile.Count == 1)
            {
                MoveCardFromDiscardToPlayer(pPlayer);
                mPlayerHasPicked = true;
            }
            else if (mDeck.DrawPile.Count == 0)
            {
                mDeck.DeckRefresh();
                MoveCardFromDrawToPlayer(pPlayer);
                mPlayerHasPicked = true;
            }
            else
            {
                MoveCardFromDrawToPlayer(pPlayer);
                mPlayerHasPicked = true;
            }
            if (pPlayer == mCurrentPlayer)
            {
                mPlayerHasPicked = true;
                mPlayerHasDiscarded = true;//set these to allow play to continue 
                if ((mPlayers[mCurrentPlayer] as PlayerStackable).TurnsToSkip != 0)
                {
                    mPlayers[mCurrentPlayer].SortPlayerCards();
                    EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
                }
            }
        }

        /// <summary>
        /// Overrides the base method by creating PlayerStackable objects instead of Player objects.
        /// </summary>
        /// <param name="pPlayerNames"></param>
        /// <returns></returns>
        protected override List<Player> GenerateNewPlayers(List<string>pPlayerNames)
        {
            List<Player> players = new List<Player>();
            for (int playerIndex = 0; playerIndex < pPlayerNames.Count; playerIndex++)
            {
                PlayerStackable player = new PlayerStackable(playerIndex, pPlayerNames[playerIndex]);
                players.Add(player);
            }
            return players;
        }
    }
}
