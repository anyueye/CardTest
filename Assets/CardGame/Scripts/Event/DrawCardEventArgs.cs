using GameFramework;
using GameFramework.Event;

namespace CardGame
{
    public class DrawnCardEventArgs:GameEventArgs
    {
        public static int EventId => typeof(DrawnCardEventArgs).GetHashCode();

        public DrawnCardEventArgs()
        {
            drawCount = 0;
        }
        public int drawCount;

        public static DrawnCardEventArgs Create(int count)
        {
            DrawnCardEventArgs result = ReferencePool.Acquire<DrawnCardEventArgs>();
            result.drawCount = count;
            return result;
        }
        public override void Clear()
        {
            drawCount = 0;
        }

        public override int Id => EventId;
    }
}