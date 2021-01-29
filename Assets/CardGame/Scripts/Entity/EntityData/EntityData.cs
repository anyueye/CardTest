using UnityEngine;

namespace CardGame
{
    public class EntityData
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private int m_TypeId = 0;
        [SerializeField]
        private Vector3 m_Position = Vector3.zero;


        public EntityData(int entityId, int typeId)
        {
            m_Id = entityId;
            m_TypeId = typeId;
        }
        /// <summary>
        /// entity 实时 id
        /// </summary>
        public int Id
        {
            get => m_Id;
        }
        /// <summary>
        /// entity 预制体ID
        /// </summary>
        public int TypeId
        {
            get => m_TypeId;
        }
        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }
    }
}