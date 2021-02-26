using GameFramework;
using GameFramework.Event;

namespace CardGame
{
    public sealed class EventTest:GameEventArgs
    {
        public static int EventId => typeof(EventTest).GetHashCode();
        public int t0, t1;
        public EventTest()
        {
            
        }
        public static EventTest Create0(int _t0)
        {
            EventTest test = ReferencePool.Acquire<EventTest>();
            test.t0 = _t0;
            return test;
        }
        public static EventTest Create1(int _t1)
        {
            EventTest test = ReferencePool.Acquire<EventTest>();
            test.t1 = _t1;
            return test;
        }
        public override void Clear()
        {
            
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
    }
}