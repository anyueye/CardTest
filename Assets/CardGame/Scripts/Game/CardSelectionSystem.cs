using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class CardSelectionSystem : GameSystem
    {
        public TargetingArrow _targetingArrow;

        public DeckDrawingSystem DeckDrawingSystem;
        public HandPresentationSystem HandPresentationSystem;


        private Camera MainCamera;
        private LayerMask CardLayer;
        private LayerMask EnemyLayer;

        private Entity SelectedCard;
        private bool cardHasTargetable;

        private Entity selectEnemy;


        private Vector3 prevClickPos;
        private bool isArrowCreated;
        private bool isCardAboutToBePlayed;

        private Vector3 originalCardPos;
        private Quaternion originalCardRot;
        private int originalCardSortingOrder;

        // 有目标
        private const float CardSelectionDetectionOffset = 2.2f;
        private const float CardSelectionAnimationTime = 0.2f;
        private const float SelectedCardYOffset = -3.2f;

        private const float CardSelectionCanceledAnimationTime = 0.2f;

        // 无目标
        private const float CardAnimationTime = 0.4f;
        private const float CardAboutToBePlayedOffsetY = 1.5f;

        public override void Init()
        {
            CardLayer = 1 << LayerMask.NameToLayer($"Card");
            EnemyLayer = 1 << LayerMask.NameToLayer($"Enemy");
            MainCamera = GameEntry.Scene.MainCamera;
            _targetingArrow = GameEntry.targetingArrow;
            _targetingArrow.Init();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            CardOver();
            if (isCardAboutToBePlayed)
            {
                return;
            }

            

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                DetectCardSelection();
                DetectEnemySelection();
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                DetectEnemySelection();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                DetectCardUnselection();
            }

            if (SelectedCard == null) return;
            if (cardHasTargetable)
            {
                UpdateTargetingArrow();
            }
            else
            {
                UpdateSelectedCard();
            }

        }

        private Card tempCard;
        private void CardOver()
        {
            var mousePos = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, CardLayer);
            
            if (hitInfo.collider != null)
            {
                var temppCard=hitInfo.collider.GetComponent<Entity>() as Card;

                if (temppCard!=tempCard&&tempCard!=null)
                {
                    tempCard.OnMouseExit();
                    tempCard = null;
                }
                tempCard = temppCard;
                tempCard.OnMouseEnter();
            }
            else
            {
                if (tempCard!=null)
                {
                    tempCard.OnMouseExit();
                    tempCard = null;
                }
            }
        }

        /// <summary>
        /// 检测点击卡
        /// </summary>
        private void DetectCardSelection()
        {
            // Checks if the player clicked over a card.
            var mousePos = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, CardLayer);
            if (hitInfo.collider != null && !SelectedCard)
            {
                var card = hitInfo.collider.GetComponent<Entity>() as Card;
                if (!CardUtils.CardCanBePlayed()) return;
                SelectedCard = card;
                cardHasTargetable = CardUtils.CardHastargetableEffect(card);
                if (cardHasTargetable)
                {
                    card.CardSortingGroup.sortingOrder += 10;
                    prevClickPos = mousePos;
                }
                else
                {
                    originalCardPos = card.CachedTransform.position;
                    originalCardRot = card.CachedTransform.rotation;
                    originalCardSortingOrder = card.CardSortingGroup.sortingOrder;
                }
            }
        }

        /// <summary>
        /// 检测点击敌人
        /// </summary>
        private void DetectEnemySelection()
        {
            if (SelectedCard == null) return;
            var mousePos = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, EnemyLayer);
            if (hitInfo.collider != null)
            {
                selectEnemy = hitInfo.collider.GetComponent<Entity>();
                PlaySelectedCard();

                SelectedCard = null;

                isArrowCreated = false;
                _targetingArrow.EnableArrow(false);
            }
        }

        /// <summary>
        /// 取消选择卡
        /// </summary>
        private void DetectCardUnselection()
        {
            if (SelectedCard != null)
            {
                var card = SelectedCard as Card;
                if (cardHasTargetable)
                {
                    SelectedCard.CachedTransform.DOKill();
                    card.Reset(() =>
                    {
                        SelectedCard = null;
                        isArrowCreated = false;
                    });
                }
                else
                {
                    var seq = DOTween.Sequence();
                    seq.AppendCallback(() =>
                    {
                        card.SetState(Card.CardState.InHand);
                        SelectedCard.CachedTransform
                            .DOMove(originalCardPos, CardSelectionCanceledAnimationTime)
                            .SetEase(Ease.OutBack);
                        SelectedCard.CachedTransform.DORotate(originalCardRot.eulerAngles, CardSelectionCanceledAnimationTime);
                    });
                    seq.OnComplete(() =>
                    {
                        card.CardSortingGroup.sortingOrder = originalCardSortingOrder;
                        SelectedCard = null;
                    });
                }
            }

            _targetingArrow.EnableArrow(false);
        }

        /// <summary>
        /// 更新箭头指向
        /// </summary>
        private void UpdateTargetingArrow()
        {
            var mousePos = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var diffY = mousePos.y - prevClickPos.y;
            if (!isArrowCreated && diffY > CardSelectionDetectionOffset)
            {
                isArrowCreated = true;

                var pos = SelectedCard.CachedTransform.position;

                SelectedCard.CachedTransform.DOKill();
                var seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    SelectedCard.CachedTransform.DOMove(new Vector3(0, SelectedCardYOffset, pos.z),
                        CardSelectionAnimationTime);
                    SelectedCard.CachedTransform.DORotate(Vector3.zero, CardSelectionAnimationTime);
                });
                seq.AppendInterval(0.05f);
                seq.OnComplete(() => { _targetingArrow.EnableArrow(true); });
            }
        }

        private void UpdateSelectedCard()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                var card = SelectedCard as Card;
                if (card.State == Card.CardState.AboutToBePlayed)
                {
                    isCardAboutToBePlayed = true;

                    var seq = DOTween.Sequence();
                    seq.Append(SelectedCard.transform
                        .DOMove(Vector3.zero, CardAnimationTime)
                        .SetEase(Ease.OutBack));
                    seq.AppendInterval(CardAnimationTime + 0.1f);
                    seq.AppendCallback(() =>
                    {
                        PlaySelectedCard();
                        SelectedCard = null;
                        isCardAboutToBePlayed = false;
                    });
                    SelectedCard.transform.DORotate(Vector3.zero, CardAnimationTime);
                }
                else
                {
                    card.SetState(Card.CardState.InHand);
                    card.Reset(() => SelectedCard = null);
                }
            }

            if (SelectedCard == null) return;
            {
                var card = SelectedCard as Card;
                var mousePos = MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mousePos.z = 0;
                card.CachedTransform.position = mousePos;
                if (mousePos.y > originalCardPos.y + CardAboutToBePlayedOffsetY)
                    card.SetState(Card.CardState.AboutToBePlayed);
                else
                    card.SetState(Card.CardState.InHand);
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }


        protected virtual void PlaySelectedCard()
        {
            var card = SelectedCard as Card;
            card.SetInteractable(false);

            HandPresentationSystem.RearrangeHand(card);
            HandPresentationSystem.RemoveCardFromHand(card);
            HandPresentationSystem.MoveCardToDiscardPile(card);

            DeckDrawingSystem.MoveCardToDiscardPile(card.Id);
        }

        public bool HasSelectedCard()
        {
            return SelectedCard != null;
        }
    }
}