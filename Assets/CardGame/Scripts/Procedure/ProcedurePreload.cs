﻿using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner=GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace CardGame
{
    public class ProcedurePreload:ProcedureBase
    {
        public override bool UseNativeDialog { get=>true; }

        public static readonly string[] DataTableNames = new[]
        {
            "Cards",
            "Characters",
            "Entity",
            "Enemy",
            "Status",
            "UIForm",        
            "EnemyPattern",
            "Scene"
                      
        };

        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId,OnLoadConfigSuccess);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);

            m_LoadedFlag.Clear();
            
            ProLoadResurouces();
            
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, OnLoadDictionarySuccess);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, OnLoadDictionaryFailure);
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            foreach (var loadflag in m_LoadedFlag)
            {
                if (!loadflag.Value)
                {
                    return;
                }
            }
            procedureOwner.SetData<VarInt32>(NEXT_SCENE_ID, (int)SceneId.Menu);
            procedureOwner.SetData<VarByte>(GAME_MODE, (byte)GameMode.Normal);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private void ProLoadResurouces()
        {
            // LoadConfig("DefaultConfig");
            foreach (var dataTableName in DataTableNames)
            {
                LoadDataTable(dataTableName);
            }
        }

        private void LoadDataTable(string dataTableName)
        {
            string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, false);
            m_LoadedFlag.Add(dataTableAssetName,false);
            GameEntry.DataTable.LoadDataTable(dataTableName,dataTableAssetName,this);
        }

        private void LoadDictionary(string dictionaryName)
        {
            string dictioncryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
            m_LoadedFlag.Add(dictioncryAssetName, false);
            GameEntry.Localization.ReadData(dictioncryAssetName,this);
        }

        private void LoadConfig(string configName)
        {
            string configAssetname = AssetUtility.GetConfigAsset(configName, false);
            m_LoadedFlag.Add(configAssetname, false);
            GameEntry.Config.ReadData(configName,this);
        }

        private void LoadFont(string fontName)
        {
            m_LoadedFlag.Add(Utility.Text.Format("Font.{0}", fontName), false);
            GameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName),Constant.AssetPriority.FontAsset,new LoadAssetCallbacks((assetName, asset, duration, userData) =>
            {
                m_LoadedFlag[Utility.Text.Format("Font.{0}", fontName)] = false;
                Log.Info("Load font '{0}' OK.",fontName);
            },((assetName, status, errorMessage, userData) =>
            {
                Log.Error("Can not load font '{0]' from '{1}' with error message '{2}'.",fontName,assetName,errorMessage);
            })));
        }


        void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs ne = (LoadConfigSuccessEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }
            
            m_LoadedFlag[ne.ConfigAssetName] = true;
            Log.Info($"加载{ne.ConfigAssetName} 成功");
        }

        void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs ne = (LoadConfigFailureEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }
            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", ne.ConfigAssetName, ne.ConfigAssetName, ne.ErrorMessage);

        }

        void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs) e;
            if (ne.UserData!=this)
            {
                return;
            }
            m_LoadedFlag[ne.DataTableAssetName] = true;
            Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
        }
        
        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs ne = (LoadDictionarySuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_LoadedFlag[ne.DictionaryAssetName] = true;
            Log.Info("Load dictionary '{0}' OK.", ne.DictionaryAssetName);
        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs ne = (LoadDictionaryFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load dictionary '{0}' from '{1}' with error message '{2}'.", ne.DictionaryAssetName, ne.DictionaryAssetName, ne.ErrorMessage);
        }
        
    }
    
}