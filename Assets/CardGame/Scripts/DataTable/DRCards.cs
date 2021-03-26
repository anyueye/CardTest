//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-03-26 10:05:55.109
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    /// <summary>
    /// 测试。
    /// </summary>
    public class DRCards : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取卡片名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取费用。
        /// </summary>
        public int Cost
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取0=伤害卡，1=buff卡，2技能卡。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡牌外观。
        /// </summary>
        public string Material
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡牌图标。
        /// </summary>
        public string Picture
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取描述。
        /// </summary>
        public string Describe
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡牌包含效果。
        /// </summary>
        public string Effect0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取0=玩家，1=敌人，2=随机敌人，3=全部敌人，4=所有人包括自己。
        /// </summary>
        public int Target0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数值或系数或回合数。
        /// </summary>
        public int Value0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对自己特效。
        /// </summary>
        public List<int> SourceActions0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对目标特效。
        /// </summary>
        public List<int> TargetActions0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡牌包含效果。
        /// </summary>
        public string Effect1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取0=玩家，1=敌人，2=随机敌人，3=全部敌人，4=所有人包括自己。
        /// </summary>
        public int Target1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数值或系数或回合数。
        /// </summary>
        public int Value1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对自己特效。
        /// </summary>
        public List<int> SourceActions1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对目标特效。
        /// </summary>
        public List<int> TargetActions1
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            Cost = int.Parse(columnStrings[index++]);
            Type = int.Parse(columnStrings[index++]);
            Material = columnStrings[index++];
            Picture = columnStrings[index++];
            Describe = columnStrings[index++];
            Effect0 = columnStrings[index++];
            Target0 = int.Parse(columnStrings[index++]);
            Value0 = int.Parse(columnStrings[index++]);
            SourceActions0 = DataTableExtension.ParseInt32List(columnStrings[index++]);
            TargetActions0 = DataTableExtension.ParseInt32List(columnStrings[index++]);
            Effect1 = columnStrings[index++];
            Target1 = int.Parse(columnStrings[index++]);
            Value1 = int.Parse(columnStrings[index++]);
            SourceActions1 = DataTableExtension.ParseInt32List(columnStrings[index++]);
            TargetActions1 = DataTableExtension.ParseInt32List(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    Cost = binaryReader.Read7BitEncodedInt32();
                    Type = binaryReader.Read7BitEncodedInt32();
                    Material = binaryReader.ReadString();
                    Picture = binaryReader.ReadString();
                    Describe = binaryReader.ReadString();
                    Effect0 = binaryReader.ReadString();
                    Target0 = binaryReader.Read7BitEncodedInt32();
                    Value0 = binaryReader.Read7BitEncodedInt32();
                    SourceActions0 = binaryReader.ReadInt32List(0);
                    TargetActions0 = binaryReader.ReadInt32List(0);
                    Effect1 = binaryReader.ReadString();
                    Target1 = binaryReader.Read7BitEncodedInt32();
                    Value1 = binaryReader.Read7BitEncodedInt32();
                    SourceActions1 = binaryReader.ReadInt32List(0);
                    TargetActions1 = binaryReader.ReadInt32List(0);
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, string>[] m_Effect = null;

        public int EffectCount
        {
            get
            {
                return m_Effect.Length;
            }
        }

        public string GetEffect(int id)
        {
            foreach (KeyValuePair<int, string> i in m_Effect)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetEffect with invalid id '{0}'.", id.ToString()));
        }

        public string GetEffectAt(int index)
        {
            if (index < 0 || index >= m_Effect.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetEffectAt with invalid index '{0}'.", index.ToString()));
            }

            return m_Effect[index].Value;
        }

        private KeyValuePair<int, int>[] m_Target = null;

        public int TargetCount
        {
            get
            {
                return m_Target.Length;
            }
        }

        public int GetTarget(int id)
        {
            foreach (KeyValuePair<int, int> i in m_Target)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetTarget with invalid id '{0}'.", id.ToString()));
        }

        public int GetTargetAt(int index)
        {
            if (index < 0 || index >= m_Target.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetTargetAt with invalid index '{0}'.", index.ToString()));
            }

            return m_Target[index].Value;
        }

        private KeyValuePair<int, int>[] m_Value = null;

        public int ValueCount
        {
            get
            {
                return m_Value.Length;
            }
        }

        public int GetValue(int id)
        {
            foreach (KeyValuePair<int, int> i in m_Value)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetValue with invalid id '{0}'.", id.ToString()));
        }

        public int GetValueAt(int index)
        {
            if (index < 0 || index >= m_Value.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetValueAt with invalid index '{0}'.", index.ToString()));
            }

            return m_Value[index].Value;
        }

        private KeyValuePair<int, List<int>>[] m_SourceActions = null;

        public int SourceActionsCount
        {
            get
            {
                return m_SourceActions.Length;
            }
        }

        public List<int> GetSourceActions(int id)
        {
            foreach (KeyValuePair<int, List<int>> i in m_SourceActions)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetSourceActions with invalid id '{0}'.", id.ToString()));
        }

        public List<int> GetSourceActionsAt(int index)
        {
            if (index < 0 || index >= m_SourceActions.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetSourceActionsAt with invalid index '{0}'.", index.ToString()));
            }

            return m_SourceActions[index].Value;
        }

        private KeyValuePair<int, List<int>>[] m_TargetActions = null;

        public int TargetActionsCount
        {
            get
            {
                return m_TargetActions.Length;
            }
        }

        public List<int> GetTargetActions(int id)
        {
            foreach (KeyValuePair<int, List<int>> i in m_TargetActions)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetTargetActions with invalid id '{0}'.", id.ToString()));
        }

        public List<int> GetTargetActionsAt(int index)
        {
            if (index < 0 || index >= m_TargetActions.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetTargetActionsAt with invalid index '{0}'.", index.ToString()));
            }

            return m_TargetActions[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_Effect = new KeyValuePair<int, string>[]
            {
                new KeyValuePair<int, string>(0, Effect0),
                new KeyValuePair<int, string>(1, Effect1),
            };

            m_Target = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, Target0),
                new KeyValuePair<int, int>(1, Target1),
            };

            m_Value = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, Value0),
                new KeyValuePair<int, int>(1, Value1),
            };

            m_SourceActions = new KeyValuePair<int, List<int>>[]
            {
                new KeyValuePair<int, List<int>>(0, SourceActions0),
                new KeyValuePair<int, List<int>>(1, SourceActions1),
            };

            m_TargetActions = new KeyValuePair<int, List<int>>[]
            {
                new KeyValuePair<int, List<int>>(0, TargetActions0),
                new KeyValuePair<int, List<int>>(1, TargetActions1),
            };
        }
    }
}
