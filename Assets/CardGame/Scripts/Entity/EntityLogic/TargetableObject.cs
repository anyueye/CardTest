using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public abstract class TargetableObject:Entity
    {
        [SerializeField] private TargetableObjectData m_TargetObjectData = null;
        protected int shieldValue = 0;
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

        public virtual void GainShield(Entity entity,TargetableObjectData.Shield shield)
        {
            m_TargetObjectData.currentShield.Add(shield);
            shieldValue = 0;
            for (int i = 0; i < m_TargetObjectData.currentShield.Count; i++)
            {
                shieldValue += m_TargetObjectData.currentShield[i].value;
            }
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