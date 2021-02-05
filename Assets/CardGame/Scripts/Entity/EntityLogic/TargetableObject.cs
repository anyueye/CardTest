using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public abstract class TargetableObject:Entity
    {
        [SerializeField] private TargetableObjectData m_TargetObjectData = null;

        public bool IsDead
        {
            get => m_TargetObjectData.currentHP <= 0;
        }

        public abstract ImpactData GetImpactData();

        public virtual void ApplyDamage(Entity attacker, int damage)
        {
            m_TargetObjectData.currentHP -= damage;
            if (m_TargetObjectData.currentHP<=0)
            {
                OnDead(attacker);
            }
        }
        
        public virtual void HealthHp(Entity healther, int hp)
        {
            m_TargetObjectData.currentHP += hp;
        }
        
        
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_TargetObjectData=userData as TargetableObjectData;
            if (m_TargetObjectData==null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
            
        }
        protected virtual void OnDead(Entity attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }
    }
}