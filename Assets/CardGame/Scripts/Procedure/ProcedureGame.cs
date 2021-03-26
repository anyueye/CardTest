using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace CardGame
{
    public class ProcedureGame : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySceonds = 0f;
        private GameBase m_CurrentGame = null;
        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();

        public override bool UseNativeDialog
        {
            get => true;
        }


        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Normal, new NormalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            m_Games.Clear();
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;
            GameEntry.Widget.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;

            m_GotoMenu = false;
            GameMode gameMode = (GameMode) procedureOwner.GetData<VarByte>(GAME_MODE).Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_CurrentGame?.Shutdown();

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);

                return;
            }

            if (!m_GotoMenu)
            {
                m_GotoMenu = true;
                m_GotoMenuDelaySceonds = 0;
            }

            m_GotoMenuDelaySceonds += elapseSeconds;
            if (m_GotoMenuDelaySceonds >= GameOverDelayedSeconds)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}