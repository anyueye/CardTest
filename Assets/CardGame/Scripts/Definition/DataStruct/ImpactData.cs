using System.Collections.Generic;

namespace CardGame
{
    public struct ImpactData
    {
        private readonly int _hp;
        private readonly int _damage;
        private readonly int _shield;
        private readonly Dictionary<string, TargetableObjectData.Status> _status;
        
        public ImpactData(int hp, int damage, int shield,Dictionary<string, TargetableObjectData.Status> status)
        {
            _hp = hp;
            _damage = damage;
            _shield = shield;
            _status = status;
        }

        public int Shield => _shield;

        public int Damage => _damage;

        public int Hp => _hp;

        public Dictionary<string, TargetableObjectData.Status> Status => _status;
    }
}