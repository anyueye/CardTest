namespace CardGame
{
    public abstract class TargetableEffect:Effect
    {
        public EffectTargetType Target;
        public abstract void Resolve(TargetableObject instigator, TargetableObject target);
    }
}