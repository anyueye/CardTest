using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class EnemyLogic:TargetableObject
    {
        [SerializeField] private EnemyData _enemyData = null;
        public override ImpactData GetImpactData()
        {
            return new ImpactData(_enemyData.currentHP,_enemyData.DefaultAtk,_enemyData.Shield);
        }

        public override void ApplyDamage(Entity attacker, int damage)
        {
            base.ApplyDamage(attacker, damage);
            GameEntry.hpBar.ShowHPBar(this,_enemyData.currentHP,0);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _enemyData=userData as EnemyData;
            if (_enemyData==null)
            {
                Log.Error("EnemyData is invalid.");
                return;
            }
            GameEntry.hpBar.ShowHPBar(this,_enemyData.MaxHP,0);
        }
        
        
    }
}