using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    class UnoGame
    {
        /// <summary>
        /// This is the main control point for an individual game.
        /// <param name="mPlayers"> List of player objects</param>
        /// <para name = "mDiscard">List of ints representing the stack of discard cards</para>
        /// <para name = "mDeck">List of ints representing the current deck</para>
        /// <para name = "mForwards">bool controling the direction of play</para>
        /// <para name = "mCurrentPlayer">int which is the index of the current player in the mPlayers list</para>
        /// </summary>
        private List<Player> mPlayers;
        private List<int> mDiscard;
        private List<int> mDeck;
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

            this.mDeck = new List<int>();
            this.mDiscard = GenerateNewDeck(); //add to discard so when next player called it works correctly

            DealPlayerCards();
            RefreshDiscardPile();
            mforwards = true;
            mCurrentPlayer = pdealer;
            int skipPlayers = 0;
        }

        private void NextPlayer(int pSkipPlayers)
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
            //add code here to display the GUI appropriate to the current player

        }

        /// <summary>
        /// Takes the discard pile, turns it over and places the top card on the discard pile for the game to continue.
        /// </summary>
        private void RefreshDiscardPile()
        {
            mDeck = new List<int>();
            mDeck = mDiscard;
            mDiscard = new List<int>();
            mDiscard.Add(mDeck[0]);
            mDeck.RemoveAt(0);
        }

        /// <summary>
        /// For each player, take 7 cards from the top of the deck and assign those cards to the player. 
        /// </summary>
        private void DealPlayerCards()
        {
            foreach (Player player in mPlayers)
            {
                List<int> playerCards = new List<int>();
                for (int playerCardsIndex = 0; playerCardsIndex < 7; playerCardsIndex++)
                {
                    player.Cards.Add(mDeck[0]);
                    mDiscard.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Makes a new deck and then shuffles it randomly.
        /// New deck is created in the discard list so the RefreshDiscardPile() mehtod can be re-used
        /// </summary>
        private List<int> GenerateNewDeck()
        {
            List<int> deck = new List<int>();
            for (int cardIndex = 0; cardIndex < UnoMain.CardDeck.Count; cardIndex++)
            {
                deck.Add(cardIndex);
            }
            ShuffleDeck(deck);
            return deck;
        }

        /// <summary>
        /// takes the current deck and suffles it randomly. 
        /// </summary>
        private void ShuffleDeck(List<int> pDeck)
        {
            Random random = new Random();
            for (int cardIndex = 0; cardIndex < pDeck.Count; cardIndex++)
            {
                int swapIndex = random.Next(0, pDeck.Count - 1);
                int temp = pDeck[cardIndex];
                pDeck[cardIndex] = pDeck[swapIndex];
                pDeck[swapIndex] = temp;
            }
        }

        /// <summary>
        /// uses the player name list to generate a list of new players. 
        /// </summary>
        /// <param name="pPlayerNames"></param>
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
