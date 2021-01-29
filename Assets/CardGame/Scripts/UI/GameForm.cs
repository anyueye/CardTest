using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class GameForm : UGuiForm
    {
        public enum DeckOrDiscard
        {
            deck,
            discard
        }
        
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI deckCountText;
        [SerializeField] private TextMeshProUGUI discardPileText;
        [SerializeField] private Button endTurnButton;

        private int maxCost;
        private int deckSize;
        private int discardSize;
        public void InitializeCost(VarInt32 cost)
        {
            maxCost = cost.Value;
            SetValue(cost.Value);
        }

        private void SetValue(int value)
        {
            costText.text = $"{value.ToString()}/{maxCost.ToString()}";
        }

        public void OnCostChanged(int value)
        {
            SetValue(value);
        }

        public void SetAmount(int amount,DeckOrDiscard deckOrDiscard)
        {
            switch (deckOrDiscard)
            {
                case DeckOrDiscard.deck:
                    deckSize = amount;
                    deckCountText.text = amount.ToString();
                    break;
                case DeckOrDiscard.discard:
                    discardSize = amount;
                    discardPileText.text = amount.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deckOrDiscard), deckOrDiscard, null);
            }
        }

        public void RemoveCard()
        {
            SetAmount(deckSize-1,DeckOrDiscard.deck);
        }

        public void AddCard()
        {
            SetAmount(discardSize+1,DeckOrDiscard.discard);
        }
        
        
        
        
        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var tes= GameEntry.UI.GetUIForm(UIFormId.GameForm) as GameForm;
            tes.SetAmount(5,DeckOrDiscard.deck);
            Log.Info(tes);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }  
}

