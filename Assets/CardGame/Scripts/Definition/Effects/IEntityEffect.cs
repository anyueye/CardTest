namespace CardGame
{
    public interface IEntityEffect
    {
        void Resolve(TargetableObjectData instigator, TargetableObjectData target);
    }
}