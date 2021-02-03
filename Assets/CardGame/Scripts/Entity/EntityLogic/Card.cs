using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

namespace CardGame
{
    public class Card : Entity
    {
        private CardData _cardData;
        public PlayerLogic playerLogic;
        private SpriteRenderer sr => GetComponent<SpriteRenderer>();
        private SpriteRenderer image;
        private TextMeshPro costText;
        private TextMeshPro nameText;
        private TextMeshPro typeText;
        private TextMeshPro describeText;


        public CardSelectionSystem css;
        private Vector3 onSelectCardPos = Vector3.zero;

        public enum CardState
        {
            InHand,
            AboutToBePlayed
        }

        public CardState State => currState;
        private CardState currState;

        private SortingGroup sortingGroup;
        private Vector3 cachedPos;
        private Quaternion cachedRot;
        private int cachedSortingOrder;
        private int highlightedSortingOrder;


        private bool beingHighlighted;
        private bool beingUnhighlighted;

        private bool interactable = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            sortingGroup = CachedTransform.GetComponent<SortingGroup>();

            image = CachedTransform.Find("PictureMask/image").GetComponent<SpriteRenderer>();
            costText = CachedTransform.Find("CostText").GetComponent<TextMeshPro>();
            nameText = CachedTransform.Find("NameText").GetComponent<TextMeshPro>();
            typeText = CachedTransform.Find("TypeText").GetComponent<TextMeshPro>();
            describeText = CachedTransform.Find("DescribeText").GetComponent<TextMeshPro>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _cardData = userData as CardData;

            nameText.text = _cardData.CardName;
            costText.text = _cardData.Cost.ToString();
            image.sprite = _cardData.Icon;
            describeText.text = _cardData.Description;
        }
        


        public void SetInteractable(bool value)
        {
            interactable = value;
        }

        public CardData CardData
        {
            get => _cardData;
            set => _cardData = value;
        }

        public SortingGroup CardSortingGroup
        {
            get => sortingGroup;
        }

        public void SetState(CardState state)
        {
            currState = state;
            switch (currState)
            {
                case CardState.InHand:
                    // glow.color = inHandColor;
                    break;

                case CardState.AboutToBePlayed:
                    // glow.color = aboutToBePlayedColor;
                    break;
            }
        }


        public void CacheTransform(Vector3 position, Quaternion rotation)
        {
            cachedPos = position;
            cachedRot = rotation;
            cachedSortingOrder = sortingGroup.sortingOrder;
            highlightedSortingOrder = cachedSortingOrder + 10;
        }

        public void HighlightCard()
        {
            if (css.HasSelectedCard())
            {
                return;
            }

            if (beingHighlighted)
            {
                return;
            }

            beingHighlighted = true;
            sortingGroup.sortingOrder = highlightedSortingOrder;
            transform.DOMove(cachedPos + new Vector3(0, 0.3f, 0), 0.05f).SetEase(Ease.OutCubic).OnComplete(() => beingHighlighted = false);
        }

        public void UnHighlightCard()
        {
            if (css.HasSelectedCard())
            {
                return;
            }

            if (beingUnhighlighted)
            {
                return;
            }

            beingUnhighlighted = true;
            sortingGroup.sortingOrder = cachedSortingOrder;
            transform.DOMove(cachedPos, 0.02f).SetEase(Ease.OutCubic).OnComplete(() => beingUnhighlighted = false);
        }

        public void Reset(Action onComplete)
        {
            transform.DOMove(cachedPos, 0.2f);
            transform.DORotateQuaternion(cachedRot, 0.2f);
            sortingGroup.sortingOrder = cachedSortingOrder;
            onComplete();
        }


        public void OnMouseEnter()
        {
            
            if (interactable)
            {
                HighlightCard();
            }
        }

        public void OnMouseExit()
        {
            if (interactable)
            {
                UnHighlightCard();
            }
        }
    }
}