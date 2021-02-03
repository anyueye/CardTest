//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-02 16:57:23.042
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
        /// 获取。
        /// </summary>
        public string Material
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public string Picture
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public List<int> Effects
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
            Effects = DataTableExtension.ParseList(columnStrings[index++]);

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
                    Effects = binaryReader.ReadList(2);
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
