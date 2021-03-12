namespace CardGame
{
    public partial class GameEntry
    {
        // public static BuiltinDataComponent BuiltinData
        // {
        //     get;
        //     private set;
        // }
        public static WidgetComponent Widget
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
            Widget = UnityGameFramework.Runtime.GameEntry.GetComponent<WidgetComponent>();
        }
    }
}