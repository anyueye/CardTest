namespace CardGame
{
    public class ApplyStatus:IntegerEffect
    {
        
        public ApplyStatus(int value_, EffectTargetType target_, int templateId)
        {
            Value = value_;
            Target = target_;
        }
        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            
        }
    }
}