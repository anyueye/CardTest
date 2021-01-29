using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

namespace CardGame
{
    public class PlayerData : TargetableObjectData
    {
        [SerializeField] private int handLimit = 0;
        [SerializeField] private int defaultHp = 0;
        [SerializeField] private int defaultMp = 0;
        [SerializeField] private int defaultPhysical = 0;
        [SerializeField] private List<int> defaultCards = new List<int>();
        /// <summary>
        /// 当前局所有牌库
        /// </summary>
        public List<int> allCards = new List<int>();
        public PlayerData(int entityId, int typeId) : base(entityId, typeId, CampType.Player)
        {
            var dtPlayer = GameEntry.DataTable.GetDataTable<DRCharacters>();
            var drPlayer = dtPlayer.GetDataRow(typeId);
            if (drPlayer==null)
            {
                return;
            }
            handLimit = drPlayer.HandLimit;
            defaultHp = drPlayer.HP;
            defaultMp = drPlayer.MP;
            defaultPhysical = drPlayer.Physical;
            defaultCards = drPlayer.DefaultCards;
            allCards.AddRange(defaultCards);
        }

        public int HandLimit
        {
            get => handLimit;
            set => handLimit = value;
        }

        public override int MaxHP
        {
            get => defaultHp;
            set => defaultHp = value;
        }

        public int DefaultMp
        {
            get => defaultMp;
            set => defaultMp = value;
        }

        public int DefaultPhysical
        {
            get => defaultPhysical;
            set => defaultPhysical = value;
        }

        public List<int> DefaultCards
        {
            get => defaultCards;
            set => defaultCards = value;
        }
    }
}