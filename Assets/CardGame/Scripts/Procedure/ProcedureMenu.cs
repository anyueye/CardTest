using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace CardGame
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;

        private MenuForm _menuForm = null;

        // private 
        public override bool UseNativeDialog
        {
            get => true;
        }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.UI.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;
            GameEntry.Widget.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;
            m_StartGame = false;
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                procedureOwner.SetData<VarInt32>(NEXT_SCENE_ID, (int) SceneId.Main);
                procedureOwner.SetData<VarByte>(GAME_MODE, (byte) GameMode.Normal);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            if (_menuForm == null) return;
            _menuForm.Close(isShutdown);
            _menuForm = null;
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }
            if (ne.UserData.GetType() == typeof(MenuForm))
            {
                _menuForm = (MenuForm) ne.UIForm.Logic;
            }
        }
    }
}