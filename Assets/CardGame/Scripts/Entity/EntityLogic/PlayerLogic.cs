using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class PlayerLogic : TargetableObject
    {
        [SerializeField] private PlayerData m_PlayerData = null;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
        }
    }
}