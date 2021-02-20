﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-02-19 16:32:42.167
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
    public class DREnemyPattern : DataRowBase
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
        /// 获取特性名称。
        /// </summary>
        public string PatternName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取特性图片id。
        /// </summary>
        public string Icon
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取数值。
        /// </summary>
        public int Value
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public string Effect
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public List<int> SourceActions
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public List<int> TargetActions
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
            PatternName = columnStrings[index++];
            Icon = columnStrings[index++];
            Value = int.Parse(columnStrings[index++]);
            Effect = columnStrings[index++];
            SourceActions = DataTableExtension.ParseList(columnStrings[index++]);
            TargetActions = DataTableExtension.ParseList(columnStrings[index++]);

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
                    PatternName = binaryReader.ReadString();
                    Icon = binaryReader.ReadString();
                    Value = binaryReader.Read7BitEncodedInt32();
                    Effect = binaryReader.ReadString();
                    SourceActions = binaryReader.ReadList(3);
                    TargetActions = binaryReader.ReadList(3);
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
