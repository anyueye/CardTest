using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace CardGame
{
    public class Card:Entity
    {
        private CardData _cardData;
        public PlayerLogic playerLogic;
        private SpriteRenderer sr => GetComponent<SpriteRenderer>();
        private SpriteRenderer image;
        private TextMeshPro name;
        private TextMeshPro describe;
        private Vector3 onSelectCardPos = Vector3.zero;
        [SerializeField] private CardState _cardState = CardState.onExit;
        
        private SortingGroup sortingGroup;
        private Vector3 cachedPos;
        private Quaternion cachedRot;
        private int cachedSortingOrder;
        private int highlightedSortingOrder;
        

        private bool interactable = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            sortingGroup = CachedTransform.GetComponent<SortingGroup>();
            image = CachedTransform.Find("image").GetComponent<SpriteRenderer>();
            name = CachedTransform.Find("name").GetComponent<TextMeshPro>();
            describe = CachedTransform.Find("describe").GetComponent<TextMeshPro>();

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _cardData= userData as CardData;
            
            
            name.text = _cardData.CardName;
            // if (_cardData.Damage<0)
            // {
            //     describe.text = Utility.Text.Format(_cardData.Descirbe,_cardData.Recover);
            //     return;
            // }
            //
            // if (_cardData.Recover<0)
            // {
            //     describe.text = Utility.Text.Format(_cardData.Descirbe,_cardData.Damage);
            //     return;
            // }
            // describe.text = Utility.Text.Format(_cardData.Descirbe,_cardData.Damage,_cardData.Recover);
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

        public void CacheTransform(Vector3 position, Quaternion rotation)
        {
            cachedPos = position;
            cachedRot = rotation;
            cachedSortingOrder = sortingGroup.sortingOrder;
            highlightedSortingOrder = cachedSortingOrder + 10;
        }
        
        
        private void OnMouseEnter()
        {
            if (_cardState != CardState.onExit) return;
            _cardState = CardState.onHover;
            transform.DOScale(Vector3.one * 1.2f, 0.2f);
            transform.DOLocalMove(new Vector3(onSelectCardPos.x, sr.size.y * 0.1f, -0.1f), 0.2f);
        }

        private void OnMouseExit()
        {
            if (_cardState == CardState.onHover)
            {
                _cardState = CardState.onExit;
                transform.DOScale(Vector3.one, 0.2f);
                transform.DOLocalMove(onSelectCardPos, 0.2f);
            }
        }

        private void OnMouseDown()
        {
            _cardState = CardState.onSelect;
        }

        private void OnMouseUp()
        {
            if (transform.position.y >onSelectCardPos.y+sr.size.y+0.2f)
            {
                transform.localScale=Vector3.one;
                playerLogic.PushCard(this);
                return;
            }

            _cardState = CardState.onUnSelect;
            transform.localPosition = onSelectCardPos;
            transform.localScale = Vector3.one;
            _cardState = CardState.onExit;
        }

        private void OnMouseDrag()
        {
            if (_cardState != CardState.onSelect) return;
            var pos = Input.mousePosition;
            pos.z = Mathf.Abs(transform.parent.position.z - Camera.main.transform.position.z) - 0.1f;
            transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
    }
}