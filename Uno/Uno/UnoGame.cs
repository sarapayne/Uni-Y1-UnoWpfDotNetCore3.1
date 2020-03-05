using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    class UnoGame
    {
        [NonSerialized]
        private List<Player> mPlayers;

        [NonSerialized]
        private Deck mDeck;     
        
        private bool mforwards;
        private int mCurrentPlayer;
        private GameRules mGameRules;

        public UnoGame (List<string> pPlayerNames, int pdealer, GameRulesType pGameRulesType)
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
        }

        public List <Player> Players
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
                }
            }
        }

        private GameRules SetGameRules(GameRulesType pGameRulesType)
        {
            GameRules gameRules = new GameRules();
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

        public void NextPlayer(int pSkipPlayers)
        {
            int change = pSkipPlayers + 1; //will always be at least 1
            if (!mforwards) change *= -1;
            mCurrentPlayer += change;
            int adjustment = 0;
            if (mCurrentPlayer < 0)
            {   //adjust for going beyond the list size accounting for skipped players
                //if we are at -1 change to last list index, if lower decrease by the difference
                adjustment = mCurrentPlayer +1;
                mCurrentPlayer = (mPlayers.Count - 1) + adjustment;
            }
            else if (mCurrentPlayer >= mPlayers.Count)
            {   //adjust for going beyond the list size accounting for skipped players
                //if we are at mPlayers.Length change to player0, if higher adjust by the difference
                adjustment = mCurrentPlayer - (mPlayers.Count);
                mCurrentPlayer = 0 + adjustment;
            }
            DisplayCurrentPlayerGui();
        }

        private void DisplayCurrentPlayerGui()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
            wpfWindowGame.Show();
        }

        private List<Player> GenerateNewPlayers(List <string> pPlayerNames)
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
