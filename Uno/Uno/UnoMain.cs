using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Uno.View;
using System.Xml;
using Uno.EventsComponents;
using Uno.Game;

namespace Uno
{
    [Serializable()]
    enum RulesType
    {
        Standard, House1, House2, House3
    }

    [Serializable()]
    class UnoMain
    {
        private UnoGame mUnoGame;
        private UnoTournament mUnoTournament;

        public UnoMain()
        {
            SubscribeToEvents();
            mUnoGame = new UnoGame();
            mUnoTournament = new UnoTournament();        
            StartNewGuiInteface();
            EventPublisher.MainMenu();
        }

        public UnoGame UnoGame
        {
            get { return mUnoGame; }
        }

        /// <summary>
        /// Subscribes to all events needed by this class.
        /// </summary>
        private void SubscribeToEvents()
        {
            EventPublisher.RaiseNewGame += UnoMain_RaiseNewGame;
            EventPublisher.RaiseLoadGame += UnoMain_LoadGame;
            EventPublisher.RaiseSaveGame += UnoMain_SaveGame;
            EventPublisher.RaiseCheckForActiveGame += UnoMain_CheckForActiveGame;
            EventPublisher.RaiseNewTournament += UnoMain_NewTournament;
            EventPublisher.RaiseLoadTournament += UnoMain_LoadTournament;
            EventPublisher.RaiseSaveTournament += UnoMain_SaveTournament;
        }

        /// <summary>
        /// Takes new game info from the GUI and passes it to the NewGame method
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">Players Names, Dealer and Rules type information</param>
        private void UnoMain_RaiseNewGame (object sender, EventArgsGame eventArgs)
        {
            EventPublisher.UnsubscribeEvents();
            NewGame(eventArgs.Players, eventArgs.Dealer, eventArgs.GameRulesType, eventArgs.NumOfSwapHands);
        }

        /// <summary>
        /// Launches a new game based on the parameters
        /// </summary>
        /// <param name="pPlayerNames">List of player names</param>
        /// <param name="pDealer">player number of the dealer</param>
        /// <param name="rulesType">enum defining the game rules</param>
        private void NewGame(List<string> pPlayerNames, int pDealer, RulesType rulesType, int pNumOfSwapHands)
        {
            switch (rulesType)
            {
                case RulesType.Standard:
                    mUnoGame = new UnoGame(pPlayerNames, pDealer, pNumOfSwapHands);
                    break;
                case RulesType.House1:
                    mUnoGame = new UnoGameHouse1(pPlayerNames, pDealer, pNumOfSwapHands);
                    break;
                case RulesType.House2:
                    mUnoGame = new UnoGameHouse2(pPlayerNames, pDealer, pNumOfSwapHands);
                    break;
                case RulesType.House3:
                    mUnoGame = new UnoGameHouse3(pPlayerNames, pDealer, pNumOfSwapHands);
                    break;
            } 
        }

        /// <summary>
        /// Checks if an active game exists before raising the event to return to the game.
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
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

        /// <summary>
        /// Checks for active game returning a convient bool
        /// </summary>
        /// <returns>true/false</returns>
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

        /// <summary>
        /// Loads the three main GUI windows.
        /// </summary>
        private void StartNewGuiInteface()
        {
            WpfWindowGame wpfWindowGame = new WpfWindowGame();
            WpfWindowFinalScore wpfWindowFinalScore = new WpfWindowFinalScore();
            WpfWindowMainMenu wpfWindowMainMenu = new WpfWindowMainMenu();
            WpfChooseSwapPlayer wpfChooseSwapPlayer = new WpfChooseSwapPlayer();
            WpfWindowConsequences wpfWindowConsequences = new WpfWindowConsequences();
        }

        /// <summary>
        /// Takes instruction from the Load game event and passes to the load game method. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsLoadSave">name of file from file pick dialog</param>
        private void UnoMain_LoadGame(object sender, EventArgsLoadSave eventArgsLoadSave)
        {
            LoadGame(eventArgsLoadSave.Name);
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// Performs the actual loading of game files
        /// </summary>
        /// <param name="pFileToLoad">file name</param>
        public void LoadGame(string pFileToLoad)
        {
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
                    EventPublisher.UnsubscribeEvents();
                    mUnoGame.SubscribeToEvents();
                    MessageBox.Show("Your game has been restored", "game restored");
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error this file can not be loaded, please choose another or start a new game", "game load error");
            }
        }

        /// <summary>
        /// takes instrcution from the save game event and passes it to the save game method
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsLoadSave">file name from dialog</param>
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

        /// <summary>
        /// performas the actual saving of game files
        /// </summary>
        /// <param name="pUnogame">always null</param>
        /// <param name="pName">file name</param>
        public static void SaveGame(UnoGame pUnogame, String pName)
        {
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

        /// <summary>
        /// handles the new tournament event and passes it to the method
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        private void UnoMain_NewTournament(object sender, EventArgs eventArgs)
        {
            NewTournament();
        }

        /// <summary>
        /// handles the load tournament event and passes it to the load tournament method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgsLoadSave"></param>
        private void UnoMain_LoadTournament(object sender, EventArgsLoadSave eventArgsLoadSave)
        {
            LoadTournament(eventArgsLoadSave.Name);
        }

        /// <summary>
        /// handles the save tournament event and passes it to the method. 
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsLoadSave">file name</param>
        private void UnoMain_SaveTournament(object sender, EventArgsLoadSave eventArgsLoadSave)
        {
            if (mUnoTournament != null)
            {
                SaveTournament(mUnoTournament, eventArgsLoadSave.Name);
            }
            else
            {
                MessageBox.Show("Sorry there is no active tournament to save, aborted", "no active tournament error");
                EventPublisher.MainMenu();
            }
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// performs the actual saving of tournament files
        /// </summary>
        /// <param name="pTournament">always null</param>
        /// <param name="pFileName">file name</param>
        private void SaveTournament(UnoTournament pTournament, string pFileName)
        {
            try
            {
                using (Stream stream = File.Open(pFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, pTournament);
                    MessageBox.Show("Your tournament has been saved", "save successful");
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error saving your tournament, unable to save for retrieval", "save file error");
            }
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// performs the actual loading of tournament files
        /// </summary>
        /// <param name="pFileName"></param>
        private void LoadTournament(string pFileName)
        {
            
            try
            {
                using (Stream stream = File.Open(pFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    if (mUnoGame == null)
                    {
                        mUnoTournament = new UnoTournament();
                    }
                    mUnoTournament = (UnoTournament)bin.Deserialize(stream);
                    EventPublisher.UnsubscribeTournamentEvents();
                    mUnoTournament.SubscribeEvents();
                    MessageBox.Show("Your tournament has been restored", "tournament restored");
                }
            }
            catch
            {
                MessageBox.Show("Sorry there was an error this file can not be loaded, please choose another or start a new tournament", "tournament load error");
            }
            EventPublisher.MainMenu();
        }

        /// <summary>
        /// begins a new tournament
        /// </summary>
        private void NewTournament()
        {
            EventPublisher.UnsubscribeTournamentEvents();
            mUnoTournament = new UnoTournament();
            EventPublisher.MainMenu();
        }
    }
}
