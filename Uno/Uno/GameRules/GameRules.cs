using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
using Uno.GUI_Custom_Elements;
using Uno.View;

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
            EventPublisher.RaiseNextPlayerButtonClick += GameRules_RaiseNextPlayerButtonClick;
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
                    switch (card)
                    {
                        case CardWild wildcard:
                            if (wildcard.CardsToDraw > 0)
                            {
                                UnoMain.UnoGame.NextPlayerPickup += wildcard.CardsToDraw;
                                UnoMain.UnoGame.NextPlayersSkip++;
                            }
                            break;
                        case CardSpecial specialCard:
                            if (specialCard.Type == SpecialType.Draw)
                            {
                                UnoMain.UnoGame.NextPlayerPickup += 2;
                                UnoMain.UnoGame.NextPlayersSkip++;
                            }
                            else if (specialCard.Type == SpecialType.Skip)
                            {
                                UnoMain.UnoGame.NextPlayersSkip++;
                            }
                            else if (specialCard.Type == SpecialType.Reverse)
                            {
                                UnoMain.UnoGame.ReverseDirection();
                            }
                            break;
                    }
                    UnoMain.UnoGame.PlaceCard(card);
                }
            }
            else
            {
                MessageBox.Show("You can not discard more cards this turn, please click next player or draw a card.", "discard error");
            }
        }

        private void GameRules_RaiseNextPlayerButtonClick (object sender, EventArgs eventArgs)
        {
            UnoMain.UnoGame.NextPlayer();
        }

        private bool CheckIfPlayerAllowedToUseCard()
        {
            bool isAllowed = false;
            //add code here
            return isAllowed;
        }

        private bool CheckIfCardCanBePlayed(Card pCard)
        {
            bool canBePlayed = false;
            Card lastDiscardCard = UnoMain.UnoGame.Deck.DiscardPile[UnoMain.UnoGame.Deck.DiscardPile.Count - 1];
            switch (lastDiscardCard)
            {
                case CardWild discardedWild:
                    Suit nextSuit = discardedWild.NextSuit;
                    switch (pCard)
                    {
                        case CardSuit cardSuit:
                            if (cardSuit.Csuit == nextSuit) canBePlayed = true;
                            break;
                        case CardWild cardWild:
                            canBePlayed = true;
                            break;
                    }
                    break;
                case CardSuit discardedSuitCard:
                    Suit lastSuit = discardedSuitCard.Csuit;
                    switch (pCard)
                    {
                        case CardWild currentWildCard:
                            canBePlayed = true;
                            break;
                        case CardSuit currentSuitCard:
                            if (currentSuitCard.Csuit == lastSuit) canBePlayed = true;
                            else 
                            {
                                switch (discardedSuitCard)
                                {
                                    case CardSpecial discardedSpecialCard:
                                        CardSpecial currentSpecialCard = null;
                                        currentSpecialCard = pCard as CardSpecial;
                                        if (discardedSpecialCard.Type == currentSpecialCard.Type) canBePlayed = true;
                                        break;
                                    case CardNumber discardedNumberCard:
                                        CardNumber currentNumberCard = null;
                                        currentNumberCard = pCard as CardNumber;
                                        if (discardedNumberCard.Number == currentNumberCard.Number) canBePlayed = true;
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
