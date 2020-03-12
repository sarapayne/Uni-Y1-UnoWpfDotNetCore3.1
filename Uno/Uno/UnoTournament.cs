using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno
{
    class UnoTournament
    {
        private List<Player> mPlayers;
        private int mWinThreshold = 500;

        public UnoTournament()
        {
            mPlayers = new List<Player>();
            EventPublisher.RaiseAddToTournament += UnoTournament_RaiseAddToTournament;
        }

        public List<Player> UnoGames
        {
            get { return mPlayers; }
        }

        private void UnoTournament_RaiseAddToTournament(object sender, EventArgsAddToTournament eventArgsAddTo)
        {
            AddGame(eventArgsAddTo.WinningPlayer);
        }

        public void AddGame(Player pPlayer)
        {
            if (mPlayers.Count == 0)
            {
                mPlayers.Add(pPlayer);
            }
            else
            {
                bool playerFound = false;
                foreach (Player player in mPlayers)
                {
                    if (pPlayer.Name == player.Name)
                    {
                        player.FinalScore += player.FinalScore;
                        playerFound = true;
                        break;
                    }
                }
                if (!playerFound)
                {
                    mPlayers.Add(pPlayer);
                }
            }
            CheckForWinner(pPlayer);
        }

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
