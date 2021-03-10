using System;
using DG.Tweening;
using GameFramework.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

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
            /// <summary>
            /// 在手中
            /// </summary>
            InHand,
            /// <summary>
            /// 释放
            /// </summary>
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
            GameEntry.Resource.LoadAsset(AssetUtility.GetCardIconAsset(_cardData.IconPath), typeof(Sprite), Constant.AssetPriority.DictionaryAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, uD) =>
                {
                    
                    var _icon = asset as Sprite;
                    image.sprite = _icon;
                    Log.Info("Load Sprite '{0}' OK.", assetName,_icon);
                },
                (assetName, status, errorMessage, uD) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", _cardData.IconPath, assetName, errorMessage);
                }));
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
            CachedTransform.DOMove(cachedPos + new Vector3(0, 0.3f, 0), 0.05f).SetEase(Ease.OutCubic).OnComplete(() => beingHighlighted = false);
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
            CachedTransform.DOMove(cachedPos, 0.02f).SetEase(Ease.OutCubic).OnComplete(() => beingUnhighlighted = false);
        }

        public void Reset(Action onComplete)
        {
            interactable = false;
            var seq = DOTween.Sequence();
            seq.Append(CachedTransform.DOMove(cachedPos, 0.2f));
            seq.Insert(0, CachedTransform.DORotateQuaternion(cachedRot, 0.2f));
            seq.OnComplete(() => interactable = true);
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