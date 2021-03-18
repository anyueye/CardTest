using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class PlayerLogic : TargetableObject
    {
        [SerializeField] private PlayerData m_PlayerData = null;

        public PlayerData PlayerData
        {
            get => m_PlayerData;
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
        }


        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_PlayerData.currentHP,0,GetShieldValue(),m_PlayerData.CurrentStatus);
        }

        // public override void ApplyDamage(Entity attacker, int damage)
        // {
        //     base.ApplyDamage(attacker, damage);
        //     GameEntry.Widget.ShowHPBar(this,m_PlayerData.currentHP,m_PlayerData.MaxHP,0);
        // }

        // public override void HealthHp(Entity healther, int hp)
        // {
        //     base.HealthHp(healther, hp);
        //     GameEntry.Widget.ShowHPBar(this,m_PlayerData.currentHP,m_PlayerData.MaxHP,0);
        // }
        //
        // public override void GainShield(Entity entity, TargetableObjectData.Shield shield)
        // {
        //     base.GainShield(entity, shield);
        //     GameEntry.Widget.ShowHPBar(this,m_PlayerData.currentHP,m_PlayerData.MaxHP,shieldValue);
        // }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            // m_PlayerData = userData as PlayerData;
            // if (m_PlayerData==null)
            // {
            //     return;
            // }
            // GameEntry.Widget.ShowHPBar(this,m_PlayerData.currentHP,m_PlayerData.MaxHP,0);
        }
    }
}