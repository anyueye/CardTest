using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace CardGame
{
    public class IntentItemObject : ObjectBase
    {
        public static IntentItemObject Create(object target)
        {
            IntentItemObject intentItemObject = ReferencePool.Acquire<IntentItemObject>();
            intentItemObject.Initialize(target);
            return intentItemObject;
        }
    
        protected override void Release(bool isShutdown)
        {
            IntentItem intentItem = (IntentItem) Target;
            if (intentItem==null)
            {
                return;
            }

            Object.Destroy(intentItem.gameObject);
        }
    }
}
