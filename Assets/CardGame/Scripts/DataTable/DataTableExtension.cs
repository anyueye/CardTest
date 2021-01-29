using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public static class DataTableExtension
    {
        private const string DataRowClassPrefixName = "CardGame.DR";
        internal static readonly char[] DataSplitSeparators = new char[] {'\t'};
        internal static readonly char[] DataTrimSeparators = new char[] {'\"'};

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, string dataTableAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitedNames = dataTableName.Split('_');
            if (splitedNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataRowClassName = DataRowClassPrefixName + splitedNames[0];
            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            string name = splitedNames.Length > 1 ? splitedNames[1] : null;
            DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
            dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
        }
        public static List<int> ParseList(string value)
        {
            string[] splitedValue = value.Split(',');
            List<int> temp = new List<int>();
            foreach (var t in splitedValue)
            {
                temp.Add(int.Parse(t));;
            }
            return temp;
        }
    }
}