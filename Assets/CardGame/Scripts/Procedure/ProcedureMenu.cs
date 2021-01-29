using ProcedureOwner=GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace CardGame
{
    public class ProcedureMenu:ProcedureBase
    {
        private bool m_StartGame = false;
        public override bool UseNativeDialog { get => true; }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
        }
    }
}