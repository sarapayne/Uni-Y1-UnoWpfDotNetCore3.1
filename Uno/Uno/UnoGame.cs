using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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
        private bool mPlayerHasPickedorDiscard = false;

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
            this.mPlayerHasPickedorDiscard = true;// set to true initially so that the next player function call works. 
        }

        public bool PlayerHasPickedUpOrDiscarded
        {
            get { return this.mPlayerHasPickedorDiscard; }
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
            mPlayers[mCurrentPlayer].SortPlayerCards();
            mPlayerHasPickedorDiscard = true;
            if (mPlayers[mCurrentPlayer].Cards.Count == 1)
            {
                MessageBox.Show(mPlayers[mCurrentPlayer].Name + ": UNO!");
            }
            EventPublisher.UpdateGUI();
        }

        public void NextPlayer()
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
                DrawCard();
            }
            mNextPlayerPickupTotal = 0;
            mNextPlayersToSkipTotal = 0;
            mPlayerHasPickedorDiscard = false;
            mPlayers[CurrentPlayer].SortPlayerCards();
            DisplayCurrentPlayerGui();
        }

        public void DrawCard()
        {
            if (mDeck.DrawPile.Count > 0)
            {
                MoveCardFromDrawToPlayer();
            }
            else
            {
                RefreshCardPiles();
                if (mDeck.DrawPile.Count > 0)
                {
                    if (mDeck.DiscardPile.Count > 0)
                    {
                        MoveCardFromDiscardToPlayer();
                    }
                    else MessageBox.Show("Sorry there are no cards left to draw", "no cards left");
                }
            }
            mPlayerHasPickedorDiscard = true;
            mPlayers[mCurrentPlayer].SortPlayerCards();
            EventPublisher.UpdateGUI();
        }

        private void MoveCardFromDiscardToPlayer()
        {
            mPlayers[CurrentPlayer].Cards.Add(mDeck.DiscardPile[0]);
            mDeck.DiscardPile.RemoveAt(0);
            mPlayers[mCurrentPlayer].SortPlayerCards();
            EventPublisher.UpdateGUI();
        }

        private void MoveCardFromDrawToPlayer()
        {
            mPlayers[CurrentPlayer].Cards.Add(mDeck.DrawPile[0]);
            mDeck.DrawPile.RemoveAt(0);
            mPlayers[mCurrentPlayer].SortPlayerCards();
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
