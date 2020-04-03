using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.Cards;
using Uno.EventsComponents;
using Uno.Interfaces;
using Uno.Players;

namespace Uno
{
    [Serializable()]
    class UnoGame
    {
        protected List<Player> mPlayers;
        protected Deck mDeck;
        protected bool mforwards;
        protected int mCurrentPlayer;
        protected int mNextPlayersToSkipTotal = 0;
        protected bool mPlayerHasDiscarded = false;
        protected bool mPlayerHasPicked = false;
        protected Player mWinner = null;
        protected List<int> mCardsDrawnThisTurn;
        protected int mNumOfSwapHands;

        public UnoGame ()
        {
            
        }

        public UnoGame(List<string> pPlayerNames, int pdealer, int pNumOfSwapHands)
        {
            if (pPlayerNames.Count <= 10)
            {
                this.mPlayers = GenerateNewPlayers(pPlayerNames);
            }
            else
            {   //this should be impossible, its made impossible by GUI limits
                MessageBox.Show("Sorry something has gone wrong, the maximum number of players is 10, please reduce and try again", "too many players");
            }
            this.mNumOfSwapHands = pNumOfSwapHands;
            this.mDeck = new Deck(mNumOfSwapHands);
            DealCards();
            this.mforwards = true;
            this.mCurrentPlayer = pdealer; //set to the dealer, so when next player is called, it moves to the preson after the dealer. 
            this.mNextPlayersToSkipTotal = 0;
            this.mPlayerHasPicked = true;// set to true initially so that the next player function call works.
            this.mPlayerHasDiscarded = true; // set to true initially so that the next player function call works.
            this.mCardsDrawnThisTurn = new List<int>();
            
            SubscribeToEvents();
            mDeck.DeckRefresh();
            EventPublisher.NextPlayerButtonClick();//not a button click but does the job
        }

        /// <summary>
        /// Subscribes to all events needed by the class
        /// </summary>
        public virtual void SubscribeToEvents()
        {
            EventPublisher.RaiseColourPick += WildCardSetNextSuit;
            EventPublisher.RaisePlus4Challenge += ChallengePlus4ButtonClick;
            EventPublisher.RaiseDrawTwoCards += DrawTwoCards;
            EventPublisher.RaiseReverseDirection += ReverseDirection;
            EventPublisher.RaiseSkipGo += SkipGo;
            EventPublisher.RaiseNextPlayerButtonClick += NextPlayerButtonClick;
            EventPublisher.RaisePlayCard += PlayCard;
            EventPublisher.RaiseAcceptDraw4 += AcceptDraw4ButtonClick;
            EventPublisher.RaiseDrawCard += DrawCardButtonClick;
            EventPublisher.RaiseGameButtonClick += DiscardCardButtonClick;
            EventPublisher.RaiseReturnToGame += ReturnToGame;
            EventPublisher.RaiseUnsubscribeEvents += UnSubscribeEvents;
            EventPublisher.RaiseSwapHandsPlayerChosen += SwapHandsPlayerChosen;
        }

        /// <summary>
        /// Unsubscribes from all event subscriptions
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void UnSubscribeEvents(object sender, EventArgs eventArgs)
        {
            EventPublisher.RaiseColourPick -= WildCardSetNextSuit;
            EventPublisher.RaisePlus4Challenge -= ChallengePlus4ButtonClick;
            EventPublisher.RaiseDrawTwoCards -= DrawTwoCards;
            EventPublisher.RaiseReverseDirection -= ReverseDirection;
            EventPublisher.RaiseSkipGo -= SkipGo;
            EventPublisher.RaiseNextPlayerButtonClick -= NextPlayerButtonClick;
            EventPublisher.RaisePlayCard -= PlayCard;
            EventPublisher.RaiseAcceptDraw4 -= AcceptDraw4ButtonClick;
            EventPublisher.RaiseDrawCard -= DrawCardButtonClick;
            EventPublisher.RaiseGameButtonClick -= DiscardCardButtonClick;
            EventPublisher.RaiseReturnToGame -= ReturnToGame;
            EventPublisher.RaiseSwapHandsPlayerChosen -= SwapHandsPlayerChosen;
        }

        /// <summary>
        /// Swaps the hands of the current player, with the chosen player then updates the gui with the current players hand
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsPlayer">Card object of the chosen player.</param>
        protected virtual void SwapHandsPlayerChosen(object sender, EventArgsPlayer eventArgsPlayer)
        {
            List<Card> temp = eventArgsPlayer.ChosenPlayer.Cards;
            eventArgsPlayer.ChosenPlayer.Cards = new List<Card>();
            eventArgsPlayer.ChosenPlayer.Cards = mPlayers[mCurrentPlayer].Cards;
            mPlayers[mCurrentPlayer].Cards = new List<Card>();
            mPlayers[mCurrentPlayer].Cards = temp;
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, "");
        }

