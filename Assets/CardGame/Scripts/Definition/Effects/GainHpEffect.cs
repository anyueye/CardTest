using UnityEngine;

namespace CardGame
{
    public class GainHpEffect:IntegerEffect
    {
        public GainHpEffect(int value,EffectTargetType target_)
        {
            Target = target_;
            Value = value;
        }

        /// <summary>
        /// 对目标回血
        /// </summary>
        /// <param name="instigator">自己</param>
        /// <param name="target">目标</param>
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            Debug.Log("GainHp");
            ImpactData targetImpactData = target.GetImpactData();
            int damage = HealthHp(Value);
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