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
        
        static UnoGame mUnoGame;

        public static UnoGame UnoGame
        {
            get { return mUnoGame; }
        }

        public static void NewGame(List<string> pPlayerNames, int pDealer)
        {
            StartNewGuiInteface();
            mUnoGame = new UnoGame(pPlayerNames, pDealer);
            mUnoGame.RefreshCardPiles();
            EventPublisher.NextPlayerButtonClick();//not actually clicked but does the same thing
        }

        private static void StartNewGuiInteface()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
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
    }
}
