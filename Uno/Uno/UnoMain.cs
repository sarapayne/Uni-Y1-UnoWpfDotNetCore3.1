using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Resources;
using System.Reflection;
using Uno;

namespace Uno
{
    class UnoMain : System.Windows.Application
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent()
        {

#line 5 "..\..\..\App.xaml"
            this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

#line default
#line hidden
        }

        static UnoGame unoGame; //central point to be saved and restored to. Will allow easy imaging of any game state. 
        static List<Card> cardDeck;
        static string dictionaryFileName = "Dictionary.Bin";

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public static void Main()
        {
            cardDeck = new List<Card>();
            LoadCardDeck();
            Uno.App app = new Uno.App();
            app.InitializeComponent();
            app.Run();
        }

        static void LoadCardDeck()
        {
            try
            {
                using (Stream stream = File.Open (dictionaryFileName, FileMode.Open  ))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    cardDeck = (List<Card>)bin.Deserialize(stream);
                }
            }
            catch(IOException)
            {
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "Dictionary load error");
                GenerateCardDictionary();
            }
        }

        static void GenerateCardDictionary()
        {
            cardDeck = new List<Card>();
            List<Suit> suites = new List<Suit> {Suit.Red, Suit.Green, Suit.Blue, Suit.Yellow};
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
            Image imageStandard = GetResourceImage("card_front_wild_standard");
            Image imagePickup = GetResourceImage("card_front_wild_pickup");
            CardWild cardWildStandard = new CardWild(imageStandard, 0);
            CardWild cardWildPickup = new CardWild(imagePickup, 4);
            Add2OfEachCardToDeck(cardWildStandard);
            Add2OfEachCardToDeck(cardWildPickup);
        }

        static void Add2OfEachCardToDeck(Card card)
        {
            cardDeck.Add(card);
            cardDeck.Add(card);
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
                Image image = GetResourceImage(imgFileName);
                CardSpecial cardSpecial = new CardSpecial(image, pSuit, specialType);
                Add2OfEachCardToDeck(cardSpecial);
            }
        }

        static void GenerateNumberCards(Suit pSuit, string pColour)
        {
            for (int number = 0; number <= 9; number++)
            {
                string imgFileName = "card_front_suit_" + pColour + "_" + number.ToString();
                Image image = GetResourceImage(imgFileName);
                CardNumber cardNumber = new CardNumber(image, pSuit, number);
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

        static Image GetResourceImage (string pName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetName().Name + ".Properties.Resources";
            ResourceManager rm = new ResourceManager(resourceName, assembly);
            return (Image)rm.GetObject(pName);
        }
        
        static void SaveCardDeck()
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
