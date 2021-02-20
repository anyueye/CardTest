using System;
using System.Collections.Generic;

namespace CardGame
{
    public class EffectResolutionSystem:GameSystem
    {
        /// <summary>
        /// 释放有目标卡牌所有效果
        /// </summary>
        /// <param name="c">卡</param>
        /// <param name="target">目标</param>
        public void ResolveCardEffects(Card c,TargetableObject target)
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
        private List<TargetableObject> GetTargets(EffectTargetType target,TargetableObject selectedEnemy)
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
            }
            return targets;
        }
    }
}