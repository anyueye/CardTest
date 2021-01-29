using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner=GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace CardGame
{
    public class ProcedureGame:ProcedureBase
    {
        
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySceonds = 0f;
        private GameBase m_CurrentGame = null;
        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        
        public override bool UseNativeDialog { get=>true; }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Games.Add(GameMode.Normal,new NormalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            m_Games.Clear();
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
            
           
            m_GotoMenu = false;
            GameMode gameMode = (GameMode) procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();
            
        }
    }
}