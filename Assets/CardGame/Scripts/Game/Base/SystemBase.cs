namespace CardGame
{
    public class SystemBase
    {
        public SystemBase systemBase;
        
        public virtual void Init(){}

        public virtual void Update(float elapseSeconds, float realElapseSeconds) { }

        public virtual void Shutdown(){}

        
    }
}