namespace CardGame
{
    public static class CardUtils
    {
        public static bool CardCanBePlayed()
        {
            return true;
        }

        public static bool CardHastargetableEffect(Card c)
        {
            foreach (var effect in c.CardData.CardEffectDatas)
            {
                if (effect.Target==EffectTargetType.TargetEnemy)
                {
                    return true;
                }
            }
            return false;
        }
    }
}