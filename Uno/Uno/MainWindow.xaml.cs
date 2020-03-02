using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Uno
{
    [Serializable()]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static UnoGame unoGame; //central point to be saved and restored to. Will allow easy imaging of any game state. 
        private List<Card> cardDeck;
        static string dictionaryFileName = "CardDeck.Bin";

        public MainWindow()
        {
            InitializeComponent();
            cardDeck = new List<Card>();
            LoadCardDeck();
        }

        private void LoadCardDeck()
        {
            try
            {
                using (Stream stream = File.Open(dictionaryFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    cardDeck = (List<Card>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "Dictionary load error");
                GenerateCardList();
            }
        }

        private void GenerateCardList()
        {
            cardDeck = new List<Card>();
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

        private void GenerateWildCards()
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

        private void Add2OfEachCardToDeck(Card card)
        {
            cardDeck.Add(card);
            cardDeck.Add(card);
        }

        private void GenerateSuitCards(Suit pSuit, string pColour)
        {
            GenerateNumberCards(pSuit, pColour);
            GenerateSpecialCards(pSuit, pColour);
        }

        private void GenerateSpecialCards(Suit pSuit, string pColour)
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

        private void GenerateNumberCards(Suit pSuit, string pColour)
        {
            for (int number = 0; number <= 9; number++)
            {
                string imgFileName = "card_front_suit_" + pColour + "_" + number.ToString();
                CardNumber cardNumber = new CardNumber(imgFileName, pSuit, number);
                if (number == 0)
                {
                    cardDeck.Add(cardNumber);
                }
                else
                {
                    Add2OfEachCardToDeck(cardNumber);
                }
            }
        }

        /*  -- Removed ready to rework inside gui partial class. 
        static Image GetResourceImage (string pName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetName().Name + ".Properties.Resources";
            ResourceManager rm = new ResourceManager(resourceName, assembly);
            return (Image)rm.GetObject(pName);
        }
        */

        private void SaveCardDeck()
        {
            try
            {
                using (Stream stream = File.Open(dictionaryFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, cardDeck);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Sorry there was an error saving the setup files, is the drive writable?", "Save dictionary error");
            }
        }
    }
}
