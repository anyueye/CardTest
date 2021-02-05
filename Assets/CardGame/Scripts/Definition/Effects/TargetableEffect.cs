namespace CardGame
{
    public abstract class TargetableEffect:Effect
    {
        public int _value;
        public abstract void Resolve(TargetableObject instigator, TargetableObject target);
    }
}