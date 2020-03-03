using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Uno
{
    [Serializable()]
    class UnoMain
    {
        static string mCardDeckFileName = "CardDeck.bin";
        static UnoGame mUnoGame;
        [NonSerialized]
        static List<Card> mCardDeck;

        public static List<Card> CardDeck
        {
            get { return mCardDeck; }
        }

        public static void LoadCardDeck()
        {
            try
            {
                using (Stream stream = File.Open(mCardDeckFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    mCardDeck = (List<Card>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "Dictionary load error");
                GenerateCardList();
            }
        }

        public static void LoadGame(string pFileToLoad)
        {
            string fileToLoad = pFileToLoad + ".game";
            try
            {
                using (Stream stream = File.Open(pFileToLoad, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    mUnoGame = (UnoGame)bin.Deserialize(stream);
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error this file can not be loaded, please choose another or start a new game", "game load error");
            }
        }

        public static void NewGame(List<string> pPlayerNames, int pDealer, GameRulesType pGameRulesType)
        {
            mUnoGame = new UnoGame(pPlayerNames, pDealer, pGameRulesType);
        }

        static void GenerateCardList()
        {
            mCardDeck = new List<Card>();
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
            SaveCardDeck();
        }

        static void GenerateWildCards()
        {
            string imageStandard = "card_front_wild_standard";
            string imagePickup = "card_front_wild_pickup";
            CardWild cardWildStandard = new CardWild(imageStandard, 0);
            CardWild cardWildPickup = new CardWild(imagePickup, 4);
            Add2OfEachCardToDeck(cardWildStandard);
            Add2OfEachCardToDeck(cardWildStandard);
            Add2OfEachCardToDeck(cardWildPickup);
            Add2OfEachCardToDeck(cardWildPickup);
        }

        static void Add2OfEachCardToDeck(Card card)
        {
            mCardDeck.Add(card);
            mCardDeck.Add(card);
        }

        static void GenerateSuitCards(Suit pSuit, string pColour)
        {
            GenerateNumberCards(pSuit, pColour);
            GenerateSpecialCards(pSuit, pColour);
        }

        static void GenerateSpecialCards(Suit pSuit, string pColour)
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
                Add2OfEachCardToDeck(cardSpecial);
            }
        }

        static void GenerateNumberCards(Suit pSuit, string pColour)
        {
            for (int number = 0; number <= 9; number++)
            {
                string imgFileName = "card_front_suit_" + pColour + "_" + number.ToString();
                CardNumber cardNumber = new CardNumber(imgFileName, pSuit, number);
                if (number == 0)
                {
                    mCardDeck.Add(cardNumber);
                }
                else
                {
                    Add2OfEachCardToDeck(cardNumber);
                }
            }
        }

        public static void SaveGame(string pFileToSave)
        {
            string fileToSave = pFileToSave + ".game";
            try
            {
                using (Stream stream = File.Open(pFileToSave, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, mUnoGame);
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error saving your game, unable to save for retrieval", "save game file error");
            }
        }

        static void SaveCardDeck()
        {
            try
            {
                using (Stream stream = File.Open(mCardDeckFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, mCardDeck);
                }
            }
            catch 
            {
                MessageBox.Show("Sorry there was an error saving the setup files, is the drive writable?", "Save deck error");
            }
        }

    }
}
