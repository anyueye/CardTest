using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace CardGame
{
    public class DeckDrawingSystem : GameSystem
    {
        // public HandPresentationSystem handPresentatation;

        /// <summary>
        /// 抽排堆
        /// </summary>
        public List<int> deckPile;

        /// <summary>
        /// 弃牌堆
        /// </summary>
        public List<int> discardPile;

        /// <summary>
        /// 手牌
        /// </summary>
        public List<int> handPile;

        public override void Init()
        {
            base.Init();
            GameEntry.Event.Subscribe(CardSelectionEventArgs.EventId,MoveCardToDiscardPile);
            GameEntry.Event.Subscribe(DrawnCardEventArgs.EventId,DrawCardsFromDeck);
            deckPile = new List<int>();
            discardPile = new List<int>();
            handPile = new List<int>();
        }

        private void DrawCardsFromDeck(object sender, GameEventArgs e)
        {
            DrawnCardEventArgs ne = (DrawnCardEventArgs) e;
            DrawCardsFromDeck(ne.drawCount);
        }

        private void MoveCardToDiscardPile(object sender, GameEventArgs e)
        {
            CardSelectionEventArgs ne = (CardSelectionEventArgs) e;
            MoveCardToDiscardPile(ne.selectCard.Id);
        }

        public override void Shutdown()
        {
            deckPile.Clear();
            discardPile.Clear();
            handPile.Clear();
            GameEntry.Event.Unsubscribe(DrawnCardEventArgs.EventId,DrawCardsFromDeck);
            GameEntry.Event.Unsubscribe(CardSelectionEventArgs.EventId,MoveCardToDiscardPile);
            base.Shutdown();
        }
        

        public void ShuffleDeck()
        {
            deckPile.Shuffle();
        }

        public int LoadDeck(IEnumerable<int> deck)
        {
            var deckSize = 0;
            deckPile.AddRange(deck);
            deckSize = deckPile.Count;
            gameUI.SetAmount(deckSize, GameForm.DeckOrDiscard.deck);
            gameUI.SetAmount(0, GameForm.DeckOrDiscard.discard);
            return deckSize;
        }

        private void DrawCardsFromDeck(int drawCount)
        {
            while (true)
            {
                var deckSize = deckPile.Count;
                if (deckSize >= drawCount)
                {
                    var prevDeckSize = deckSize;
                    var drawnCards = new List<int>(drawCount);
                    for (int i = 0; i < drawCount; i++)
                    {
                        var cardId = deckPile[0];
                        deckPile.RemoveAt(0);
                        handPile.Add(cardId);
                        drawnCards.Add(cardId);
                    }

                    // handPresentatation.CreateCardInHand(drawnCards, prevDeckSize);
                    GameEntry.Event.FireNow(this,DeckDrawingEventArgs.Create(drawnCards));
                    GameEntry.Event.FireNow(this,UpdateDeckCountEventArgs.Create(prevDeckSize));

                }
                else
                {
                    deckPile.AddRange(discardPile);
                    discardPile.Clear();
                    GameEntry.Event.FireNow(this,UpdateDiscardCountEventArgs.Create(discardPile.Count));
                    // handPresentatation.UpdateDiscardPileSize(discardPile.Count);
                    if (drawCount > deckPile.Count + discardPile.Count)
                    {
                        drawCount = deckPile.Count + discardPile.Count;
                    }

                    deckPile.Shuffle();
                    continue;
                }

                break;
            }
        }

        private void MoveCardToDiscardPile(int cardID)
        {
            handPile.Remove(cardID);
            discardPile.Add(cardID);
        }

        public void MoveCardToDiscardPile()
        {
            discardPile.AddRange(handPile);
            handPile.Clear();
        }
    }

    public sealed class DeckDrawingEventArgs : GameEventArgs
    {
        public static int EventId => typeof(DeckDrawingEventArgs).GetHashCode();

        public List<int> drawnCards;
        
        public static DeckDrawingEventArgs Create(List<int> drawnCard)
        {
            DeckDrawingEventArgs result = ReferencePool.Acquire<DeckDrawingEventArgs>();
            result.drawnCards = drawnCard;
            return result;
        }
        public override void Clear()
        {
            drawnCards.Clear();
        }

        public override int Id => EventId;
    }
    public sealed class UpdateDeckCountEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateDeckCountEventArgs).GetHashCode();
        public int deckCount;

        public UpdateDeckCountEventArgs()
        {
            deckCount = 0;
        }
        public static UpdateDeckCountEventArgs Create(int count)
        {
            UpdateDeckCountEventArgs result = ReferencePool.Acquire<UpdateDeckCountEventArgs>();
            result.deckCount = count;
            return result;
        }
        public override void Clear()
        {
            deckCount = 0;
        }

        public override int Id => EventId;
    }

    public sealed class UpdateDiscardCountEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateDiscardCountEventArgs).GetHashCode();
        public int discardCount;

        public UpdateDiscardCountEventArgs()
        {
            discardCount = 0;
        }
        public static UpdateDiscardCountEventArgs Create(int count)
        {
            UpdateDiscardCountEventArgs result = ReferencePool.Acquire<UpdateDiscardCountEventArgs>();
            result.discardCount = count;
            return result;
        }
        public override void Clear()
        {
            discardCount = 0;
        }

        public override int Id => EventId;
    }
    
    
}