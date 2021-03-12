namespace CardGame
{
    public class GainShieldEffect:IntegerEffect
    {
        public GainShieldEffect(int value_,EffectTargetType target_)
        {
            Target = target_;
            Value = value_;
        }

        /// <summary>
        /// 对目标添加护甲
        /// </summary>
        /// <param name="instigator"></param>
        /// <param name="target"></param>
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            ImpactData targetImpactData = target.GetImpactData();
            TargetableObjectData.Shield shield = GainShield(Value);
            target.GainShield(instigator,shield);
        }

        private TargetableObjectData.Shield GainShield(int shield,int duraion=1)
        {
            TargetableObjectData.Shield result = new TargetableObjectData.Shield {value = shield, duration = duraion};
            return result;
        }
    }
}