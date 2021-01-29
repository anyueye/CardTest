using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace CardGame
{
    public class ProcedureLaunch:ProcedureBase
    {
        public override bool UseNativeDialog { get=>true; }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }
    }
}