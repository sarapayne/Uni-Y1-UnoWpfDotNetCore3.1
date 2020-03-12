using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Uno
{
    class UnoTournament
    {
        private List<Player> mPlayers;
        private int mWinThreshold = 500;

        public UnoTournament()
        {
            mPlayers = new List<Player>();
        }

        public List<Player> UnoGames
        {
            get { return mPlayers; }
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
                if (playerFound)
                {
                    CheckForWinner(pPlayer);
                }
                else
                {
                    mPlayers.Add(pPlayer);
                }
            }
        }

        private void CheckForWinner(Player player)
        {
            if (player.FinalScore >= 500)
            {
                MessageBox.Show(player.Name + " has won the tournament", "tournament won");
                EventPublisher.MainMenu();
            }
        }
            
    }
}
