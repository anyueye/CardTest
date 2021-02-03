using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public abstract class GameBase
    {
        
        public abstract GameMode GameMode { get; }


        private PlayerLogic _mPlayerLogic = null;
        
        
        public bool GameOver { get; protected set; }

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameOver = false;


            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 1)
            {
                Position = new Vector3(-3.65f,-1.14f,0),
                LocalScale = Vector3.one*0.6f,
            });

            
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),200)
            {
                Position = new Vector3(3.58f,-1.14f,0),
                LocalScale = Vector3.one*0.6f,
            });
            

            GameOver = false;
            _mPlayerLogic = null;
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSecondes)
        {
            if (_mPlayerLogic == null || !_mPlayerLogic.IsDead) return;
            GameOver = true;
            return;
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }


        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
            if (ne.EntityLogicType == typeof(PlayerLogic))
            {
                _mPlayerLogic = (PlayerLogic) ne.Entity.Logic;
                
            }
        }
    }
}