        /// <summary>
        /// When a game is won, this loops through all other players totaling the value of the 
        /// cards in their combined hands to calculate the winners final score. 
        /// Sends to the final score gui via event
        /// Removes all event subscriptions
        /// </summary>
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
            UnSubscribeEvents(null, null);
            EventPublisher.FinalScore(mWinner);
        }

        /// <summary>
        /// Checks the player has the right to play a card
        /// Checks to make sure the card is a legitimate card to play
        /// If both ok, sends a play card event, else message error. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">Card object selected</param>
        protected virtual void DiscardCardButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            if (!mPlayerHasDiscarded) //block playing more cards if player has discard
            {   //only come here if play is allowed for this player. 
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
            else
            {
                MessageBox.Show("You can not discard more cards this turn, please click next player or draw a card.", "discard error");
            }
        }

        /// <summary>
        /// Used after a player has draw a card, to ensure that they only play one of the cards
        /// they have picked up. Stops playing other cards after that point. As cards are picked
        /// up they get added to the list this loops through and checks. 
        /// </summary>
        /// <param name="pCard">Card object selected</param>
        /// <returns></returns>
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

        /// <summary>
        /// When a player challenges the use of a +4 card this method evaluates if the challenge is valid. 
        /// If the player who used +4 did so in an unallowed way they pick up 4 cards, play passes without a skip.
        /// If the use was legitimate the challenger picks up 6 cards and skips a go. 
        /// Feedback given to players. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void ChallengePlus4ButtonClick(object sender, EventArgs eventArgs)
        {
            string message = mPlayers[NextPlayerWithoutSkips()].Name + "has challenged " + mPlayers[mCurrentPlayer].Name + "'s use of a +4 card";
            MessageBox.Show(message, "+4 challenge");
            bool hadPlayableCard = false;
            foreach (Card card in mPlayers[mCurrentPlayer].Cards)
            {
                bool playableCardFound = CheckIfCardCanBePlayed(card, 1);//the 0 is the offset from the last discarded card. 
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
            mPlayerHasPicked = true; //set to allow the change of player.
            mPlayerHasDiscarded = true; //set to block the playing of more cards.
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// Player accepted the use of a +4 card, they draw 4 cards and skip a turn. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void AcceptDraw4ButtonClick(object sender, EventArgs eventArgs)
        {
            for (int number = 0; number <4; number++)
            {
                DrawCard(NextPlayerWithoutSkips());
            }
            mPlayerHasDiscarded = true;//set this so the next player method doesn't refuse to work. and stops playing of more cards. 
            //mPlayerHasPicked = true;
            EventPublisher.SkipGo();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// executes special card features
        /// moves card from player to discard pile
        /// if card is wild triggers colour picker
        /// if not sends to finish place card method. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">selected card object</param>
        protected virtual void PlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            if (eventArgs.UnoCard is IFeatureCard)
            {
                (eventArgs.UnoCard as IFeatureCard).CardFeatures();
            }
                
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

        /// <summary>
        /// When end turn is clicked this event checks turn passing is allowed, then applies skips
        /// normal next player calculations. Fixing out of bounds for list range. 
        /// Resets Turn Variables, then sends an update to the gui with the new player information and 
        /// current deck status. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void NextPlayerButtonClick(object sender, EventArgs eventArgs)
        {
            if (mPlayerHasDiscarded || mPlayerHasPicked)
            {
                //int startPlayer = mCurrentPlayer;
                int nextPlayer = mCurrentPlayer; 
                for (int skips = 0; skips < mNextPlayersToSkipTotal + 1; skips++) //add one because the player always needs to change by at least one person.
                {
                    nextPlayer = NextPlayerWithoutSips(nextPlayer);
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
        /// Resets the variables for this turn for the next player.
        /// </summary>
        protected virtual void ResetTurnVariblesForNextPlayer()
        {
            mNextPlayersToSkipTotal = 0;
            mPlayerHasPicked = false;
            mPlayerHasDiscarded = false;
            mCardsDrawnThisTurn.Clear();
        }

        /// <summary>
        /// This increase when skips are added, could currently just be a bool as 0> is all that is 
        /// evaluated, but set this way for more alternate rules to come later. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void SkipGo(object sender, EventArgs eventArgs)
        {
            mNextPlayersToSkipTotal++;
        }

        /// <summary>
        /// just inverts the bool used to evaluate the direction of play 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void ReverseDirection(object sender, EventArgs eventArgs)
        {
            mforwards = !mforwards;
        }

        /// <summary>
        /// causes the next player to pickup two cards, and skip a go
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void DrawTwoCards(object sender, EventArgs eventArgs)
        {
            int nextPlayer = NextPlayerWithoutSkips();
            DrawCard(nextPlayer);
            DrawCard(nextPlayer);
            EventPublisher.SkipGo();
        }

        /// <summary>
        /// Takes the suit sent in the event args by the gui and applies it to the
        /// discared cards next suit category, this is used after to update the image
        /// on the gui and check the follow cards agaisnt it. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="argsColourPick">enum suit</param>
        protected virtual void WildCardSetNextSuit(object sender, EventArgsColourPick argsColourPick)
        {
            if (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] is CardWild)
            {
                CardWild cardWild = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild;
                cardWild.NextSuit = argsColourPick.NextSuit;
                FinishPlaceCard();
                if (cardWild.CardsToDraw > 0)
                {
                    EventPublisher.GuiUpdate(mPlayers[NextPlayerWithoutSkips()], mDeck, "Challenge+4");
                }
                else if (cardWild is CardWildSwapHands)
                {
                    List<Player> otherPlayers = new List<Player>();
                    foreach(Player player in mPlayers)
                    {   //we dont want to be able to swap hands with ourself so make a new list without the current player
                        if (player.Name != mPlayers[mCurrentPlayer].Name)
                        {
                            otherPlayers.Add(player);
                        }
                    }
                    EventPublisher.HideGuiWindows();
                    EventPublisher.SwapHandsPlayerChoose(otherPlayers, mPlayers[mCurrentPlayer]);
                }
            }
        }

        /// <summary>
        /// Called after the deck is generated/loaded
        /// Shuffles the deck, then gives 7 cards to each player. 
        /// </summary>
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

        /// <summary>
        /// laste stage of a card being played, the cards are sorted, then a check for a winner is made
        /// if a winner is found its sent to calculate the final sores, if no winner is found
        /// a check for uno is made and announced, 
        /// the new player and current deck is sent to the gui via an event to update. 
        /// </summary>
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

        /// <summary>
        /// returns the next player assuming no skips are applied allowing for 
        /// direction of play, and corrects any out of bounds issues. 
        /// </summary>
        /// <returns>index of the player</returns>
        protected virtual int NextPlayerWithoutSkips()
        {
            return NextPlayerWithoutSips(mCurrentPlayer);
        }

        /// <summary>
        /// returns the next player which follows the provided player number. 
        /// </summary>
        /// <param name="pStartPlayer"></param>
        /// <returns></returns>
        protected virtual int NextPlayerWithoutSips(int pStartPlayer)
        {
            int nextPlayer = 0;
            if (mforwards)
            {
                nextPlayer = pStartPlayer + 1;
            }
            else
            {
                nextPlayer = pStartPlayer - 1;
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

        /// <summary>
        /// passes the event instruction to the DrawCard method
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        protected virtual void DrawCardButtonClick(object sender, EventArgs eventArgs)
        {
            mPlayerHasPicked = true;
            DrawCard(mCurrentPlayer);
        }

        /// <summary>
        /// Takes a card from the Draw pile and adds it to the players card if possible
        /// checks to make sure there are enough cards in the discard pile to refresh it
        /// if there are the piles are refreshed then a card is drawn, if there are not the
        /// card is draw from the discard pile, unless there are no cards, then an error is
        /// passed by message and variables set to allow game play to contine anyway. 
        /// finally the current player and deck are sent to the gui to update the display. 
        /// </summary>
        /// <param name="pPlayer">index of the player</param>
        protected virtual void DrawCard(int pPlayer)
        {
            if (mDeck.DiscardPile.Count == 0 && mDeck.DrawPile.Count == 0)
            {
                MessageBox.Show("Sorry there are no cards left to draw", "no cards left");
            }
            else if (mDeck.DrawPile.Count == 0 && mDeck.DiscardPile.Count == 1)
            {
                MoveCardFromDiscardToPlayer(pPlayer);
            }
            else if (mDeck.DrawPile.Count == 0)
            {
                mDeck.DeckRefresh();
                MoveCardFromDrawToPlayer(pPlayer);
            }
            else
            {
                MoveCardFromDrawToPlayer(pPlayer);
            }
            if (pPlayer == mCurrentPlayer)
            {
                mPlayers[mCurrentPlayer].SortPlayerCards();
            }   
        }

        /// <summary>
        /// takes a card from the discard pile to the players hand
        /// rare case but can happen with only one card in the deck. 
        /// </summary>
        /// <param name="pPlayer">player index</param>
        protected virtual void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DiscardPile[0]);
            mCardsDrawnThisTurn.Add(mDeck.DiscardPile[0].UniqueIdentifier);
            mDeck.DiscardPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// normal process for drawing a card, the top card in the pile is added 
        /// to the players cards. 
        /// </summary>
        /// <param name="pPlayer"></param>
        protected virtual void MoveCardFromDrawToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DrawPile[0]);
            mCardsDrawnThisTurn.Add(mDeck.DrawPile[0].UniqueIdentifier);
            mDeck.DrawPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// Used after the loading of a game to call the current state of play to the gui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void ReturnToGame(object sender, EventArgs eventArgs)
        {
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        /// <summary>
        /// takes the list of names passed by the event and generates a player object for
        /// each entry in the list. 
        /// </summary>
        /// <param name="pPlayerNames"></param>
        /// <returns>a list of player objects</returns>
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

        /// <summary>
        /// Evaluates if the card being selected by the player can be played or not
        /// Based on the current card in the discared pile. This does not check to 
        /// See if the player has already played, that is handled seperately. 
        /// </summary>
        /// <param name="pCard"></param>
        /// <returns></returns>
        protected virtual bool CheckIfCardCanBePlayed(Card pPlayedCard, int pOffset)
        {//the offset is from the last discared card, used by +4 challenge
            bool canBePlayed = false;
            if (!(mDeck.DiscardPile.Count == 0 || mDeck.DiscardPile == null))
            {
                if(mDeck.DiscardPile.Count < 2 )
                {   //catch the edge case where someone played a plus + card after all other cards were exhausted
                    pOffset = 0;
                }
                Card checkAgainstCard = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1 - pOffset];
                switch (checkAgainstCard)
                {   //start evaluation by looking at the discard pile
                    case CardWild discardPileWild:
                        //discard pile is wild. 
                        switch (pPlayedCard)
                        {   //Evaluate the card being played. 
                            case CardSuit playedSuitCard:
                                //if the suit matches the wild card in the discard pile ok
                                if (playedSuitCard.Csuit == discardPileWild.NextSuit) canBePlayed = true;
                                break;
                            case CardWild cardWild:
                                //if the card being played is wild ok, it can go on anything. 
                                canBePlayed = true;
                                break;
                        }
                        break;
                    case CardSuit discardPileSuit:
                        //card on discard pile is a suit card.
                        switch (pPlayedCard)
                        //evaluate next against the card being played.
                        {
                            case CardWild playedWildCard:
                                //card being played is wild, it can go on anything, ok. 
                                canBePlayed = true;
                                break;
                            case CardSuit playedSuitCard:
                                //played card is a suit card, and so is the discard pile.
                                if (discardPileSuit.Csuit == playedSuitCard.Csuit) canBePlayed = true; //matching suits. ok.
                                else
                                {   //come here if both cards are suits do they do not match
                                    switch (discardPileSuit)
                                    {
                                        case CardNumber discardPileNumberCard:
                                            //discard pile is a number card 
                                            switch (playedSuitCard)
                                            {   //check against the played card
                                                case CardNumber playedNumberCard:
                                                    if (discardPileNumberCard.Number == playedNumberCard.Number)
                                                    {   //suites are different but the numbers match ok.
                                                        canBePlayed = true;
                                                    }
                                                    break;
                                                case CardSpecial playedSpecialCard:
                                                    //card played is skip/draw/reverse but discard is a number card
                                                    canBePlayed = false; //not ok as this is not a match. 
                                                    break;
                                            }
                                            break;
                                        case CardSpecial discardPileSpecialCard:
                                            //discard pile is a draw/skip/reverse
                                            switch (playedSuitCard)
                                            {   //check against the played card, which is a different suit
                                                case CardNumber playedNumberCard:
                                                    //played card is a number card and suites do not match
                                                    canBePlayed = false; //number card doesn't match a special
                                                    break;
                                                case CardSpecial playedSpecialCard:
                                                    //played card is a special card.
                                                    switch (playedSpecialCard)
                                                    {
                                                        case CardDraw playedDrawCard:
                                                            if (discardPileSpecialCard is CardDraw)
                                                            {
                                                                canBePlayed = true;
                                                            }
                                                            else
                                                            {
                                                                canBePlayed = false;
                                                            }
                                                            break;
                                                        case CardReverse playedReverseCard:
                                                            if (discardPileSpecialCard is CardReverse)
                                                            {
                                                                canBePlayed = true;
                                                            }
                                                            else
                                                            {
                                                                canBePlayed = false;
                                                            }
                                                            break;
                                                        case CardSkip playedSkipCard:
                                                            if (discardPileSpecialCard is CardSkip)
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
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                canBePlayed = true;
            }
            return canBePlayed;
            
        }
    }
}
