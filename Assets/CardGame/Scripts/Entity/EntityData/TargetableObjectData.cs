using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData
    {
        [SerializeField] private int m_HP = 0;
        [SerializeField] private List<Shield> shield=new List<Shield>();
        [SerializeField] private List<DRStatus> _statusVariables = new List<DRStatus>();
        public TargetableObjectData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
        [Serializable]
        public struct Shield
        {
            public int value;
            public int duration;
        }
        
        public struct Status
        {
            public DRStatus drStatus;
            public int Value;
        }

        public int currentHP
        {
            get => m_HP;
            set => m_HP = value > MaxHP ? MaxHP : value;
        }

        public List<Shield> currentShield
        {
            get=>shield;
            set=>shield=value;
        }

        public List<DRStatus> CurrentStatus
        {
            get => _statusVariables;
            set => _statusVariables = value;
        }
        

        public abstract int MaxHP { get; set; }
        
    }
}