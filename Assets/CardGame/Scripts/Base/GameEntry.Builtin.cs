﻿using UnityGameFramework.Runtime;

namespace CardGame
{
    public partial class GameEntry
    {
        public static BaseComponent Base { get; private set; }

        public static ConfigComponent Config { get; private set; }

        public static DataNodeComponent DataNode { get; private set; }
        public static DataTableComponent DataTable { get; private set; }
        public static DebuggerComponent Debugger { get; private set; }
        public static DownloadComponent Download { get; private set; }
        public static EntityComponent Entity { get; private set; }
        public static EventComponent Event { get; private set; }
        public static FsmComponent Fsm { get; private set; }
        public static LocalizationComponent Localization { get; private set; }
        public static ObjectPoolComponent ObjectPool { get; private set; }
        public static ProcedureComponent Procedure { get; private set; }
        public static ResourceComponent Resource { get; private set; }
        public static SceneComponent Scene { get; private set; }
        public static SettingComponent Setting { get; private set; }
        public static SoundComponent Sound { get; private set; }
        public static UIComponent UI { get; private set; }

        private static void InitBuiltinComponents()
        {
            Base = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            Config = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigComponent>();
            DataNode = UnityGameFramework.Runtime.GameEntry.GetComponent<DataNodeComponent>();
            DataTable = UnityGameFramework.Runtime.GameEntry.GetComponent<DataTableComponent>();
            Debugger = UnityGameFramework.Runtime.GameEntry.GetComponent<DebuggerComponent>();
            Download = UnityGameFramework.Runtime.GameEntry.GetComponent<DownloadComponent>();
            Entity = UnityGameFramework.Runtime.GameEntry.GetComponent<EntityComponent>();
            Event = UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>();
            Fsm = UnityGameFramework.Runtime.GameEntry.GetComponent<FsmComponent>();
            Localization = UnityGameFramework.Runtime.GameEntry.GetComponent<LocalizationComponent>();
            ObjectPool = UnityGameFramework.Runtime.GameEntry.GetComponent<ObjectPoolComponent>();
            Procedure = UnityGameFramework.Runtime.GameEntry.GetComponent<ProcedureComponent>();
            Resource = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            Scene = UnityGameFramework.Runtime.GameEntry.GetComponent<SceneComponent>();
            Setting = UnityGameFramework.Runtime.GameEntry.GetComponent<SettingComponent>();
            Sound = UnityGameFramework.Runtime.GameEntry.GetComponent<SoundComponent>();
            UI = UnityGameFramework.Runtime.GameEntry.GetComponent<UIComponent>();
        }
    }
}