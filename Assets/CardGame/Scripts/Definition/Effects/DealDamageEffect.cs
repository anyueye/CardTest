using UnityEngine;

namespace CardGame
{
    public class DealDamageEffect:IntegerEffect
    {

        public DealDamageEffect(int value,EffectTargetType target_)
        {
            Target = target_;
            Value = value;
        }
        
        /// <summary>
        /// 对目标造成伤害
        /// </summary>
        /// <param name="instigator">发起者</param>
        /// <param name="target">目标</param>
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            Debug.Log("Deal");
            ImpactData targetImpactData = target.GetImpactData();
            int damage = CalcDamageHp(Value, targetImpactData.Shield);
            target.ApplyDamage(instigator,damage);
        }

        private int CalcDamageHp(int attack, int shield)
        {
            int result = 0;
            if (attack<0)
            {
                return  0;
            }

            result = attack - shield;
            return result;
        }
    }
}