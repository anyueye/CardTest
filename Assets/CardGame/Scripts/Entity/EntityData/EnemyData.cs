using UnityEngine;

namespace CardGame
{
    public class EnemyData:EntityData
    {
        [SerializeField] private string _enemyName;
        [SerializeField] private int _hp;
        [SerializeField] private int _defaultAtk;
        [SerializeField] private int _features;
        public EnemyData(int entityId, int typeId) : base(entityId, typeId)
        {
            var dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            var drEnemy = dtEnemy.GetDataRow(typeId);
            if (drEnemy==null)
            {
                return;
            }
            _enemyName = drEnemy.Name;
            _hp = drEnemy.HP;
            _defaultAtk = drEnemy.Atk;
            _features = drEnemy.Features;
        }

        public string EnemyName
        {
            get => _enemyName;
            set => _enemyName = value;
        }

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public int DefaultAtk
        {
            get => _defaultAtk;
            set => _defaultAtk = value;
        }

        public int Features
        {
            get => _features;
            set => _features = value;
        }
    }
}