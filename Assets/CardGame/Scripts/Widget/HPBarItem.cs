//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class HPBarItem : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Image hpBar;
        [SerializeField]
        private Image hpBarBackground;
        [SerializeField]
        private TextMeshProUGUI hpText;
        [SerializeField]
        private GameObject shieldGroup;
        [SerializeField]
        private TextMeshProUGUI shieldText;
#pragma warning restore 649
        private int maxValue;
        
        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private Entity m_Owner = null;
        private int m_OwnerId = 0;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner, Canvas parentCanvas,int hp,int maxHp,int shield)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }
            m_ParentCanvas = parentCanvas;
            
            gameObject.SetActive(true);
            
            
            
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                maxValue = maxHp;
                var newValue = hp / (float)maxValue;
                hpBar.fillAmount = newValue;
                hpText.text = $"{hp.ToString()}/{maxValue.ToString()}";
                m_Owner = owner;
                m_OwnerId = owner.Id;
                Refresh();
            }
            else
            {
                SetHp(hp);
            }
            SetShield(shield);
        }
        
        private void SetHp(int value)
        {
            var newValue = value / (float)maxValue;
            hpBar.DOFillAmount(newValue, 0.2f)
                .SetEase(Ease.InSine);

            var seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(hpBarBackground.DOFillAmount(newValue, 0.2f));
            seq.SetEase(Ease.InSine);
            hpText.text = $"{value.ToString()}/{maxValue.ToString()}";
        }

        private void SetShield(int value)
        {
            shieldText.text = $"{value.ToString()}";
            SetShieldActive(value > 0);
        }

        private void SetShieldActive(bool shieldActive)
        {
            shieldGroup.SetActive(shieldActive);
        }

        private bool Refresh()
        {
            if (m_Owner == null || !Owner.Available || Owner.Id != m_OwnerId) return true;
            var pivot = m_Owner.CachedTransform;
            var canvasPos = GameEntry.Scene.MainCamera.WorldToViewportPoint(pivot.position + new Vector3(0, -0.5f, 0));
            m_CachedTransform.anchorMin =m_CachedTransform.anchorMax= canvasPos;
            return true;
        }
        public void Reset()
        {
            m_Owner = null;
            gameObject.SetActive(false);
        }
        

        private void Awake()
        {
            m_CachedTransform = GetComponent<RectTransform>();
            if (m_CachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }
            
        }
    }
}
