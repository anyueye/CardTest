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

        public CardSelectionSystem cardSelectionSystem;
        
        private int drawnSize;
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
        
        public override void Init()
        {
            base.Init();
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            
            positions = new List<Vector3>(PositionsCapacity);
            rotations = new List<Quaternion>(RotationsCapacity);
            sortingOrders = new List<int>(SortingOrdersCapacity);
        }

        public override void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            base.Shutdown();
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
                card.SetInteractable(false);
                card.css = cardSelectionSystem;
                if (handCards.Count==drawnSize)
                {
                    AnimateCardsFromDeckToHand();
                }
            }
        }
        
       public void CreateCardInHand(List<int> hand, int deckSize)
        {
            drawnSize = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                GameEntry.Entity.ShowCard(new CardData(GameEntry.Entity.GenerateSerialId(), 100, hand[i])
                {
                    Position = gameUI.GetDeckPos(),
                    LocalScale = Vector3.zero,
                });
                ++drawnSize;
            }
            gameUI.SetAmount(deckSize,GameForm.DeckOrDiscard.deck);
        }

       public void UpdateDiscardPileSize(int discardSize)
       {
           gameUI.SetAmount(discardSize, GameForm.DeckOrDiscard.discard);
       }
       
       
        void AnimateCardsFromDeckToHand()
        {
            isAnimating = true;
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
        public void RearrangeHand(Card c)
        {
            handCards.Remove(c);
            ArrangeHandCrads();
            for (int i = 0; i < handCards.Count; i++)
            {
                var card = handCards[i];
                const float time = 0.3f;
                card.CachedTransform.DOMove(positions[i], time);
                card.CachedTransform.DORotateQuaternion(rotations[i], time);
                card.CardSortingGroup.sortingOrder = sortingOrders[i];
                card.CacheTransform(positions[i],rotations[i]);
            }
        }

        public void RemoveCardFromHand(Card c)
        {
            handCards.Remove(c);
        }
        public void MoveCardToDiscardPile(Card c)
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