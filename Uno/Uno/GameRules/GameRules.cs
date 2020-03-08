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
            EventPublisher.RaiseNextPlayerButtonClick += GameRules_RaiseNextPlayerButtonClick;
        }

        private void GameRules_RaiseGameButtonClick(object sender, EventArgs eventArgs)
        {
            EventArgsGameButtonClick ev = eventArgs as EventArgsGameButtonClick;
            Card card = ev.mPlayingCard;
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
                        }
                        break;
                    case CardSpecial specialCard:
                        if (specialCard.Type == SpecialType.Draw)
                        {
                            UnoMain.UnoGame.NextPlayerPickup += 2;
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

        private void GameRules_RaiseNextPlayerButtonClick (object sender, EventArgs eventArgs)
        {
            if (UnoMain.UnoGame.PlayerHasPickedUpOrDiscarded)
            {
                UnoMain.UnoGame.NextPlayer();
            }
            else MessageBox.Show("Sorry, you need to either draw a card or play a card to pass play to the next player", "next player error");
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
                            if (cardWild.CardsToDraw == 0) canBePlayed = true;
                            else if (!CheckForOtherPlayableCards()) canBePlayed = true; //only happens if no other playable cards
                            break;
                    }
                    break;
                case CardSuit discardedSuitCard:
                    Suit lastSuit = discardedSuitCard.Csuit;
                    switch (pCard)
                    {
                        case CardWild currentWildCard:
                            if (currentWildCard.CardsToDraw == 0) canBePlayed = true;
                            else if (!CheckForOtherPlayableCards()) canBePlayed = true; //only happens if no other playable cards
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

        private bool CheckForOtherPlayableCards()
        {
            bool hasOtherPlayableCards = false;
            List<Card> currentPlayerCards = UnoMain.UnoGame.Players[UnoMain.UnoGame.CurrentPlayer].Cards;
            foreach (Card playerCard in currentPlayerCards)
            {
                switch (playerCard)
                {
                    case CardWild wildCard:
                        if (wildCard.CardsToDraw == 0) hasOtherPlayableCards = true;
                        break;
                    case CardSuit suitcard:
                        if (CheckIfCardCanBePlayed(suitcard)) hasOtherPlayableCards = true;
                        break;
                }
            }
            return hasOtherPlayableCards;
        }
    }
}
