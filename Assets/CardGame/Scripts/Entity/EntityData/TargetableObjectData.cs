using System;
using UnityEngine;

namespace CardGame
{
    [Serializable]
    public abstract class TargetableObjectData:EntityData
    {
        [SerializeField]
        private CampType m_camp = CampType.Unknown;

        [SerializeField] private int m_HP = 0;
        
        public TargetableObjectData(int entityId, int typeId,CampType camp) : base(entityId, typeId)
        {
            m_camp = camp;
        }
        
        public CampType Camp
        {
            get => m_camp;
        }

        public int currentHP
        {
            get => m_HP;
            set => m_HP = value;
        }
        
        public abstract int MaxHP { get; set; }
        public float HPRatio
        {
            get => MaxHP > 0 ? (float) currentHP / MaxHP : 0;
        }
    }
}