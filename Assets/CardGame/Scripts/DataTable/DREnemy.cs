//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-03-26 10:05:55.133
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
    public class DREnemy : DataRowBase
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
        /// 获取名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始生命下限。
        /// </summary>
        public int HpMin
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始生命上限。
        /// </summary>
        public int HpMax
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始攻击下限。
        /// </summary>
        public int AtkMin
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取初始攻击上限。
        /// </summary>
        public int AtkMax
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取默认攻击手段。
        /// </summary>
        public List<int> Intent
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取释放概率。
        /// </summary>
        public List<int> IntentRatio
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
            HpMin = int.Parse(columnStrings[index++]);
            HpMax = int.Parse(columnStrings[index++]);
            AtkMin = int.Parse(columnStrings[index++]);
            AtkMax = int.Parse(columnStrings[index++]);
            Intent = DataTableExtension.ParseInt32List(columnStrings[index++]);
            IntentRatio = DataTableExtension.ParseInt32List(columnStrings[index++]);

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
                    HpMin = binaryReader.Read7BitEncodedInt32();
                    HpMax = binaryReader.Read7BitEncodedInt32();
                    AtkMin = binaryReader.Read7BitEncodedInt32();
                    AtkMax = binaryReader.Read7BitEncodedInt32();
                    Intent = binaryReader.ReadInt32List(3);
                    IntentRatio = binaryReader.ReadInt32List(3);
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
