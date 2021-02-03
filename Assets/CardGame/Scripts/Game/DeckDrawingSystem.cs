using System.Collections.Generic;

namespace CardGame
{
    public class DeckDrawingSystem : GameSystem
    {
        public HandPresentationSystem handPresentatation;

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
            deckPile = new List<int>();
            discardPile = new List<int>();
            handPile = new List<int>();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
        

        public void ShuffleDeck()
        {
            deckPile.Shuffle();
        }

        public int LoadDeck(List<int> deck)
        {
            var deckSize = 0;
            deckPile.AddRange(deck);
            deckSize = deckPile.Count;
            gameUI.SetAmount(deckSize, GameForm.DeckOrDiscard.deck);
            gameUI.SetAmount(0, GameForm.DeckOrDiscard.discard);
            return deckSize;
        }

        public void DrawCardsFromDeck(int drawCount)
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

                    handPresentatation.CreateCardInHand(drawnCards, prevDeckSize);
                }
                else
                {
                    deckPile.AddRange(discardPile);
                    discardPile.Clear();
                    handPresentatation.UpdateDiscardPileSize(discardPile.Count);
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

        public void MoveCardToDiscardPile(int cardID)
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
}