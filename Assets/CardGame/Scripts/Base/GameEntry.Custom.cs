namespace CardGame
{
    public partial class GameEntry
    {
        // public static BuiltinDataComponent BuiltinData
        // {
        //     get;
        //     private set;
        // }
        public static HPBarComponent hpBar
        {
            get;
            private set;
        }
        public static TargetingArrow targetingArrow
        {
            get;
            private set;
        }

        private static void InitCustomCompontns()
        {
            targetingArrow = UnityGameFramework.Runtime.GameEntry.GetComponent<TargetingArrow>();
            hpBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
        }
    }
}