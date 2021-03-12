using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class IntentItem : MonoBehaviour
    {
        [SerializeField] private Image intent=null;
        [SerializeField] private TextMeshProUGUI intentValue = null;

        private Canvas _parentCanvas=null;
        private RectTransform _cacheTransform = null;
        private Entity _owner = null;
        private int _ownerId = 0;
    
        public Entity Owner => _owner;

        private void Awake()
        {
            _cacheTransform = GetComponent<RectTransform>();
            if (_cacheTransform != null) return;
            Log.Error("RectTransform is invalid.");
            return;

        }
    
        public void Init(Entity owner_, Canvas parentCanvas, Sprite sprite, int value)
        {
            if (owner_==null)
            {
                return;
            }

            _parentCanvas = parentCanvas;
            gameObject.SetActive(true);
            if (_owner!=owner_||_ownerId!=owner_.Id)
            {
                intent.sprite = sprite;
                intentValue.text = value.ToString();
                _owner = owner_;
                _ownerId = owner_.Id;
            }

            Refresh();
        }
        
        public void Reset()
        {
            _owner = null;
            gameObject.SetActive(false);
        }

        public void ReflushIntent(Entity owner, int value, Sprite sprite)
        {
            intent.sprite = sprite;
            intentValue.text = value.ToString();
        }

        private bool Refresh()
        {
            if (_owner == null || !Owner.Available || Owner.Id != _ownerId) return true;
            var pivot = _owner.CachedTransform;
            var size = _owner.GetComponent<BoxCollider2D>().bounds.size;
            var canvasPos = GameEntry.Scene.MainCamera.WorldToViewportPoint(pivot.position + new Vector3(-0.5f, size.y + 0.7f, 0.0f));
            _cacheTransform.anchorMin =_cacheTransform.anchorMax= canvasPos;
            return true;
        }
    }
}
