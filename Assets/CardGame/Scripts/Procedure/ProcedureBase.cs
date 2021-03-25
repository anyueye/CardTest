using ProcedureOwner=GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace CardGame
{
    public abstract class ProcedureBase:GameFramework.Procedure.ProcedureBase
    {
        protected const string NEXT_SCENE_ID = "NextSceneID";
        protected const string GAME_MODE = "GameMode";
        public abstract bool UseNativeDialog
        {
            get;
        }
    }
}