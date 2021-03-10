using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame
{
    public class EnemyData : TargetableObjectData
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

        readonly List<int> intentRatio;
        private readonly List<List<IntegerEffect>> enemyPattern = new List<List<IntegerEffect>>();
        readonly List<string> _intentUI = new List<string>();

        public EnemyData(int entityId, int typeId) : base(entityId, typeId)
        {
            var dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            var drEnemy = dtEnemy.GetDataRow(typeId);
            if (drEnemy == null)
            {
                return;
            }

            _enemyName = drEnemy.Name;
            _maxHp = Utility.Random.GetRandom(drEnemy.HpMin, drEnemy.HpMax + 1);
            _defaultAtk = Utility.Random.GetRandom(drEnemy.AtkMin, drEnemy.AtkMax);
            var dtEnemyPattern = GameEntry.DataTable.GetDataTable<DREnemyPattern>();
            
            foreach (var t in drEnemy.Intent)
            {
                var skillData = dtEnemyPattern.GetDataRow(t);
                List<IntegerEffect> intentList = new List<IntegerEffect>();
                string effName;
                for (int i = 0; i < skillData.EffectCount&&(effName=skillData.GetEffectAt(i))!="null"; i++)
                {
                    var eff = Utility.Assembly.GetType($"CardGame.{effName}");
                    var value = skillData.GetValueAt(i);
                    EffectTargetType target = (EffectTargetType) skillData.GetTargetAt(i);
                    var effect = (IntegerEffect) Activator.CreateInstance(eff, value, target);
                    intentList.Add(effect);
                }
                enemyPattern.Add(intentList);
                _intentUI.Add(skillData.Icon);
            }

            intentRatio = drEnemy.IntentRatio;
            currentHP = _maxHp;
        }

        public void AttachSkill()
        {
        }

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

        public override int MaxHP
        {
            get => _maxHp;
            set => _maxHp = value;
        }
        public List<int> IntentRatio => intentRatio;
        public List<List<IntegerEffect>> EnemyPattern => enemyPattern;
        public List<string> IntentUI => _intentUI;
    }
}