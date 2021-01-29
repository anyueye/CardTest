using UnityEngine;

namespace CardGame
{
    public class TargetableObject:Entity
    {
        [SerializeField] private TargetableObjectData m_TargetObjectData = null;

        public bool IsDead
        {
            get => m_TargetObjectData.currentHP <= 0;
        }
        
    }
}