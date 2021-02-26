using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class EnemyData:TargetableObjectData
    {
        [SerializeField] private string _enemyName;
        [SerializeField] private int _defaultAtk;
        [SerializeField] private int _shield;
        [SerializeField] private int _maxHp;
        public int Shield
        {
            get => _shield;
            set => _shield = value;
        }

        [SerializeField] private List<int> defaultSkill;
        public EnemyData(int entityId, int typeId) : base(entityId, typeId)
        {
            var dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            var drEnemy = dtEnemy.GetDataRow(typeId);
            if (drEnemy==null)
            {
                return;
            }
            _enemyName = drEnemy.Name;
            _maxHp = Random.Range(drEnemy.HpMin, drEnemy.HpMax + 1);
            _defaultAtk = Random.Range(drEnemy.AtkMin,drEnemy.AtkMax);
            defaultSkill = drEnemy.Skill;
            currentHP=_maxHp;
        }
        
        public void AttachSkill(){}

        public string EnemyName
        {
            get => _enemyName;
            set => _enemyName = value;
        }
        

        public int DefaultAtk
        {
            get => _defaultAtk;
            set => _defaultAtk = value;
        }

        public override int MaxHP { 
            get => _maxHp;
            set => _maxHp = value;
            
        }
    }
}