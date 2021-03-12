//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class WidgetComponent : GameFrameworkComponent
    {
        [SerializeField]
        private HPBarItem m_HPBarItemTemplate = null;

        [SerializeField] private IntentItem m_IntentItemTemplate = null;

        [SerializeField] private Transform m_WidgetInstanceRoot = null;

        [SerializeField] private int m_HpPoolCapacity = 16;

        [SerializeField] private int m_IntentPoolCapacity = 16;
        
        

        private IObjectPool<HPBarItemObject> m_HPBarItemObjectPool = null;
        private List<HPBarItem> m_ActiveHPBarItems = null;

        private IObjectPool<IntentItemObject> m_IntentObjectPool = null;
        private List<IntentItem> m_ActiveIntentItem = null;
        
        
        
        private Canvas m_CachedCanvas = null;

        private void Start()
        {
            if (m_WidgetInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }
            m_CachedCanvas = m_WidgetInstanceRoot.GetComponent<Canvas>();
            m_HPBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HPBarItemObject>("HPBarItem", m_HpPoolCapacity);
            m_IntentObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<IntentItemObject>("IntentItem", m_IntentPoolCapacity);
            m_ActiveIntentItem = new List<IntentItem>();
            m_ActiveHPBarItems = new List<HPBarItem>();
        }

        private void OnDestroy()
        {
        }

        public void ShowIntent(Entity entity, Sprite sp, int value)
        {
            if (entity==null)
            {
                return;
            }

            IntentItem intentItem = GetActiveIntentItem(entity);
            if (intentItem==null)
            {
                intentItem = CreateIntentItem(entity);
                m_ActiveIntentItem.Add(intentItem);
            }

            intentItem.Init(entity, m_CachedCanvas, sp,value);
        }

        public void ShowHPBar(Entity entity, int hp,int maxHp,int shield)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            HPBarItem hpBarItem = GetActiveHPBarItem(entity);
            if (hpBarItem == null)
            {
                hpBarItem = CreateHPBarItem(entity);
                m_ActiveHPBarItems.Add(hpBarItem);
            }
            
            hpBarItem.Init(entity, m_CachedCanvas, hp,maxHp,shield);
        }

        public void HideHPBar(HPBarItem hpBarItem)
        {
            m_ActiveHPBarItems.Remove(hpBarItem);
            m_HPBarItemObjectPool.Unspawn(hpBarItem);
        }
        
        public void HideHPBar(Entity entity)
        {
            var hpBarItem = GetActiveHPBarItem(entity);
            hpBarItem.Reset();
            m_ActiveHPBarItems.Remove(hpBarItem);
            m_HPBarItemObjectPool.Unspawn(hpBarItem);
        }

        public void HideIntent(Entity entity)
        {
            var intentItem = GetActiveIntentItem(entity);
            if (intentItem==null)
            {
                return;
            }
            intentItem.Reset();
            m_ActiveIntentItem.Remove(intentItem);
            m_IntentObjectPool.Unspawn(intentItem);
        }

        private HPBarItem GetActiveHPBarItem(Entity entity)=>entity == null ? null : m_ActiveHPBarItems.FirstOrDefault(t => t.Owner == entity);

        private IntentItem GetActiveIntentItem(Entity entity) => entity == null ? null : m_ActiveIntentItem.FirstOrDefault(t => t.Owner == entity);
        

        private HPBarItem CreateHPBarItem(Entity entity)
        {
            HPBarItem hpBarItem;
            HPBarItemObject hpBarItemObject = m_HPBarItemObjectPool.Spawn();
            if (hpBarItemObject != null)
            {
                hpBarItem = (HPBarItem)hpBarItemObject.Target;
            }
            else
            {
                hpBarItem = Instantiate(m_HPBarItemTemplate,m_WidgetInstanceRoot,false);
                m_HPBarItemObjectPool.Register(HPBarItemObject.Create(hpBarItem), true);
            }

            return hpBarItem;
        }

        private IntentItem CreateIntentItem(Entity entity)
        {
            IntentItem intentItem = null;
            IntentItemObject intentItemObject = m_IntentObjectPool.Spawn();
            if (intentItemObject!=null)
            {
                intentItem = (IntentItem) intentItemObject.Target;
            }
            else
            {
                intentItem = Instantiate(m_IntentItemTemplate, m_WidgetInstanceRoot,false);
                m_IntentObjectPool.Register(IntentItemObject.Create(intentItem), true);
            }
            return intentItem;
        }
    }
}
