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
        private List<Player> mPlayers;
        private Deck mDeck;
        private bool mforwards;
        private int mCurrentPlayer;
        private GameRules mGameRules;
        private int mNextPlayerPickupTotal = 0;
        private int mNextPlayersToSkipTotal = 0;
        private bool mPlayerHasDiscarded = false;
        private bool mPlayerHasPicked = false;
        private int mLastPlayer;
        private bool mPlus4Processed;

        public UnoGame(List<string> pPlayerNames, int pdealer, GameRulesType pGameRulesType)
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
            this.mGameRules = SetGameRules(pGameRulesType);
            this.mNextPlayerPickupTotal = 0;
            this.mNextPlayersToSkipTotal = 0;
            this.mPlayerHasPicked = true;// set to true initially so that the next player function call works.
            this.mPlayerHasDiscarded = true; // set to true initially so that the next player function call works.
            this.mPlus4Processed = false;
            EventPublisher.RaiseColourPick += UnoGame_RaiseColourPick;
            EventPublisher.RaisePlus4Challenge += UnoGame_RaisePlus4Challenge;
            EventPublisher.RaiseDrawFourCards += UnoGame_RaiseDrawFourCards;
            EventPublisher.RaiseDrawTwoCards += UnoGame_RaiseDrawTwoCards;
            EventPublisher.RaiseReverseDirection += UnoGame_RaiseReverseDirection;
            EventPublisher.RaiseSkipGo += UnoGame_RaiseSkipGo;
            EventPublisher.RaiseNextPlayerButtonClick += UnoGame_RaiseNextPlayerButtonClick;
            EventPublisher.RaisePlayCard += UnoGame_RaisePlayCard;
            EventPublisher.RaiseAcceptDraw4 += UnoGame_AcceptDraw4;
            StartNewGuiInteface();
        }

        public bool PlayerHasPicked
        {
            get { return this.mPlayerHasPicked; }
        }

        public bool PlayerHasDiscared
        {
            get { return this.mPlayerHasDiscarded; }
        }

        public int NextPlayerPickup
        {
            get { return mNextPlayerPickupTotal; }
            set { this.mNextPlayerPickupTotal = value; }
        }

        public int NextPlayersSkip
        {
            get { return this.mNextPlayersToSkipTotal; }
            set { this.mNextPlayersToSkipTotal = value; }
        }

        public List<Player> Players
        {
            get { return this.mPlayers; }
            set { this.mPlayers = value; }
        }

        public Deck Deck
        {
            get { return this.mDeck; }
            set { this.mDeck = value; }
        }

        public int CurrentPlayer
        {
            get { return this.mCurrentPlayer; }
        }

        public void RefreshCardPiles()
        {
            mDeck.RefreshCardPiles();
        }

        private void StartNewGuiInteface()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
        }

        private void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            for (int number = 0; number <4; number++)
            {
                DrawCard(mCurrentPlayer);
            }
            mPlayerHasPicked = true;//set this so the event doesn't refuse to work. 
            mPlus4Processed = true;
            EventPublisher.NextPlayerButtonClick();
        }

        private void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            eventArgs.UnoCard.RunCardSpecialFeatures();
            mDeck.DiscardPile.Add(eventArgs.UnoCard);
            mPlayers[CurrentPlayer].Cards.Remove(eventArgs.UnoCard);
            if (eventArgs.UnoCard is CardWild)
            {
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, "ChooseColour");
            }
            else 
            { 
                FinishPlaceCard(); 
            }
        }

        private void UnoGame_RaiseNextPlayerButtonClick(object sender, EventArgs eventArgs)
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
                mPlayers[CurrentPlayer].SortPlayerCards();
                if (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] is CardWild)
                {
                    if (!mPlus4Processed)
                    {
                        CardWild cardWild = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild;
                        if (cardWild.CardsToDraw > 0)
                        {
                            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, "Challenge+4");
                        }
                    }
                    else
                    {
                        mPlus4Processed = false;
                        EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
                    }
                }
                else
                {
                    EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
                }
            }
            else
            {
                MessageBox.Show("Sorry you need to either pickup or play a card before you pass the turn to the next player", "player change error");
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }
        }

        private void UnoGame_RaiseDrawFourCards(object sender, EventArgs eventArgs)
        {
            int nextPlayerWithoutSkips = NextPlayerWithoutSkips();
            for (int cardNum = 0; cardNum < 4; cardNum++)
            {
                DrawCard(nextPlayerWithoutSkips);
            }
            EventPublisher.SkipGo();
        }

        private void UnoGame_RaiseSkipGo(object sender, EventArgs eventArgs)
        {
            mNextPlayersToSkipTotal++;
        }

        private void UnoGame_RaiseReverseDirection(object sender, EventArgs eventArgs)
        {
            mforwards = !mforwards;
        }

        private void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            int nextPlayer = NextPlayerWithoutSkips();
            DrawCard(nextPlayer);
            DrawCard(nextPlayer);
            EventPublisher.SkipGo();
        }

        private void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            int lastPlayer = 0;
            string message = mPlayers[mCurrentPlayer].Name + "has challenged " + mPlayers[mLastPlayer].Name + "'s use of a +4 card";
            MessageBox.Show(message, "+4 challenge");
            bool hadPlayableCard = false;
            foreach(Card card in mPlayers[mLastPlayer].Cards)
            {
                bool playableCardFound = mGameRules.CheckIfCardCanBePlayed(card);
                if (playableCardFound)
                {
                    if (! (card is CardWild) )
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
            int playerToDraw = 0;
            if (hadPlayableCard) 
            { 
                playerToDraw = lastPlayer;
                message = mPlayers[mCurrentPlayer].Name + " won the challenge, " + mPlayers[mLastPlayer].Name + " draws 4 cards";
            }
            else 
            { 
                playerToDraw = mCurrentPlayer;
                message = message = mPlayers[mCurrentPlayer].Name + " lost the challenge and draws 4 cards";
            }
            MessageBox.Show(message, "challenge result");
            for (int numCard = 0; numCard < 4; numCard++)
            {
                DrawCard(playerToDraw);
            }
        }

        private void UnoGame_RaiseColourPick(object sender, EventArgsColourPick argsColourPick)
        {
            if (mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] is CardWild)
            {
                CardWild cardWild = mDeck.DiscardPile[mDeck.DiscardPile.Count - 1] as CardWild;
                cardWild.NextSuit = argsColourPick.NextSuit;
                FinishPlaceCard();
            }
        }

        private void DealCards()
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

        private GameRules SetGameRules(GameRulesType pGameRulesType)
        {
            GameRules gameRules = null;
            switch (pGameRulesType)
            {
                case GameRulesType.Standard:
                    gameRules = new GameRules();
                    break;
                case GameRulesType.House1:
                    gameRules = new GameRulesHouse1();
                    break;
                case GameRulesType.House2:
                    gameRules = new GameRulesHouse2();
                    break;
                case GameRulesType.House3:
                    gameRules = new GameRulesHouse3();
                    break;
            }
            return gameRules;
        }

        private void FinishPlaceCard()
        {
            mPlayers[mCurrentPlayer].SortPlayerCards();
            mPlayerHasDiscarded = true;

            if (mPlayers[mCurrentPlayer].Cards.Count == 1)
            {
                MessageBox.Show(mPlayers[mCurrentPlayer].Name + ": UNO!");
            }
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        private int NextPlayerWithoutSkips()
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

        public int FixOutOfBounds(int pIndex)
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

        public void DrawCard()
        {
            DrawCard(mCurrentPlayer);
        }

        private void DrawCard(int pPlayer)
        {
            if (mDeck.DrawPile.Count > 0)
            {
                MoveCardFromDrawToPlayer(pPlayer);
            }
            else
            {
                RefreshCardPiles();
                if (mDeck.DrawPile.Count > 0)
                {
                    if (mDeck.DiscardPile.Count > 0)
                    {
                        MoveCardFromDiscardToPlayer(pPlayer);
                    }
                    else MessageBox.Show("Sorry there are no cards left to draw", "no cards left");
                }
            }
            if (pPlayer == mCurrentPlayer)
            {
                mPlayerHasPicked = true;
                mPlayers[mCurrentPlayer].SortPlayerCards();
                EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
            }   
        }

        private void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DiscardPile[0]);
            mDeck.DiscardPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        private void MoveCardFromDrawToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DrawPile[0]);
            mDeck.DrawPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.GuiUpdate(mPlayers[mCurrentPlayer], mDeck, null);
        }

        private List<Player> GenerateNewPlayers(List<string> pPlayerNames)
        {
            List<Player> players = new List<Player>();
            for (int playerIndex = 0; playerIndex < pPlayerNames.Count; playerIndex++)
            {
                Player player = new Player(playerIndex, pPlayerNames[playerIndex]);
                players.Add(player);
            }
            return players;
        }
    }
}
