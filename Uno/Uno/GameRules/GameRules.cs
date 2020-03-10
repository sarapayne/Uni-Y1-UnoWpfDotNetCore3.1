using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using Uno.GUI_Custom_Elements;

namespace Uno
{
    [Serializable()]
    enum GameRulesType
    {
        Standard, House1, House2, House3
    }

    [Serializable()]
    class GameRules
    {
        public GameRules ()
        {         
            EventPublisher.RaiseGameButtonClick += GameRules_RaiseGameButtonClick;    
        }

        private void GameRules_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
            if (!UnoMain.UnoGame.PlayerHasDiscared)
            {   //only come here if play is allowed for this player. 
                bool cardPlayable = CheckIfCardCanBePlayed(card);
                if (!cardPlayable) MessageBox.Show("Sorry but this card can not be played", "Card not playable");
                else
                {
                    EventPublisher.PlayCard(card);
                }
            }
            else
            {
                MessageBox.Show("You can not discard more cards this turn, please click next player or draw a card.", "discard error");
            }
        }

        public bool CheckIfCardCanBePlayed(Card pCard)
        {
            bool canBePlayed = false;
            Card discardPile = UnoMain.UnoGame.Deck.DiscardPile[UnoMain.UnoGame.Deck.DiscardPile.Count - 1];
            switch (discardPile)
            {
                case CardWild discardPileWild:
                    switch (pCard)
                    {
                        case CardSuit playedSuitCard:
                            if (playedSuitCard.Csuit == discardPileWild.NextSuit) canBePlayed = true;
                            break;
                        case CardWild cardWild:
                            canBePlayed = true;
                            break;
                    }
                    break;
                case CardSuit discardPileSuit:
                    switch (pCard)
                    {
                        case CardWild playedWildCard:
                            canBePlayed = true;
                            break;
                        case CardSuit playedSuitCard:
                            if (discardPileSuit.Csuit == playedSuitCard.Csuit) canBePlayed = true;
                            else 
                            {   //come here if both cards are suits do they do not match
                                switch(discardPileSuit)
                                {
                                    case CardNumber discardPileNumberCard:
                                        switch(playedSuitCard)
                                        {
                                            case CardNumber playedNumberCard:
                                                if (discardPileNumberCard.Number == playedNumberCard.Number)
                                                {
                                                    canBePlayed = true;
                                                }
                                                break;
                                            case CardSpecial playedSpecialCard:
                                                canBePlayed = false;
                                                break;
                                        }
                                        break;
                                    case CardSpecial discardPileSpecialCard:
                                        switch (playedSuitCard)
                                        {
                                            case CardNumber playedNumberCard:
                                                canBePlayed = false;
                                                break;
                                            case CardSpecial playedSpecialCard:
                                                if (discardPileSpecialCard.Type == playedSpecialCard.Type)
                                                {
                                                    canBePlayed = true;
                                                }
                                                else
                                                {
                                                    canBePlayed = false;
                                                }
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return canBePlayed;
        }
    }
}
