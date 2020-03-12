using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno
{
    [Serializable()]
    class UnoGame
    {
        protected List<Player> mPlayers;
        protected Deck mDeck;
        protected bool mforwards;
        protected int mCurrentPlayer;
        protected int mNextPlayerPickupTotal = 0;
        protected int mNextPlayersToSkipTotal = 0;
        protected bool mPlayerHasDiscarded = false;
        protected bool mPlayerHasPicked = false;
        protected Player mWinner = null;
        protected List<int> mCardsDrawnThisTurn;

        public UnoGame ()
        {
            
        }

        public UnoGame(List<string> pPlayerNames, int pdealer)
        {
            if (pPlayerNames.Count <= 10)
            {
                this.mPlayers = GenerateNewPlayers(pPlayerNames);
            }
            else
            {
                //add some exception code here to handle creating too many players
            }
            this.mDeck = new Deck();
            DealCards();
            this.mforwards = true;
            this.mCurrentPlayer = pdealer; //set to the dealer, so when next player is called, it moves to the preson after the dealer. 
            this.mNextPlayerPickupTotal = 0;
            this.mNextPlayersToSkipTotal = 0;
            this.mPlayerHasPicked = true;// set to true initially so that the next player function call works.
            this.mPlayerHasDiscarded = true; // set to true initially so that the next player function call works.
            this.mCardsDrawnThisTurn = new List<int>();
            SubscribeToEvents();
            EventPublisher.NextPlayerButtonClick();//not a button click but does the job
        }

        public virtual void SubscribeToEvents()
        {
            EventPublisher.RaiseColourPick += UnoGame_RaiseColourPick;
            EventPublisher.RaisePlus4Challenge += UnoGame_RaisePlus4Challenge;
            EventPublisher.RaiseDrawTwoCards += UnoGame_RaiseDrawTwoCards;
            EventPublisher.RaiseReverseDirection += UnoGame_RaiseReverseDirection;
            EventPublisher.RaiseSkipGo += UnoGame_RaiseSkipGo;
            EventPublisher.RaiseNextPlayerButtonClick += UnoGame_RaiseNextPlayerButtonClick;
            EventPublisher.RaisePlayCard += UnoGame_RaisePlayCard;
            EventPublisher.RaiseAcceptDraw4 += UnoGame_AcceptDraw4;
            EventPublisher.RaiseDrawCard += UnoGame_DrawCard;
            EventPublisher.RaiseGameButtonClick += UnoGame_RaiseGameButtonClick;
            EventPublisher.RaiseReturnToGame += UnoMain_RaiseReturnToGame;
            EventPublisher.RaiseUnsubscribeEvents += UnoGame_RaiseUnsubscribeEvents;
        }

        protected virtual void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            UnsubscribeFromEvents();
        }

        protected virtual void UnsubscribeFromEvents()
        {
            EventPublisher.RaiseColourPick -= UnoGame_RaiseColourPick;
            EventPublisher.RaisePlus4Challenge -= UnoGame_RaisePlus4Challenge;
            EventPublisher.RaiseDrawTwoCards -= UnoGame_RaiseDrawTwoCards;
            EventPublisher.RaiseReverseDirection -= UnoGame_RaiseReverseDirection;
            EventPublisher.RaiseSkipGo -= UnoGame_RaiseSkipGo;
            EventPublisher.RaiseNextPlayerButtonClick -= UnoGame_RaiseNextPlayerButtonClick;
            EventPublisher.RaisePlayCard -= UnoGame_RaisePlayCard;
            EventPublisher.RaiseAcceptDraw4 -= UnoGame_AcceptDraw4;
            EventPublisher.RaiseDrawCard -= UnoGame_DrawCard;
            EventPublisher.RaiseGameButtonClick -= UnoGame_RaiseGameButtonClick;
            EventPublisher.RaiseReturnToGame -= UnoMain_RaiseReturnToGame;
        }

        protected virtual void CalculateFinalScores()
        {
            int runningTotal = 0;
            foreach (Player player in mPlayers)
            {
                if (player != mWinner)
                {
                    foreach(Card card in player.Cards)
                    {
                        switch(card)
                        {
                            case CardWild cardWild:
                                runningTotal += 50;
                                break;
                            case CardSpecial cardSpecial:
                                runningTotal += 20;
                                break;
                            case CardNumber cardNumber:
                                runningTotal += cardNumber.Number;
                                break;
                        }
                    }
                }
            }
            mWinner.FinalScore = runningTotal;
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, "GameOver");
            UnsubscribeFromEvents();
            EventPublisher.FinalScore(mWinner);
        }

        protected virtual void UnoGame_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            if (!mPlayerHasDiscarded) //block playing more cards if player has discard
            {   //only come here if play is allowed for this player. 
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
            else
            {
                MessageBox.Show("You can not discard more cards this turn, please click next player or draw a card.", "discard error");
            }
        }

        protected virtual bool CheckIfDrawnCard(Card pCard)
        {
            bool drawnCard = false;
            foreach(int uniqueIdentifier in mCardsDrawnThisTurn)
            {
                if (uniqueIdentifier == pCard.UniqueIdentifier)
                {
                    drawnCard = true;
                    break;
                }
            }
            return drawnCard;
        }

        protected virtual void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
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
                for (int num = 0; num < 6; num++)
                {
                    DrawCard(NextPlayerWithoutSkips());
                }
                EventPublisher.SkipGo();
            }
            MessageBox.Show(message, "challenge result");
            mNextPlayerPickupTotal = 0;
            FinishPlaceCard();
            mPlayerHasPicked = true; //set to allow the change of player. 
            EventPublisher.NextPlayerButtonClick();
        }

        protected virtual void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            for (int number = 0; number <4; number++)
            {
                DrawCard(mCurrentPlayer);
            }
            mPlayerHasPicked = true;//set this so the event doesn't refuse to work. 
            EventPublisher.SkipGo();
            EventPublisher.NextPlayerButtonClick();
        }

        protected virtual void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            eventArgs.UnoCard.RunCardSpecialFeatures();
            mDeck.DiscardPile.Add(eventArgs.UnoCard);
            mPlayers[mCurrentPlayer].Cards.Remove(eventArgs.UnoCard);
            if (eventArgs.UnoCard is CardWild)
            {
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, "ChooseColour");
            }
            else 
            { 
                FinishPlaceCard(); 
            }
        }

        protected virtual void UnoGame_RaiseNextPlayerButtonClick(object sender, EventArgs eventArgs)
        {
            if (mPlayerHasDiscarded || mPlayerHasPicked)
            {
                int nextPlayerWithoutSips = NextPlayerWithoutSkips();
                if (mforwards) 
                { 
                    nextPlayerWithoutSips += mNextPlayersToSkipTotal; 
                }
                else 
                { 
                    nextPlayerWithoutSips-= mNextPlayersToSkipTotal; 
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

        protected virtual void UnoGame_RaiseSkipGo(object sender, EventArgs eventArgs)
        {
            mNextPlayersToSkipTotal++;
        }

        protected virtual void UnoGame_RaiseReverseDirection(object sender, EventArgs eventArgs)
        {
            mforwards = !mforwards;
        }

        protected virtual void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            int nextPlayer = NextPlayerWithoutSkips();
            DrawCard(nextPlayer);
            DrawCard(nextPlayer);
            EventPublisher.SkipGo();
        }

        protected virtual void UnoGame_RaiseColourPick(object sender, EventArgsColourPick argsColourPick)
        {
            if (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] is CardWild)
            {
                CardWild cardWild = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild;
                cardWild.NextSuit = argsColourPick.NextSuit;
                if (cardWild.CardsToDraw > 0)
                {
                    EventPublisher.GuiUpdate(mPlayers[NextPlayerWithoutSkips()], mDeck, "Challenge+4");
                }
                else
                {
                    FinishPlaceCard();
                }
            }
        }

        protected virtual void DealCards()
        {
            mDeck.ShuffleDeck(mDeck.DiscardPile);
            foreach (Player player in mPlayers)
            {
                player.Cards = new List<Card>();
                for (int playerCardsIndex = 0; playerCardsIndex < 7; playerCardsIndex++)
                {
                    player.Cards.Add(mDeck.DiscardPile[0]);
                    mDeck.DiscardPile.RemoveAt(0);
                    EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
                }
            }
        }

        protected virtual void FinishPlaceCard()
        {
            mPlayers[mCurrentPlayer].SortPlayerCards();
            mPlayerHasDiscarded = true;
            if (mPlayers[mCurrentPlayer].Cards.Count == 0)
            {
                MessageBox.Show(mPlayers[mCurrentPlayer].Name + ": Has won the game, please select the main menu when ready");
                mWinner = mPlayers[mCurrentPlayer];
                CalculateFinalScores();
            }
            else
            {
                if (mPlayers[mCurrentPlayer].Cards.Count == 1)
                {
                    MessageBox.Show(mPlayers[mCurrentPlayer].Name + ": UNO!");
                }
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            } 
        }

        protected virtual int NextPlayerWithoutSkips()
        {
            int nextPlayer = 0;
            if (mforwards)
            {
                nextPlayer = mCurrentPlayer+1;
            }
            else
            {
                nextPlayer = mCurrentPlayer-1;
            }
            if (nextPlayer < 0)
            {
                nextPlayer = mPlayers.Count - 1;
            }
            else if (nextPlayer >= mPlayers.Count)
            {
                nextPlayer = 0;
            }
            return nextPlayer;
        }

        protected virtual int FixOutOfBounds(int pIndex)
        {
            int toFix = pIndex;
            if (pIndex < 0)
            {
                toFix = mPlayers.Count - 1;
            }
            else if (pIndex >= mPlayers.Count)
            {
                toFix = 0;
            }
            return toFix;
        }

        protected virtual void UnoGame_DrawCard(object sender, EventArgs eventArgs)
        {
            DrawCard(mCurrentPlayer);
        }

        protected virtual void DrawCard(int pPlayer)
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
                mPlayers[mCurrentPlayer].SortPlayerCards();
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }   
        }

        protected virtual void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DiscardPile[0]);
            mCardsDrawnThisTurn.Add(mDeck.DiscardPile[0].UniqueIdentifier);
            mDeck.DiscardPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        protected virtual void MoveCardFromDrawToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DrawPile[0]);
            mCardsDrawnThisTurn.Add(mDeck.DrawPile[0].UniqueIdentifier);
            mDeck.DrawPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        protected virtual void UnoMain_RaiseReturnToGame(object sender, EventArgs eventArgs)
        {
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        protected virtual List<Player> GenerateNewPlayers(List<string> pPlayerNames)
        {
            List<Player> players = new List<Player>();
            for (int playerIndex = 0; playerIndex < pPlayerNames.Count; playerIndex++)
            {
                Player player = new Player(playerIndex, pPlayerNames[playerIndex]);
                players.Add(player);
            }
            return players;
        }

        protected virtual bool CheckIfCardCanBePlayed(Card pCard)
        {
            bool canBePlayed = false;
            Card discardPile = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1];
            switch (discardPile)
            {
                case CardWild discardPileWild:
                    switch (pCard)
                    {
                        case CardSuit playedSuitCard:
                            if (playedSuitCard.Csuit == discardPileWild.NextSuit) canBePlayed = true;
                            break;
                        case CardWild cardWild:
                            canBePlayed = true;
                            break;
                    }
                    break;
                case CardSuit discardPileSuit:
                    switch (pCard)
                    {
                        case CardWild playedWildCard:
                            canBePlayed = true;
                            break;
                        case CardSuit playedSuitCard:
                            if (discardPileSuit.Csuit == playedSuitCard.Csuit) canBePlayed = true;
                            else
                            {   //come here if both cards are suits do they do not match
                                switch (discardPileSuit)
                                {
                                    case CardNumber discardPileNumberCard:
                                        switch (playedSuitCard)
                                        {
                                            case CardNumber playedNumberCard:
                                                if (discardPileNumberCard.Number == playedNumberCard.Number)
                                                {
                                                    canBePlayed = true;
                                                }
                                                break;
                                            case CardSpecial playedSpecialCard:
                                                canBePlayed = false;
                                                break;
                                        }
                                        break;
                                    case CardSpecial discardPileSpecialCard:
                                        switch (playedSuitCard)
                                        {
                                            case CardNumber playedNumberCard:
                                                canBePlayed = false;
                                                break;
                                            case CardSpecial playedSpecialCard:
                                                if (discardPileSpecialCard.Type == playedSpecialCard.Type)
                                                {
                                                    canBePlayed = true;
                                                }
                                                else
                                                {
                                                    canBePlayed = false;
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return canBePlayed;
        }
    }
}
