using UnityGameFramework.Runtime;

namespace CardGame
{
    public class NormalGame:GameBase
    {
        public override GameMode GameMode { get=>GameMode.Normal; }
        public override void Update(float elapseSeconds, float realElapseSecondes)
        {
            base.Update(elapseSeconds, realElapseSecondes);
        }
    }
}