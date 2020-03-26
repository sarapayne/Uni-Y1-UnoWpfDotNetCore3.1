using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Image = System.Windows.Controls.Image;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Http;
using Uno.Cards;

namespace Uno
{
    [Serializable]
    class Deck
    {
        protected string mCardDeckFileName = "CardDeck.bin";
        protected List<Card> mDrawPile;
        protected List<Card> mDiscardPile;
        protected int mUniqueIdentifier;
        protected int mNumOfSwapHands;

        public Deck(int pNumOfSwapHands)
        {
            this.mNumOfSwapHands = pNumOfSwapHands;
            LoadFullCardDeck();//add cards to the discard originally so it works with refresh piles correctly
            this.mDrawPile = new List<Card>();
            mUniqueIdentifier = 0;
            
        }

        public List<Card> DrawPile
        {
            get { return this.mDrawPile; }
            set { this.mDrawPile = value; }
        }

        public List<Card> DiscardPile
        {
            get { return this.mDiscardPile; }
            set { this.mDiscardPile = value; }
        }

        /// <summary>
        /// Takes the last card on the discard pile and preserves it. 
        /// Then moves the discard pile to the draw pile
        /// Adds the preserved card to the discard pile
        /// Shuffles the draw pile. 
        /// </summary>
        public virtual void DeckRefresh()
        {
            ResetWildCards(mDiscardPile);
            MakeSureTopCardNotWild(mDiscardPile);
            mDrawPile = new List<Card>();
            mDrawPile = mDiscardPile;
            mDiscardPile = new List<Card>();
            mDiscardPile.Add(mDrawPile[mDrawPile.Count-1]);
            mDrawPile.RemoveAt(mDrawPile.Count-1);
            ShuffleDeck(mDrawPile);
        }

        /// <summary>
        /// //takes the provided list of cards and shuffles them. 
        /// </summary>
        /// <param name="pCardList">list of cards</param>
        public virtual void ShuffleDeck(List<Card> pCardList)
        {
            Random random = new Random();
            for (int cardIndex = 0; cardIndex < mDiscardPile.Count; cardIndex++)
            {
                int swapIndex = random.Next(0, mDiscardPile.Count - 1);
                Card temp = mDiscardPile[cardIndex];
                mDiscardPile[cardIndex] = mDiscardPile[swapIndex];
                mDiscardPile[swapIndex] = temp;
            }
        }

        /// <summary>
        /// loops through the provided list of cards, resetting the next
        /// suit value of any wild cards to an unused state
        /// </summary>
        /// <param name="pCardList">list of cards</param>
        protected virtual void ResetWildCards(List<Card> pCardList)
        {
            foreach (Card card in pCardList)
            {
                if (card is CardWild)
                {
                    CardWild cardWild = card as CardWild;
                    cardWild.NextSuit = Suit.Unused;
                }
            }
        }

        /// <summary>
        /// After the deck is refreshed, ensure the top card is not wild
        /// if the top card is wild, place it at the bottom, repeat untill the top 
        /// card is not wild. 
        /// </summary>
        /// <param name="pCardList"></param>
        protected virtual void MakeSureTopCardNotWild(List<Card> pCardList)
        {
            while (pCardList[0] is CardWild)
            {
                Card temp = pCardList[0];
                pCardList.RemoveAt(0);
                pCardList.Add(temp);
            }
        }
                
        /// <summary>
        /// if a saved version of the deck exists load it, if not generate a new set of cards
        /// </summary>
        protected virtual void LoadFullCardDeck()
        {
            /* Disabled into i figure out how to include load and save into the new changeable cards.
             * This might have to just get discared. Not big deal.
            try
            {
                using (Stream stream = File.Open(mCardDeckFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    mDiscardPile = (List<Card>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "load error");
                mDiscardPile = new List<Card>();
                GenerateCardList();
            }
            */
            GenerateCardList();
        }

        /// <summary>
        /// When no saved deck of cards is found, generate and entirely new deck. 
        /// Save the new deck to save generating each time in the future. 
        /// </summary>
        protected virtual void GenerateCardList()
        {
            mDiscardPile = new List<Card>();
            List<Suit> suites = new List<Suit> { Suit.Red, Suit.Green, Suit.Blue, Suit.Yellow };
            foreach (Suit suit in suites)
            {   //generate each suit one after the other. 
                string colour = "";
                switch (suit)
                {
                    case Suit.Red:
                        colour = "red";
                        break;
                    case Suit.Green:
                        colour = "green";
                        break;
                    case Suit.Blue:
                        colour = "blue";
                        break;
                    case Suit.Yellow:
                        colour = "yellow";
                        break;
                }
                GenerateSuitCards(suit, colour);
            }// after the suit cards are made also generate the wild cards. 
            GenerateWildCards();
            //SaveFullCardDeck(mDiscardPile);
        }

