using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Resource;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class EnemyLogic:TargetableObject
    {
        [SerializeField] private EnemyData _enemyData = null;
        public EnemyData enemyData => _enemyData;
        public int attackIdx = -1;
        public IFsm<EnemyLogic> enemyFsm;
        public bool prepareAttack = false;
        public bool complateReset = false;
        public override ImpactData GetImpactData()
        {
            
            return new ImpactData(_enemyData.currentHP,_enemyData.DefaultAtk,shieldValue);
        }

        public override void ApplyDamage(Entity attacker, int damage)
        {
            base.ApplyDamage(attacker, damage);
            GameEntry.Widget.ShowHPBar(this,_enemyData.currentHP,_enemyData.MaxHP,0);
        }
        public override void HealthHp(Entity healther, int hp)
        {
            base.HealthHp(healther, hp);
            GameEntry.Widget.ShowHPBar(this,_enemyData.currentHP,_enemyData.MaxHP,0);
        }
        public override void GainShield(Entity entity,TargetableObjectData.Shield shield)
        {
            base.GainShield(entity, shield);
            GameEntry.Widget.ShowHPBar(this,_enemyData.currentHP,_enemyData.MaxHP,shieldValue);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _enemyData=userData as EnemyData;
            if (_enemyData==null)
            {
                Log.Error("EnemyData is invalid.");
                return;
            }
            FsmState<EnemyLogic>[] enemyStates={
                new EnemyResetState(), new EnemyThinkState(), new EnemyAttackState(), 
            };
            enemyFsm = GameEntry.Fsm.CreateFsm(Id.ToString(), this, enemyStates);
            enemyFsm.Start<EnemyThinkState>();
            
            GameEntry.Widget.ShowHPBar(this,_enemyData.currentHP,_enemyData.MaxHP,0);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);
            GameEntry.Widget.HideHPBar(this);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }
    }
    
    /// <summary>
    /// 思考状态，怪物回合开始，选择攻击手段，并结算debuff等
    /// </summary>
    public class EnemyThinkState : FsmState<EnemyLogic>
    {
        
        protected override void OnEnter(IFsm<EnemyLogic> enemyOwner)
        {
            base.OnEnter(enemyOwner);
            var enemyData = enemyOwner.Owner.enemyData;
            
            for (int i = enemyData.currentShield.Count-1; i >=0; i--)
            {
                var temp = enemyData.currentShield[i];
                if (temp.duration==-1)
                {
                    continue;
                }
                temp.duration -= 1;
                enemyData.currentShield[i] = temp;
                if (enemyData.currentShield[i].duration<=0)
                {
                    enemyData.currentShield.Remove(enemyData.currentShield[i]);
                }
            }
            
            
            int intentIdx = Random.GetRatioRandom(enemyData.IntentRatio.ToArray(), 100);
            enemyOwner.Owner.attackIdx = intentIdx;
            GameEntry.Resource.LoadAsset(AssetUtility.GetEnemyIntentsAsset(enemyData.IntentUI[intentIdx]), typeof(Sprite), Constant.AssetPriority.DictionaryAsset,
                new LoadAssetCallbacks(((name, asset, duration, data) =>
                {
                    var icon = asset as Sprite;
                    if (enemyData.EnemyPattern[intentIdx][0] is IntegerEffect targetValue) 
                        GameEntry.Widget.ShowIntent(enemyOwner.Owner, icon, targetValue.Value);
                })));
        }

        protected override void OnUpdate(IFsm<EnemyLogic> enemyOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(enemyOwner, elapseSeconds, realElapseSeconds);
            if (enemyOwner.Owner.prepareAttack)
            {
                ChangeState<EnemyAttackState>(enemyOwner);
                enemyOwner.Owner.prepareAttack = false;
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> enemyOwner, bool isShutdown)
        {
            base.OnLeave(enemyOwner, isShutdown);
        }
    }

    /// <summary>
    /// 攻击状态，释放攻击
    /// </summary>
    public class EnemyAttackState : FsmState<EnemyLogic>
    {
        private float accTime;
        protected override void OnEnter(IFsm<EnemyLogic> enemyOwner)
        {
            
            base.OnEnter(enemyOwner);
            
            accTime = 1f;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> enemyOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(enemyOwner, elapseSeconds, realElapseSeconds);
            accTime -= Time.deltaTime;
            if (!(accTime <= 0)) return;
            var enemy = enemyOwner.Owner;
            var att = enemy.enemyData.EnemyPattern[enemy.attackIdx];
            GameEntry.Widget.HideIntent(enemyOwner.Owner);
            GameEntry.Event.FireNow(enemy,EnemySelectionEventArgs.Create(att));
            enemyOwner.Owner.CachedTransform.DOShakePosition(0.3f);
            ChangeState<EnemyResetState>(enemyOwner);
        }

        protected override void OnLeave(IFsm<EnemyLogic> enemyOwner, bool isShutdown)
        {
            base.OnLeave(enemyOwner, isShutdown);
        }
    }
    
    /// <summary>
    /// 重置状态，攻击后等待其他怪物攻击，都攻击完回到 “思考状态”
    /// </summary>
    public class EnemyResetState : FsmState<EnemyLogic>
    {
        protected override void OnEnter(IFsm<EnemyLogic> enemyOwner)
        {
            base.OnEnter(enemyOwner);
            
        }

        protected override void OnUpdate(IFsm<EnemyLogic> enemyOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(enemyOwner, elapseSeconds, realElapseSeconds);
            
            if (enemyOwner.Owner.complateReset)
            {
                ChangeState<EnemyThinkState>(enemyOwner);
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> enemyOwner, bool isShutdown)
        {
            base.OnLeave(enemyOwner, isShutdown);
        }
    }

    /// <summary>
    /// 怪物攻击事件
    /// </summary>
    public sealed class EnemySelectionEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemySelectionEventArgs).GetHashCode();
        public List<Effect> effects;

        public static EnemySelectionEventArgs Create([NotNull] List<Effect> effects)
        {
            if (effects == null) throw new ArgumentNullException(nameof(effects));
            EnemySelectionEventArgs result = ReferencePool.Acquire<EnemySelectionEventArgs>();
            result.effects = new List<Effect>(effects);
            return result;
        }
        public override void Clear()
        {
            effects.Clear();
        }

        public override int Id => EventId;
    }
    
}