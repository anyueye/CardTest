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
        [SerializeField] private List<int> defaultCards = new List<int>();

        [SerializeField] private int m_MaxHP=0;
        [SerializeField] private int m_Defense = 0;
        /// <summary>
        /// 当前局所有牌库
        /// </summary>
        public List<int> allCards = new List<int>();
        public PlayerData(int entityId, int typeId) : base(entityId, typeId)
        {
            var dtPlayer = GameEntry.DataTable.GetDataTable<DRCharacters>();
            var drPlayer = dtPlayer.GetDataRow(typeId);
            if (drPlayer==null)
            {
                return;
            }
            RefreshData(drPlayer);
            defaultCards.AddRange(drPlayer.DefaultCards);
            allCards.AddRange(defaultCards);
            currentHP = m_MaxHP;
        }

        /// <summary>
        /// 初始手牌上限
        /// </summary>
        public int HandLimit
        {
            get => handLimit;
            set => handLimit = value;
        }

        /// <summary>
        /// 最大HP
        /// </summary>
        public override int MaxHP
        {
            get => m_MaxHP;
            set => m_MaxHP = value;
        }
        
        /// <summary>
        /// 初始MP
        /// </summary>
        public int DefaultMp
        {
            get => defaultMp;
            set => defaultMp = value;
        }

        // public int DefaultPhysical
        // {
        //     get => defaultPhysical;
        //     set => defaultPhysical = value;
        // }
        /// <summary>
        /// 初始卡组
        /// </summary>
        public List<int> DefaultCards
        {
            get => defaultCards;
            set => defaultCards = value;
        }

        private void RefreshData(DRCharacters pd)
        {
            m_MaxHP = 0;
            m_Defense = 0;
            m_MaxHP += pd.HP;
            if (currentHP>m_MaxHP)
            {
                currentHP = m_MaxHP;
            }
        }
    }
}