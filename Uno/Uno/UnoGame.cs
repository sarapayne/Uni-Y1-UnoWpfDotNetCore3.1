using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;
using Uno.View;

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
            EventPublisher.RaiseColourPick += UnoGame_RaiseColourPick;
            EventPublisher.RaisePlus4Challenge += UnoGame_RaisePlus4Challenge;
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

        public void ReverseDirection()
        {
            mforwards = !mforwards;
        }


        public void RefreshCardPiles()
        {
            mDeck.RefreshCardPiles();
        }

        private void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            int lastPlayer = 0;
            if (mforwards) lastPlayer = mCurrentPlayer-1;
            else lastPlayer = mCurrentPlayer+1;
            if (lastPlayer < 0) lastPlayer = mPlayers.Count - 1;
            else if (lastPlayer >= mPlayers.Count) lastPlayer = 0;
            string message = "Player " + mCurrentPlayer.ToString() + "has challenged " + lastPlayer.ToString() + "'s use of a +4 card";
            MessageBox.Show(message, "+4 challenge");
            bool hadPlayableCard = false;
            foreach(Card card in mPlayers[lastPlayer].Cards)
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
                message = mPlayers[mCurrentPlayer].Name + " won the challenge, " + mPlayers[lastPlayer].Name + " draws 4 cards";
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
                    EventPublisher.UpdateGUI();
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

        public void PlaceCard(Card card)
        {
            mDeck.DiscardPile.Add(card);
            mPlayers[CurrentPlayer].Cards.Remove(card);
            if (card is CardWild)
            {
                WpfWindowChooseColour wpfWindowChooseColour = new WpfWindowChooseColour();
                wpfWindowChooseColour.Show();
            }
            else { FinishPlaceCard(); }
        }

        private void FinishPlaceCard()
        {
            mPlayers[mCurrentPlayer].SortPlayerCards();
            mPlayerHasDiscarded = true;

            if (mPlayers[mCurrentPlayer].Cards.Count == 1)
            {
                MessageBox.Show(mPlayers[mCurrentPlayer].Name + ": UNO!");
            }
            EventPublisher.UpdateGUI();
        }

        public void NextPlayer()
        {
            if (mPlayerHasDiscarded || mPlayerHasPicked)
            {
                int change = mNextPlayersToSkipTotal + 1; //will always be at least 1
                if (!mforwards) change *= -1;
                mCurrentPlayer += change;
                int adjustment = 0;
                if (mCurrentPlayer < 0)
                {   //adjust for going beyond the list size accounting for skipped players
                    //if we are at -1 change to last list index, if lower decrease by the difference
                    adjustment = mCurrentPlayer + 1;
                    mCurrentPlayer = (mPlayers.Count - 1) + adjustment;
                }
                else if (mCurrentPlayer >= mPlayers.Count)
                {   //adjust for going beyond the list size accounting for skipped players
                    //if we are at mPlayers.Length change to player0, if higher adjust by the difference
                    adjustment = mCurrentPlayer - (mPlayers.Count);
                    mCurrentPlayer = 0 + adjustment;
                }
                for (int cardsToDraw = 0; cardsToDraw < mNextPlayerPickupTotal; cardsToDraw++)
                {
                    DrawCard(mCurrentPlayer);
                }
                mNextPlayerPickupTotal = 0;
                mNextPlayersToSkipTotal = 0;
                mPlayerHasPicked = false;
                mPlayerHasDiscarded = false;
                mPlayers[CurrentPlayer].SortPlayerCards();
                DisplayCurrentPlayerGui();
            }
            else
            {
                MessageBox.Show("Sorry you need to either pickup or play a card before you pass the turn to the next player", "player change error");
                DisplayCurrentPlayerGui();
            }
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
                EventPublisher.UpdateGUI();
            }   
        }

        private void MoveCardFromDiscardToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DiscardPile[0]);
            mDeck.DiscardPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.UpdateGUI();
        }

        private void MoveCardFromDrawToPlayer(int pPlayer)
        {
            mPlayers[pPlayer].Cards.Add(mDeck.DrawPile[0]);
            mDeck.DrawPile.RemoveAt(0);
            mPlayers[pPlayer].SortPlayerCards();
            EventPublisher.UpdateGUI();
        }

        private void DisplayCurrentPlayerGui()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
            wpfWindowGame.Show();
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
