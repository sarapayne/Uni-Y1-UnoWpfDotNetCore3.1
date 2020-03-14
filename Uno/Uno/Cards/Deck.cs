using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Image = System.Windows.Controls.Image;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Uno
{
    [Serializable]
    class Deck
    {
        protected string mCardDeckFileName = "CardDeck.bin";
        protected List<Card> mDrawPile;
        protected List<Card> mDiscardPile;
        protected int mUniqueIdentifier;

        public Deck()
        {
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

        protected virtual void MakeSureTopCardNotWild(List<Card> pCardList)
        {
            while (pCardList[0] is CardWild)
            {
                Card temp = pCardList[0];
                pCardList.RemoveAt(0);
                pCardList.Add(temp);
            }
        }
                

        protected virtual void LoadFullCardDeck()
        {
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
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "Dictionary load error");
                mDiscardPile = new List<Card>();
                GenerateCardList();
            }
        }

        protected virtual void GenerateCardList()
        {
            mDiscardPile = new List<Card>();
            List<Suit> suites = new List<Suit> { Suit.Red, Suit.Green, Suit.Blue, Suit.Yellow };
            foreach (Suit suit in suites)
            {
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
            }
            GenerateWildCards();
            SaveFullCardDeck(mDiscardPile);
        }

        protected virtual void GenerateWildCards()
        {
            string imageStandardName = "card_front_wild_standard";
            string imagePickupName = "card_front_wild_pickup";
            CardWild cardWildStandard = new CardWild(imageStandardName, 0);
            CardWild cardWildPickup = new CardWild(imagePickupName, 4);
            AddCardToDeck(cardWildStandard, 4);
            AddCardToDeck(cardWildPickup, 4);
        }



        protected virtual void GenerateSuitCards(Suit pSuit, string pColour)
        {
            GenerateNumberCards(pSuit, pColour);
            GenerateSpecialCards(pSuit, pColour);
        }

        protected virtual void GenerateSpecialCards(Suit pSuit, string pColour)
        {
            List<SpecialType> specialTypes = new List<SpecialType> { SpecialType.Draw, SpecialType.Reverse, SpecialType.Skip };
            foreach (SpecialType specialType in specialTypes)
            {
                string tail = "";
                switch (specialType)
                {
                    case SpecialType.Draw:
                        tail = "draw";
                        break;
                    case SpecialType.Reverse:
                        tail = "reverse";
                        break;
                    case SpecialType.Skip:
                        tail = "skip";
                        break;
                }
                string imgFileName = "card_front_suit_" + pColour + "_" + tail;
                CardSpecial cardSpecial = new CardSpecial(imgFileName, pSuit, specialType);
                AddCardToDeck(cardSpecial, 2);
            }
        }

        protected virtual void GenerateNumberCards(Suit pSuit, string pColour)
        {
            for (int number = 0; number <= 9; number++)
            {
                string imgFileName = "card_front_suit_" + pColour + "_" + number.ToString();
                CardNumber cardNumber = new CardNumber(imgFileName, pSuit, number);
                if (number == 0)
                {
                    AddCardToDeck(cardNumber,1);
                }
                else
                {
                    AddCardToDeck(cardNumber,2);
                }
            }
        }

        protected virtual void AddCardToDeck(Card pCard, int pNumToAdd)
        {
            for (int count = 0; count < pNumToAdd; count++)
            {
                pCard.UniqueIdentifier = mUniqueIdentifier;
                mDiscardPile.Add(pCard);
                mUniqueIdentifier++;
            }
        }

        protected virtual void AddNewCardToDeck(Card pcard)
        {   //ever time this is called increment then return the number before it was incremented. 
            pcard.UniqueIdentifier = mUniqueIdentifier;
            mDiscardPile.Add(pcard);
            mUniqueIdentifier++;
        }

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
