namespace CardGame
{
    public struct ImpactData
    {
        private readonly int _hp;
        private readonly int _attack;
        private readonly int _shield;

        public ImpactData(int hp, int attack, int shield)
        {
            _hp = hp;
            _attack = attack;
            _shield = shield;
        }

        public int Shield => _shield;

        public int Attack => _attack;

        public int Hp => _hp;
    }
}