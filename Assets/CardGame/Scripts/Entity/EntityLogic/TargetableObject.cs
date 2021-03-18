using System.Linq;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public abstract class TargetableObject : Entity
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
            if (m_TargetObjectData.currentHP <= 0)
            {
                OnDead(attacker);
            }
            
            GameEntry.Widget.ShowHPBar(this,m_TargetObjectData.currentHP,m_TargetObjectData.MaxHP,GetShieldValue());
        }

        public virtual void HealthHp(Entity healther, int hp)
        {
            m_TargetObjectData.currentHP += hp;
            GameEntry.Widget.ShowHPBar(this,m_TargetObjectData.currentHP,m_TargetObjectData.MaxHP,GetShieldValue());
        }

        public virtual void GainShield(Entity entity, TargetableObjectData.Shield shield)
        {
            m_TargetObjectData.currentShield.Add(shield);
            GameEntry.Widget.ShowHPBar(this,m_TargetObjectData.currentHP,m_TargetObjectData.MaxHP,GetShieldValue());
        }

        public virtual void GainStatus(Entity entity, TargetableObjectData.Status status)
        {
            if (m_TargetObjectData.CurrentStatus.ContainsKey(status.drStatus.FeaturesName))
            {
                var s = m_TargetObjectData.CurrentStatus[status.drStatus.FeaturesName];
                s.duration += 1;
            }
            else
            {
                m_TargetObjectData.CurrentStatus.Add(status.drStatus.FeaturesName, status);
            }
        }

        protected int GetShieldValue()
        {
            int shieldValue = 0;
            for (int i = 0; i < m_TargetObjectData.currentShield.Count; i++)
            {
                shieldValue += m_TargetObjectData.currentShield[i].value;
            }

            return shieldValue;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_TargetObjectData = userData as TargetableObjectData;
            if (m_TargetObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
            GameEntry.Widget.ShowHPBar(this,m_TargetObjectData.currentHP,m_TargetObjectData.MaxHP,GetShieldValue());
        }

        protected virtual void OnDead(Entity attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }
    }
}