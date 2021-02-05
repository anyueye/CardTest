namespace CardGame
{
    public class DealDamageEffect:TargetableEffect
    {

        public DealDamageEffect(int value)
        {
            _value = value;
        }
        
        /// <summary>
        /// 对目标造成伤害
        /// </summary>
        /// <param name="instigator">自己</param>
        /// <param name="target">目标</param>
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            ImpactData targetImpactData = target.GetImpactData();
            int damage = CalcDamageHp(_value, targetImpactData.Shield);
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