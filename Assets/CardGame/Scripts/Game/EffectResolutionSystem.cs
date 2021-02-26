using System;
using System.Collections.Generic;
using GameFramework.Event;

namespace CardGame
{
    public class EffectResolutionSystem:GameSystem
    {
        public override void Init()
        {
            base.Init();
            GameEntry.Event.Subscribe(CardSelectionEventArgs.EventId,CardOut);
        }

        public override void Shutdown()
        {
            GameEntry.Event.Unsubscribe(CardSelectionEventArgs.EventId,CardOut);
            base.Shutdown();
        }

        private void CardOut(object sender, GameEventArgs e)
        {
            CardSelectionEventArgs ne = (CardSelectionEventArgs) e;
            ResolveCardEffects(ne.selectCard,ne.selectTarget);
        }


        /// <summary>
        /// 释放有目标卡牌所有效果
        /// </summary>
        /// <param name="c">卡</param>
        /// <param name="target">目标</param>
        private void ResolveCardEffects(Card c,TargetableObject target)
        {
            //获取当前卡牌所有行为，每个行为单独释放效果
            foreach (var effect in c.CardData.Effects)
            {
                //获取当前卡牌释放目标
                var targets = GetTargets(effect.Target, target);
                foreach (var entity in targets)
                {
                    effect.Resolve(player,entity);
                }
            }
        }
        private IEnumerable<TargetableObject> GetTargets(EffectTargetType target,TargetableObject selectedEnemy)
        {
            var targets=new List<TargetableObject>(4);
            switch (@target)
            {
                case EffectTargetType.Self:
                    targets.Add(player);
                    break;
                case EffectTargetType.TargetEnemy:
                    targets.Add(selectedEnemy);
                    break;
                case EffectTargetType.AllEnemies:
                    targets.AddRange(enemys);
                    break;
                case EffectTargetType.All:
                    targets.Add(player);
                    targets.AddRange(enemys);
                    break;
                case EffectTargetType.RandomEnemy:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), @target, null);
            }
            return targets;
        }
    }
}