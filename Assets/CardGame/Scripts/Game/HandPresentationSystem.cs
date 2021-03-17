using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class HandPresentationSystem:GameSystem
    {
        private List<Card> handCards=new List<Card>();
        private List<Card> drawnCard=new List<Card>();
        
        private int drawnSize;
        private int showSize;
        private bool isAnimating;
        
        private List<Vector3> positions;
        private List<Quaternion> rotations;
        private List<int> sortingOrders;
        
        //手牌上限
        private const int PositionsCapacity = 30;
        private const int RotationsCapacity = 30;
        private const int SortingOrdersCapacity = 30;
        
        private const float CenterRadius = 16.0f;
        private readonly Vector3 centerPoint = new Vector3(0.0f, -19.7f, 0.0f);
        private readonly Vector3 originalCardScale = new Vector3(0.6f, 0.6f, 1.0f);
        
        public static float CardToDiscardPileAnimationTime = 0.3f;
        public CardSelectionSystem cardSelectionSystem;
        
        public override void Init()
        {
            base.Init();
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameEntry.Event.Subscribe(CardSelectionEventArgs.EventId,CardOut);
            GameEntry.Event.Subscribe(DeckDrawingEventArgs.EventId,OpreateCards);
            GameEntry.Event.Subscribe(UpdateDeckCountEventArgs.EventId,UpdateDeckCount);
            GameEntry.Event.Subscribe(UpdateDiscardCountEventArgs.EventId,UpdateDiscardCount);
            positions = new List<Vector3>(PositionsCapacity);
            rotations = new List<Quaternion>(RotationsCapacity);
            sortingOrders = new List<int>(SortingOrdersCapacity);
        }

        private void CardOut(object sender, GameEventArgs e)
        {
            CardSelectionEventArgs ne = (CardSelectionEventArgs) e;
            var card = ne.selectCard;
            RearrangeHand(card);
            RemoveCardFromHand(card);
            MoveCardToDiscardPile(card);
        }

        public override void Shutdown()
        {
            GameEntry.Event.Unsubscribe(CardSelectionEventArgs.EventId,CardOut);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameEntry.Event.Unsubscribe(DeckDrawingEventArgs.EventId,OpreateCards);
            GameEntry.Event.Unsubscribe(UpdateDeckCountEventArgs.EventId,UpdateDeckCount);
            GameEntry.Event.Unsubscribe(UpdateDiscardCountEventArgs.EventId,UpdateDiscardCount);
            base.Shutdown();
        }

        private void UpdateDeckCount(object sender, GameEventArgs e)
        {
            UpdateDeckCountEventArgs ne = (UpdateDeckCountEventArgs) e;
            gameUI.SetAmount(ne.deckCount,GameForm.DeckOrDiscard.deck);
        }

        private void UpdateDiscardCount(object sender, GameEventArgs e)
        {
            UpdateDiscardCountEventArgs ne = (UpdateDiscardCountEventArgs) e;
            gameUI.SetAmount(ne.discardCount, GameForm.DeckOrDiscard.discard);
        }

        private void OpreateCards(object sender, GameEventArgs e)
        {
            DeckDrawingEventArgs ne = (DeckDrawingEventArgs) e;
            CreateCardInHand(ne.drawnCards);
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
                handCards.Add(card);
                drawnCard.Add(card);
                showSize++;
                card.SetInteractable(false);
                card.css = cardSelectionSystem;
                if (showSize==drawnSize)
                {
                    AnimateCardsFromDeckToHand(drawnCard);
                }
            }
        }


        private void CreateCardInHand(IEnumerable<int> hand)
        {
            drawnSize = 0;
            showSize = 0;
            drawnCard.Clear();
            foreach (var t in hand)
            {
                GameEntry.Entity.ShowCard(new CardData(GameEntry.Entity.GenerateSerialId(), 0, t)
                {
                    Position = gameUI.GetDeckPos(),
                    LocalScale = Vector3.zero,
                });
                ++drawnSize;
            }
        }
       
        void AnimateCardsFromDeckToHand(ICollection<Card> drawnCards)
        {
            isAnimating = true;
            ArrangeHandCrads();
            var interval = 0.0f;
            for (int i = 0; i < handCards.Count; i++)
            {
                var i1 = i;
                const float time = 0.5f;
                var card = handCards[i];
                if (drawnCards.Contains(card))
                {
                    var seq = DOTween.Sequence();
                    seq.AppendInterval(interval);
                    seq.AppendCallback(() =>
                    {
                        gameUI.RemoveDeckCard();
                        var move = card.CachedTransform.DOMove(positions[i1], time).OnComplete(() =>
                        {
                            card.CacheTransform(positions[i1], rotations[i1]);
                        
                        });
                        card.CachedTransform.DORotateQuaternion(rotations[i1], time);
                        card.CachedTransform.DOScale(originalCardScale, time);
                        if (i1==handCards.Count-1)
                        {
                            move.OnComplete(() =>
                            {
                                isAnimating = false;
                                card.CacheTransform(positions[i1], rotations[i1]);
                                foreach (var c in handCards)
                                {
                                    c.SetInteractable(true);
                                }
                            });
                        }
                    });
                    card.CardSortingGroup.sortingOrder = sortingOrders[i];
                    interval += 0.2f;
                }
                else
                {
                    card.CachedTransform.DOMove(positions[i1], time).OnComplete(() => {
                        card.CacheTransform(positions[i1], rotations[i1]);
                        card.SetInteractable(true);
                    });
                    card.CachedTransform.DORotateQuaternion(rotations[i1], time);
                }
            }
        }

        /// <summary>
        /// 计算手牌位置 Arrange:vt. 排列
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
        /// <summary>
        /// 删除特定牌后从新排列手牌   Rearrange:vt. 重新安排, 重新布置
        /// </summary>
        /// <param name="c"></param>
        void RearrangeHand(Card c)
        {
            handCards.Remove(c);
            ArrangeHandCrads();
            for (int i = 0; i < handCards.Count; i++)
            {
                var card = handCards[i];
                const float time = 0.1f;
                card.CachedTransform.DOMove(positions[i], time);
                card.CachedTransform.DORotateQuaternion(rotations[i], time);
                card.CardSortingGroup.sortingOrder = sortingOrders[i];
                card.CacheTransform(positions[i],rotations[i]);
            }
        }

        void RemoveCardFromHand(Card c)
        {
            handCards.Remove(c);
        }
        void MoveCardToDiscardPile(Card c)
        {
            var seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                c.CachedTransform.DOMove(gameUI.GetDiscardPos(), CardToDiscardPileAnimationTime);
                c.CachedTransform.DOScale(Vector3.zero, CardToDiscardPileAnimationTime).OnComplete(() =>
                {
                    GameEntry.Entity.HideEntity(c);
                });
            });
            seq.AppendCallback(() =>
            {
                gameUI.AddDiscardCard();
                handCards.Remove(c);
            });
        }
        public void MoveHandToDiscardPile()
        {
            foreach (var card in handCards)
                MoveCardToDiscardPile(card);
            handCards.Clear();
        }
        
        private Vector3 CalculateCardPosition(float angle)
        {
            return new Vector3(
                centerPoint.x - CenterRadius * Mathf.Sin(Mathf.Deg2Rad * angle),
                centerPoint.y + CenterRadius * Mathf.Cos(Mathf.Deg2Rad * angle),
                0.0f);
        }
        public bool IsAnimating()
        {
            return isAnimating;
        }
    }
}