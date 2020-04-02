using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;
using Uno.Players;
using System.Linq; 

namespace Uno
{
    [Serializable]
    class UnoTournament
    {
        private List<Player> mPlayers;
        private int mWinThreshold = 500;

        public UnoTournament()
        {
            mPlayers = new List<Player>();
            SubscribeEvents();
        }

        public List<Player> UnoGames
        {
            get { return mPlayers; }
        }

        /// <summary>
        /// used as an method so subscribers can be initialised after loading from file.
        /// </summary>
        public void SubscribeEvents()
        {
            EventPublisher.RaiseAddToTournament += UnoTournament_RaiseAddToTournament;
            EventPublisher.RaiseUnsubscribeTournamentEvents += UnoTournament_UnsubscribeTournamentEvents;
        }

        /// <summary>
        /// Used to ensure no duplicate events are triggered before garbage collection after loading or creating new instances
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgs">always null</param>
        private void UnoTournament_UnsubscribeTournamentEvents(object sender, EventArgs eventArgs)
        {
            EventPublisher.RaiseAddToTournament -= UnoTournament_RaiseAddToTournament;
            EventPublisher.RaiseUnsubscribeTournamentEvents -= UnoTournament_UnsubscribeTournamentEvents;
        }

        /// <summary>
        /// processes the event then sends it to the Add Game method.
        /// </summary>
        /// <param name="sender">always null</param>
        /// <param name="eventArgsAddTo">name and score</param>
        private void UnoTournament_RaiseAddToTournament(object sender, EventArgsAddToTournament eventArgsAddTo)
        {
            AddGame(eventArgsAddTo.WinningPlayer);
        }

        /// <summary>
        /// Checks for matching player entries, if found adds the new score to the last
        /// If not found adds a new player to the contestents. 
        /// Reports score or winner
        /// </summary>
        /// <param name="pPlayer">player object containing all player details</param>
        public void AddGame(Player pPlayer)
        {
            if(mPlayers.Exists(player => player.Name == pPlayer.Name))
            {
                pPlayer.FinalScore += pPlayer.FinalScore;
            }
            else
            {
                mPlayers.Add(pPlayer);
            }
            CheckForWinner(pPlayer);
        }

        /// <summary>
        /// checks to see if the new score takes the player over the winning threshold,
        /// if it does anounces it, else tells the user the points were added. 
        /// </summary>
        /// <param name="pPlayer">player object with all details about the player.</param>
        private void CheckForWinner(Player pPlayer)
        {
            if (pPlayer.FinalScore >= 500)
            {
                MessageBox.Show(pPlayer.Name + " has won the tournament", "tournament won");
                
            }
            else
            {
                MessageBox.Show(pPlayer.Name + "'s points have been added to the tournament", "points added");
            }
            EventPublisher.MainMenu();
        }
            
    }
}
