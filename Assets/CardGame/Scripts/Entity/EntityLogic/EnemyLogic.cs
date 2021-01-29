using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class EnemyLogic:Entity
    {
        [SerializeField] private EnemyData _enemyData = null;
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _enemyData=userData as EnemyData;
            if (_enemyData==null)
            {
                Log.Error("EnemyData is invalid.");
                return;
            }
            
        }
    }
}