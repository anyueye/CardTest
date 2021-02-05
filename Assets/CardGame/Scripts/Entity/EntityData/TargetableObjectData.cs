using System;
using UnityEngine;

namespace CardGame
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData
    {
        [SerializeField] private int m_HP = 0;

        public TargetableObjectData(int entityId, int typeId) : base(entityId, typeId)
        {
        }

        public int currentHP
        {
            get => m_HP;
            set => m_HP = value > MaxHP ? MaxHP : value;
        }

        public abstract int MaxHP { get; set; }

        public float HPRatio
        {
            get => MaxHP > 0 ? (float) currentHP / MaxHP : 0;
        }
    }
}