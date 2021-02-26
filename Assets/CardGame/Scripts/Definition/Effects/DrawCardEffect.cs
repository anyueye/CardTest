using UnityEngine;

namespace CardGame
{
    public class DrawCardEffect:IntegerEffect
    {
        public DrawCardEffect(int value_,EffectTargetType target_)
        {
            Value = value_;
            Target = target_;
        }

        public override void Resolve(TargetableObject instigator, TargetableObject target)
        {
            GameEntry.Event.FireNow(this,DrawnCardEventArgs.Create(Value));
        }
    }
}