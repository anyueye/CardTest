namespace CardGame
{
    public struct ImpactData
    {
        private readonly int _hp;
        private readonly int _damage;
        private readonly int _shield;

        public ImpactData(int hp, int damage, int shield)
        {
            _hp = hp;
            _damage = damage;
            _shield = shield;
        }

        public int Shield => _shield;

        public int Damage => _damage;

        public int Hp => _hp;
    }
}