//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-01-29 15:43:10.988
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
    public class DRCharacters : DataRowBase
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
        /// 获取职业。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取生命。
        /// </summary>
        public int HP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取魔法。
        /// </summary>
        public int MP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取体力。
        /// </summary>
        public int Physical
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取手牌上限。
        /// </summary>
        public int HandLimit
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取默认卡组。
        /// </summary>
        public List<int> DefaultCards
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
            HP = int.Parse(columnStrings[index++]);
            MP = int.Parse(columnStrings[index++]);
            Physical = int.Parse(columnStrings[index++]);
            HandLimit = int.Parse(columnStrings[index++]);
            DefaultCards = DataTableExtension.ParseList(columnStrings[index++]);

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
                    HP = binaryReader.Read7BitEncodedInt32();
                    MP = binaryReader.Read7BitEncodedInt32();
                    Physical = binaryReader.Read7BitEncodedInt32();
                    HandLimit = binaryReader.Read7BitEncodedInt32();
                    DefaultCards = binaryReader.ReadList(4);
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
