using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;

namespace CardGame
{
    public class EffectResolutionSystem:GameSystem
    {
        public override void Init()
        {
            base.Init();
            GameEntry.Event.Subscribe(CardSelectionEventArgs.EventId,CardOut);
            GameEntry.Event.Subscribe(EnemySelectionEventArgs.EventId,EnemyAtt);
        }

        private void EnemyAtt(object sender, GameEventArgs e)
        {
            EnemySelectionEventArgs ne = (EnemySelectionEventArgs) e;
            TargetableObject enemy = (TargetableObject) sender;
            ResolveEnemyEffects(enemy,ne.effects);
        }

        public override void Shutdown()
        {
            GameEntry.Event.Unsubscribe(CardSelectionEventArgs.EventId,CardOut);
            GameEntry.Event.Unsubscribe(EnemySelectionEventArgs.EventId,EnemyAtt);
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

        private void ResolveEnemyEffects(TargetableObject instigator,List<Effect> effects)
        {
            foreach (var effect in effects)
            {
                if (effect is TargetableEffect targetableEffect)
                {
                    var targets = GetTargets(targetableEffect.Target, instigator);
                    foreach (var target in targets)
                    {
                        targetableEffect.Resolve(instigator,target);
                    }
                }
                
            }
        }
        
        /// <summary>
        /// 获取目标，如果卡牌为单一目标则为选择到的敌人，否则根据卡牌效果返回目标
        /// </summary>
        /// <param name="target">卡牌效果攻击的目标</param>
        /// <param name="selectedEnemy">选择到的敌人</param>
        /// <returns>所有目标</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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