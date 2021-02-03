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
            foreach (var effect in c.CardData.Target)
            {
                if (effect==CardData.CardTarget.Target)
                {
                    return true;
                }
            }
            return false;
        }
    }
}