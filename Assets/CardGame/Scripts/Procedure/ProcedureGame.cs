using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace CardGame
{
    public class ProcedureGame : ProcedureBase
    {
        private GameForm m_GameForm = null;
        
        
        private const float GameOverDelayedSeconds = 2f;

        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySceonds = 0f;
        private GameBase m_CurrentGame = null;
        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private readonly List<GameSystem> _system = new List<GameSystem>();

        private CardSelectionSystem _cardSelectionSystem;
        private DeckDrawingSystem _deckDrawingSystem;
        private HandPresentationSystem _handPresentationSystem;

        public override bool UseNativeDialog
        {
            get => true;
        }


        public List<int> staringDeck = new List<int>() {1000, 1000, 1001, 1002};

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            
            m_Games.Add(GameMode.Normal, new NormalGame());
            
            _cardSelectionSystem=new CardSelectionSystem();
            
            _deckDrawingSystem = new DeckDrawingSystem();
            _handPresentationSystem = new HandPresentationSystem();
            _system.Add(_cardSelectionSystem);
            _system.Add(_deckDrawingSystem);
            _system.Add(_handPresentationSystem);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            m_Games.Clear();
            _system.Clear();
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
            
            m_GotoMenu = false;
            GameMode gameMode = (GameMode) procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();

            foreach (var sys in _system)
            {
                sys.Init();
                
            }
            
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
            }

            foreach (var sys in _system)
            {
                sys.Shutdown();
            }
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                foreach (var sys in _system)
                {
                    sys.Update(elapseSeconds,realElapseSeconds);
                }
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
        
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_GameForm = (GameForm)ne.UIForm.Logic;

            foreach (var sys in _system)
            {
                sys.SetUI(m_GameForm);
            }

            _cardSelectionSystem.DeckDrawingSystem = _deckDrawingSystem;
            _cardSelectionSystem.HandPresentationSystem = _handPresentationSystem;
            _deckDrawingSystem.handPresentatation=_handPresentationSystem;
            _handPresentationSystem.cardSelectionSystem = _cardSelectionSystem;
            
            
            _deckDrawingSystem.LoadDeck(staringDeck);
            _deckDrawingSystem.ShuffleDeck();
            _deckDrawingSystem.DrawCardsFromDeck(5);
        }
    }
}