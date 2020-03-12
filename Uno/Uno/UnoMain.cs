using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Uno.View;
using System.Xml;
using Uno.EventsComponents;

namespace Uno
{
    [Serializable()]
    class UnoMain
    {
        private UnoGame mUnoGame;
        private UnoTournament mUnoTournament;

        public UnoMain()
        {
            EventPublisher.RaiseNewGame += UnoMain_RaiseNewGame;
            EventPublisher.RaiseLoadGame += UnoMain_LoadGame;
            EventPublisher.RaiseSaveGame += UnoMain_SaveGame;
            EventPublisher.RaiseCheckForActiveGame += UnoMain_CheckForActiveGame;
            mUnoGame = new UnoGame();
            mUnoTournament = new UnoTournament();        
            StartNewGuiInteface();
            EventPublisher.MainMenu();
        }

        public UnoGame UnoGame
        {
            get { return mUnoGame; }
        }

        private void UnoMain_RaiseNewGame (object sender, EventArgsGame eventArgs)
        {
            EventPublisher.UnsubscribeEvents();
            NewGame(eventArgs.Players, eventArgs.Dealer);
        }

        private void NewGame(List<string> pPlayerNames, int pDealer)
        {
            EventPublisher.NextPlayerButtonClick();//not actually clicked but does the same thing
            mUnoGame = new UnoGame(pPlayerNames, pDealer);
        }

        private void UnoMain_CheckForActiveGame(object sender, EventArgs eventArgs)
        {   //called from return to active game. 
            if (ActiveGameExists())
            {
                EventPublisher.ReturnToGame();
            }
            else
            {
                MessageBox.Show("Sorry there is no active game to return to", "no active game");
                EventPublisher.MainMenu();
            }
        }

        private bool ActiveGameExists()
        {
            if (mUnoGame != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void StartNewGuiInteface()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
            WpfWindowFinalScore wpfWindowFinalScore = new WpfWindowFinalScore();
            WpfWindowMainMenu wpfWindowMainMenu = new WpfWindowMainMenu();
        }

        private void UnoMain_LoadGame(object sender, EventArgsLoadSave eventArgsLoadSave)
        {
            EventPublisher.UnsubscribeEvents();
            LoadGame(eventArgsLoadSave.Name);
            EventPublisher.MainMenu();
        }

        public void LoadGame(string pFileToLoad)
        {
            //string fileToLoad = pFileToLoad + ".unogame";
            try
            {
                using (Stream stream = File.Open(pFileToLoad, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    if (mUnoGame == null)
                    {
                        mUnoGame = new UnoGame();
                    }
                    mUnoGame = (UnoGame)bin.Deserialize(stream);
                    mUnoGame.SubscribeToEvents();
                    MessageBox.Show("Your game has been restored", "game restored");
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error this file can not be loaded, please choose another or start a new game", "game load error");
            }
        }

        private void UnoMain_SaveGame(object sender, EventArgsLoadSave eventArgsLoadSave)
        {
            if (mUnoGame != null)
            {
                SaveGame(mUnoGame, eventArgsLoadSave.Name);
            }
            else
            {
                MessageBox.Show("Sorry there is no active game to save, aborted", "no active game error");
                EventPublisher.MainMenu();
            }
            EventPublisher.MainMenu();
        }

        public static void SaveGame(UnoGame pUnogame, String pName)
        {
            //string fileName=  pName + ".unogame";
            try
            {
                using (Stream stream = File.Open(pName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, pUnogame);
                    MessageBox.Show("Your game has been saved", "save successful");
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error saving your game, unable to save for retrieval", "save game file error");
            }
        }
    }
}
