﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Uno.EventsComponents;

namespace Uno.Game
{
    class UnoGameHouse3: UnoGame
    {
        protected int mNextPlayerInLine;
        protected int mNextPlayerAfterSkips;

        public UnoGameHouse3(List<string> pPlayerNames, int pdealer, int pNumOfSwapHands) : base(pPlayerNames, pdealer, pNumOfSwapHands)
        {
            
        }

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
        }

        protected override void UnoGame_RaiseUnsubscribeEvents(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseUnsubscribeEvents(sender, eventArgs);
        }

        protected override int NextPlayerWithoutSkips()
        {
            return base.NextPlayerWithoutSkips();
        }

        protected override void FinishPlaceCard()
        {
            base.FinishPlaceCard();
        }

        protected override void UnoGame_RaiseDrawTwoCards(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseDrawTwoCards(sender, eventArgs);
        }

        protected override void UnoGame_RaiseNextPlayerButtonClick(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaiseNextPlayerButtonClick(sender, eventArgs);
        }

        protected override void UnoGame_RaisePlayCard(object sender, EventArgsPlayCard eventArgs)
        {
            base.UnoGame_RaisePlayCard(sender, eventArgs);
        }

        protected override void UnoGame_AcceptDraw4(object sender, EventArgs eventArgs)
        {
            base.UnoGame_AcceptDraw4(sender, eventArgs);
        }

        protected override void UnoGame_RaisePlus4Challenge(object sender, EventArgs eventArgs)
        {
            base.UnoGame_RaisePlus4Challenge(sender, eventArgs);
        }
    }
}
