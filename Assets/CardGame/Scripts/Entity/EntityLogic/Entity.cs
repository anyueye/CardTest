using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class Entity:EntityLogic
    {
        [SerializeField] private EntityData m_EntityData = null;
        public int Id
        {
            get => Entity.Id;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_EntityData=userData as EntityData;
            if (m_EntityData==null)
            {
                return;
            }
            CachedTransform.localPosition = m_EntityData.Position;
            CachedTransform.localRotation = Quaternion.identity;
            CachedTransform.localScale = Vector3.one;
        }
    }
}