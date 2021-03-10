using GameFramework;
using GameFramework.Fsm;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class EnemyLogic:TargetableObject
    {
        [SerializeField] private EnemyData _enemyData = null;
        public EnemyData enemyData => _enemyData;
        public IFsm<EnemyLogic> enemyFsm;
        public bool prepareAttack = false;
        public bool complateReset = false;
        public override ImpactData GetImpactData()
        {
            return new ImpactData(_enemyData.currentHP,_enemyData.DefaultAtk,_enemyData.Shield);
        }

        public override void ApplyDamage(Entity attacker, int damage)
        {
            base.ApplyDamage(attacker, damage);
            GameEntry.hpBar.ShowHPBar(this,_enemyData.currentHP,0);
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
            
            GameEntry.hpBar.ShowHPBar(this,_enemyData.MaxHP,0);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }
    }

    public class EnemyThinkState : FsmState<EnemyLogic>
    {
        
        protected override void OnEnter(IFsm<EnemyLogic> enemyOwner)
        {
            base.OnEnter(enemyOwner);
            var enemyData = enemyOwner.Owner.enemyData;
            int intentIdx = Random.GetRatioRandom(enemyData.IntentRatio.ToArray(), 100);
            
            GameEntry.Resource.LoadAsset(AssetUtility.GetEnemyIntentsAsset(enemyData.IntentUI[intentIdx]), typeof(Sprite), Constant.AssetPriority.DictionaryAsset,
                new LoadAssetCallbacks(((name, asset, duration, data) =>
                {
                    var icon = asset as Sprite;
                    var targetValue = enemyData.EnemyPattern[intentIdx][0].Value;
                    
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

    public class EnemyAttackState : FsmState<EnemyLogic>
    {
        protected override void OnEnter(IFsm<EnemyLogic> enemyOwner)
        {
            
            base.OnEnter(enemyOwner);
            
        }

        protected override void OnUpdate(IFsm<EnemyLogic> enemyOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(enemyOwner, elapseSeconds, realElapseSeconds);
            
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                ChangeState<EnemyResetState>(enemyOwner);
            }
        }

        protected override void OnLeave(IFsm<EnemyLogic> enemyOwner, bool isShutdown)
        {
            base.OnLeave(enemyOwner, isShutdown);
        }
    }
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
}