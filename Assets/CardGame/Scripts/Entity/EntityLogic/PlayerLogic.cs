using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Rendering;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class PlayerLogic : TargetableObject
    {
        [SerializeField] private PlayerData m_PlayerData = null;


        /// <summary>
        /// 抽排堆
        /// </summary>
        public List<int> deckPile = new List<int>();

        /// <summary>
        /// 弃牌堆
        /// </summary>
        public List<int> discardPile = new List<int>();

        /// <summary>
        /// 手牌
        /// </summary>
        public List<int> handPile = new List<int>();

        private List<Card> handCards = new List<Card>();


        private List<Vector3> positions;
        private List<Quaternion> rotations;
        private List<int> sortingOrders;

        // Change these values to the ones that make the most sense for your game. These
        // must be in sync with the capacities defined in DeckDrawingSystem.
        private const int PositionsCapacity = 30;
        private const int RotationsCapacity = 30;
        private const int SortingOrdersCapacity = 30;

        /// <summary>
        /// 牌堆宽度
        /// </summary>
        private float cardWight => 3f;

        /// <summary>
        /// 卡牌宽度
        /// </summary>
        float cardWidth => 0.5f;


        private const float CenterRadius = 16.0f;

        //卡牌初始位置
        private readonly Vector3 centerPoint = new Vector3(0.0f, -20.5f, 0.0f);

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            positions = new List<Vector3>(PositionsCapacity);
            rotations = new List<Quaternion>(RotationsCapacity);
            sortingOrders = new List<int>(SortingOrdersCapacity);
        }

        protected override void OnRecycle()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            base.OnRecycle();
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
            if (ne.EntityLogicType == typeof(Card))
            {
                var card = (Card) ne.Entity.Logic;
                card.playerLogic = this;
                card.SetInteractable(false);
                handCards.Add(card);
                // ReflushCardPos();
                if (handCards.Count==handPile.Count)
                {
                    AnimateCardsFromDeckToHand();
                }
            }
        }

        void AnimateCardsFromDeckToHand()
        {
            ArrangeHandCrads();
            var interval = 0.0f;
            for (int i = 0; i < handCards.Count; i++)
            {
                var i1 = i;
                const float time = 0.5f;
                var card = handCards[i];
                var seq = DOTween.Sequence();
                seq.AppendInterval(interval);
                seq.AppendCallback(() =>
                {
                    var move = card.CachedTransform.DOMove(positions[i1], time).OnComplete(() =>
                    {
                        card.CacheTransform(positions[i1], rotations[i1]);
                        card.SetInteractable(true);
                    });
                    card.CachedTransform.DORotateQuaternion(rotations[i1], time);
                });
                card.CardSortingGroup.sortingOrder = sortingOrders[i];

                interval += 0.2f;
            }
        }

        /// <summary>
        /// 计算手牌位置
        /// </summary>
        private void ArrangeHandCrads()
        {
            positions.Clear();
            rotations.Clear();
            sortingOrders.Clear();
            const float angle = 5.0f;
            var cardAngle = (handCards.Count - 1) * angle / 2;
            var z = 0.0f;
            for (int i = 0; i < handCards.Count; i++)
            {
                //rotate
                var rotation = Quaternion.Euler(0, 0, cardAngle - i * angle);    
                rotations.Add(rotation);
                //move
                z -= 0.1f;
                var position = CalculateCardPosition(cardAngle - i * angle);
                position.z = z;
                positions.Add(position);
                sortingOrders.Add(i);
            }
        }

        private void RearrangeHand(Card selectedCard)
        {
            handCards.Remove(selectedCard);
            ArrangeHandCrads();

            for (int i = 0; i < handCards.Count; i++)
            {
                var card = handCards[i];
                const float time = 0.3f;
                card.CachedTransform.DOMove(positions[i], time);
                card.CachedTransform.DORotateQuaternion(rotations[i], time);
                card.CardSortingGroup.sortingOrder = sortingOrders[i];
                // card.GetComponent<CardObject>().SetGlowEnabled(playerMana.Value);
                card.CacheTransform(positions[i], rotations[i]);
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EndOfRound();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                DrawCardsFromDeck(3);
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            deckPile.AddRange(m_PlayerData.allCards);
            if (m_PlayerData != null) DrawCardsFromDeck(6);
        }


        public void DrawCardsFromDeck(int drawCount)
        {
            while (true)
            {
                var deckSize = deckPile.Count;
                if (deckSize >= drawCount)
                {
                    // var drawCardList = deckPile.GetRandom(drawCount);
                    // ReflushCard(drawCardList);
                    // foreach (var cardId in drawCardList)
                    // {
                    //     deckPile.Remove(cardId);
                    // }
                    var prevDeckSize = deckSize;
                    var drawnCards = new List<int>(drawCount);
                    for (int i = 0; i < drawCount; i++)
                    {
                        var cardId = deckPile[0];
                        deckPile.RemoveAt(0);
                        handPile.Add(cardId);
                        drawnCards.Add(cardId);
                    }

                    ReflushCard(drawnCards);
                }
                else
                {
                    // if (deckPile.Count < drawCount)
                    // {
                    //     ReflushCard(m_PlayerData.allCards.GetRandom(m_PlayerData.allCards.Count));
                    //     deckPile.Clear();
                    //     return;
                    // }
                    //
                    // ReflushCard(deckPile);
                    // deckPile.Clear();
                    // deckPile.AddRange(m_PlayerData.allCards);
                    // foreach (var cardId in handPile)
                    // {
                    //     deckPile.Remove(cardId);
                    // }
                    //
                    // discardPile.Clear();
                    // drawCount -= handPile.Count;
                    deckPile.AddRange(discardPile);
                    discardPile.Clear();

                    if (drawCount > deckPile.Count + discardPile.Count)
                    {
                        drawCount = deckPile.Count + discardPile.Count;
                    }

                    continue;
                }

                break;
            }
        }

        void ReflushCard(IReadOnlyList<int> cards)
        {
            int len = cards.Count;
            for (int i = 0; i < len; i++)
            {
                GameEntry.Entity.ShowCard(new CardData(GameEntry.Entity.GenerateSerialId(), 100, cards[i])
                {
                    Position = new Vector3(-3, -3),
                });
            }
        }

        void MoveCardsToDiscardPile()
        {
            discardPile.AddRange(handPile);
            handPile.Clear();
            // if (deckPile.Count > 0) return;
            // deckPile.AddRange(m_PlayerData.allCards);
            // discardPile.Clear();
        }

        void MoveCardToDiscardPile(Card c)
        {
            handPile.Remove(c.CardData.CardId);
            discardPile.Add(c.CardData.CardId);
        }

        public void PushCard(Card c)
        {
            handPile.Remove(c.CardData.CardId);
            discardPile.Add(c.CardData.CardId);
            GameEntry.Entity.HideEntity(c.Id);
        }

        public void EndOfRound()
        {
            // for (int i = handCards.Count - 1; i >= 0; i--)
            // {
            //     GameEntry.Entity.HideEntity(handCards[i].Id);
            // }
            //
            // handCards.Clear();
            MoveCardsToDiscardPile();
        }

        private Vector3 CalculateCardPosition(float angle)
        {
            return new Vector3(
                centerPoint.x - CenterRadius * Mathf.Sin(Mathf.Deg2Rad * angle),
                centerPoint.y + CenterRadius * Mathf.Cos(Mathf.Deg2Rad * angle),
                0.0f);
        }
    }
}