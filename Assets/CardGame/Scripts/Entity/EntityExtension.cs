using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace CardGame
{
    public static class EntityExtension
    {
        private static int s_SerialId = 0;

        public static void ShowPlayer(this EntityComponent entityComponent, PlayerData data)
        {
            entityComponent.ShowEntity(typeof(PlayerLogic), "Player", Constant.AssetPriority.Player, data);
        }

        public static void ShowEnemy(this EntityComponent entityComponent, EnemyData data)
        {
            entityComponent.ShowEntity(typeof(EnemyLogic), "Enemy", Constant.AssetPriority.Monster, data);
        }
        
        

        public static void ShowCard(this EntityComponent entityComponent, CardData data)
        {
            entityComponent.ShowEntity(typeof(Card), "Card", Constant.AssetPriority.Card, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }

    static class RandomExt
    {
        /// <summary>
        /// 固定数组中的不重复随机
        /// </summary>
        /// <param name="nums">数组</param>
        /// <param name="count">要随机的个数</param>
        /// <returns></returns>
        public static List<T> GetRandom<T>(this List<T> nums, int count)
        {
            if (count > nums.Count)
            {
                Debug.LogError("要取的个数大于数组长度！");
                return null;
            }

            List<T> result = new List<T>();
            List<int> id = new List<int>();

            for (int i = 0; i < nums.Count; i++)
            {
                id.Add(i);
            }

            while (id.Count > nums.Count - count)
            {
                var r = Random.Range(0, id.Count);
                result.Add(nums[id[r]]);
                id.Remove(id[r]);
            }

            return (result);
        }
    }
}