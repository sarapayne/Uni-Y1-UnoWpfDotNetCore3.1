using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Uno.EventsComponents;

namespace Uno
{
    static class EventPublisher
    {
        public static event EventHandler<EventArgsGameButtonClick> RaiseGameButtonClick; 
        public static event EventHandler<EventArgsColourPick> RaiseColourPick;
        public static event EventHandler<EventArgsPlayCard> RaisePlayCard;
        public static event EventHandler<EventArgsGuiUpdate> RaiseGuiUpdate;
        public static event EventHandler RaiseNextPlayerButtonClick;
        public static event EventHandler RaisePlus4Challenge;
        public static event EventHandler RaiseDrawTwoCards;
        public static event EventHandler RaiseSkipGo;
        public static event EventHandler RaiseReverseDirection;
        public static event EventHandler RaiseAcceptDraw4;
        public static event EventHandler RaiseDrawCard;
        public static event EventHandler RaiseRefreshCardPiles;

        public static void RefreshCardPiles()
        {
            if (RaiseRefreshCardPiles != null)
            {
                EventPublisher.RaiseRefreshCardPiles(null, null);
            }
        }

        public static void DrawCard()
        {
            if (RaiseDrawCard != null)
            {
                EventPublisher.RaiseDrawCard(null, null);
            }
        }

        public static void AcceptDraw4()
        {
            if (RaiseAcceptDraw4 != null )
            {
                EventPublisher.RaiseAcceptDraw4(null, null);
            }
        }

        public static void GuiUpdate(Player pPlayer, Deck pDeck, string pExtras)
        {
            if (RaiseGuiUpdate != null)
            {
                EventPublisher.RaiseGuiUpdate(null, new EventArgsGuiUpdate(pPlayer, pDeck, pExtras));
            }
        }

        public static void PlayCard(Card pcard)
        {
            if (RaisePlayCard != null)
            {
                EventPublisher.RaisePlayCard(null, new EventArgsPlayCard(pcard));
            }
        }

        public static void GameButtonClick(Card pCard)
        {
            if (RaiseGameButtonClick != null)
            {
                EventPublisher.RaiseGameButtonClick(null, new EventArgsGameButtonClick(pCard));
            }
        }

        public static void ReverseDirection()
        {
            if (RaiseReverseDirection != null)
            {
                EventPublisher.RaiseReverseDirection(null, null);
            }
        }

        public static void SkipGo()
        {
            if (RaiseSkipGo != null)
            {
                EventPublisher.RaiseSkipGo(null, null);
            }
        }

        public static void DrawTwoCards()
        {
            if (RaiseDrawTwoCards != null)
            {
                EventPublisher.RaiseDrawTwoCards(null, null);
            }
        }

        public static void Plus4Challenge()
        {
            if (RaisePlus4Challenge != null)
            {
                EventPublisher.RaisePlus4Challenge(null, null);
            }
        }

        public static void ColourPick(Suit pSuit)
        {
            if (RaiseColourPick != null)
            {
                EventPublisher.RaiseColourPick(null, new EventArgsColourPick(pSuit));
            }
        }      

        public static void NextPlayerButtonClick()
        {
            if (RaiseNextPlayerButtonClick != null)
            {
                EventPublisher.RaiseNextPlayerButtonClick(null, null);
            }
                
        }
    }
}
