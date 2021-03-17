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
        [SerializeField] private int _maxHp;
        private readonly int _statusId;

        

        readonly List<int> intentRatio;
        private readonly List<List<Effect>> enemyPattern = new List<List<Effect>>();
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
                List<Effect> intentList = new List<Effect>();
                string effName;
                for (int i = 0; i < skillData.EffectCount&&(effName=skillData.GetEffectAt(i))!="null"; i++)
                {
                    if (effName.Contains("_"))
                    {
                        _statusId = int.Parse(effName.Split('_')[1]);
                        effName = effName.Substring(0, effName.IndexOf('_'));
                    }
                    var eff = Utility.Assembly.GetType($"CardGame.{effName}Effect");
                    var value = skillData.GetValueAt(i);
                    EffectTargetType target = (EffectTargetType) skillData.GetTargetAt(i);
                    IntegerEffect effect;
                    if (eff == typeof(ApplyStatusEffect))
                    {
                        effect = (IntegerEffect) Activator.CreateInstance(eff, value, target,_statusId);
                    }
                    else
                    {
                        effect = (IntegerEffect) Activator.CreateInstance(eff, value, target);
                    }
                    intentList.Add(effect);
                }
                enemyPattern.Add(intentList);
                _intentUI.Add(skillData.Icon);
            }

            intentRatio = drEnemy.IntentRatio;
            currentHP = _maxHp;
        }
        
        public string EnemyName
        {
            get => _enemyName;
            set => _enemyName = value;
        }
        public int StatusId
        {
            get => _statusId;
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
        public List<List<Effect>> EnemyPattern => enemyPattern;
        public List<string> IntentUI => _intentUI;
    }
}