using UnityEngine;

namespace CardGame
{
    public class GainHpEffect:TargetableEffect
    {
        public GainHpEffect(int value)
        {
            _value = value;
        }

        /// <summary>
        /// 对目标回血
        /// </summary>
        /// <param name="instigator">自己</param>
        /// <param name="target">目标</param>
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            ImpactData targetImpactData = target.GetImpactData();
            int damage = HealthHp(_value);
            target.ApplyDamage(instigator,damage);
        }
        
        private int HealthHp(int health)
        {
            int result = 0;
            result = health;
            return result;
        }
    }
}