        /// <summary>
        /// Generates all the wild cards in the deck.
        /// </summary>
        protected virtual void GenerateWildCards()
        {
            string imageStandardName = "card_front_wild_standard";
            string imagePickupName = "card_front_wild_pickup";
            CardWild cardWildStandard = new CardWild(imageStandardName, 0);
            CardWild cardWildPickup = new CardWild(imagePickupName, 4);
            AddCardToDeck(cardWildStandard, 4); //four of each
            AddCardToDeck(cardWildPickup, 4); //four of each
            if (mNumOfSwapHands > 0)
            {
                string imageSwapHandsName = "card_front_wild_swaphands";
                CardWildSwapHands cardWildSwapHands = new CardWildSwapHands(imageSwapHandsName, 0);
                AddCardToDeck(cardWildSwapHands, mNumOfSwapHands);
            }
            
        }

        /// <summary>
        /// Takes the supplied parameters and uses them to generate
        /// All the number cards in the given suit and then all the 
        /// special cards in the given suit. 
        /// </summary>
        /// <param name="pSuit">emum of the suit</param>
        /// <param name="pColour">name to match the enum, used to make the image name</param>
        protected virtual void GenerateSuitCards(Suit pSuit, string pColour)
        {
            GenerateNumberCards(pSuit, pColour);
            GenerateSpecialCards(pSuit, pColour);
        }

        /// <summary>
        /// takes the supplied suit and generates all the special cards in that suit
        /// 
        /// </summary>
        /// <param name="pSuit">enum suit</param>
        /// <param name="pColour">string matching the emum</param>
        protected virtual void GenerateSpecialCards(Suit pSuit, string pColour)
        {
            List<string> specialTypes = new List<string> { "Draw", "Reverse", "Skip" };
            string prefixFileName = "card_front_suit_" + pColour + "_";
            string imgFileName = "";
            foreach (string specialType in specialTypes)
            {
                string tail = "";
                switch (specialType)
                {
                    case "Draw":
                        tail = "draw";
                        imgFileName = prefixFileName + tail;
                        CardDraw cardDraw = new CardDraw(imgFileName, pSuit);
                        AddCardToDeck(cardDraw, 2);//two of each
                        break;
                    case "Reverse":
                        tail = "reverse";
                        imgFileName = prefixFileName + tail;
                        CardReverse cardReverse = new CardReverse(imgFileName, pSuit);
                        AddCardToDeck(cardReverse, 2);//two of each
                        break;
                    case "Skip":
                        tail = "skip";
                        imgFileName = prefixFileName + tail;
                        CardSkip cardSkip = new CardSkip(imgFileName, pSuit);
                        AddCardToDeck(cardSkip, 2);//two of each
                        break;
                } 
            }
        }

        /// <summary>
        /// takes the supplied suit and generates all the number cards for that suit
        /// </summary>
        /// <param name="pSuit">enum for the suit</param>
        /// <param name="pColour">string mathing the enum</param>
        protected virtual void GenerateNumberCards(Suit pSuit, string pColour)
        {
            for (int number = 0; number <= 9; number++)
            {
                string imgFileName = "card_front_suit_" + pColour + "_" + number.ToString();
                CardNumber cardNumber = new CardNumber(imgFileName, pSuit, number);
                if (number == 0)
                {   //one of each
                    AddCardToDeck(cardNumber,1);
                }
                else
                {   //two of each
                    AddCardToDeck(cardNumber,2);
                }
            }
        }

        /// <summary>
        /// Takes the supplied card, gives it a number and adds it to the deck however
        /// many times the pNumToAdd parameter states. 
        /// 
        /// About The Identifiers: 
        /// The Cards are all supplied in an order. First of all All the red numbers, then the red specials,
        /// followed by the same for the other suites, then the wild cards come in last. This allows for increment
        /// based identifiers to be generated.
        /// 
        /// Because these are references the result is that each unique
        /// type of card gets the same number. eg. Red-0 is different from any other card in the deck.
        /// But Red-1 is the same as Red-1 even though there are two of them in the deck. 
        /// In the same way, each of the wild cards type has one identifier. These identifiers are
        /// used during hand sorting. They are also used after a player has picked up when deciding if a card can be played.
        /// </summary>
        /// <param name="pCard">Card object supplied</param>
        /// <param name="pNumToAdd">number of times this card should be added.</param>
        protected virtual void AddCardToDeck(Card pCard, int pNumToAdd)
        {
            for (int count = 0; count < pNumToAdd; count++)
            {
                pCard.UniqueIdentifier = mUniqueIdentifier;
                mDiscardPile.Add(pCard);
                mUniqueIdentifier++;
            }
        }

        /// <summary>
        /// saves the newly generated list of cards so it can just be loaded next time. 
        /// </summary>
        /// <param name="pCardList">list of all cards in the deck</param>
        protected virtual void SaveFullCardDeck(List<Card> pCardList)
        {
            try
            {
                using (Stream stream = File.Open(mCardDeckFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, pCardList);
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error saving the setup files, is the drive writable?", "Save deck error");
            }
        }
    }
}
