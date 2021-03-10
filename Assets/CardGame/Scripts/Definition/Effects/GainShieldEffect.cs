namespace CardGame
{
    public class GainShieldEffect:IntegerEffect
    {
        public GainShieldEffect(int value_,EffectTargetType target_)
        {
            Target = target_;
            Value = value_;
        }

        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            
        }
    }
}