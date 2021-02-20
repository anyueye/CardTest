using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode { get; }

        public GameTurn gameTurn;

        protected GameForm m_GameForm = null;

        private PlayerLogic _mPlayerLogic = null;
        private List<EnemyLogic> _enemyLogic;

        private readonly List<GameSystem> _system = new List<GameSystem>();

        private CardSelectionSystem _cardSelectionSystem;
        private DeckDrawingSystem _deckDrawingSystem;
        private HandPresentationSystem _handPresentationSystem;
        private EffectResolutionSystem _effectResolutionSystem;
        public List<int> staringDeck = new List<int>(){1001,1001,1001,1003,1002,1002,1002,1002,1000};
        public bool GameOver { get; protected set; }
        public bool Victory { get; protected set; }


        private bool _prepareCharacter;
        private bool _prepareUI;

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            gameTurn = GameTurn.None;

            GameEntry.UI.OpenUIForm(UIFormId.GameForm, this);

            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 1)
            {
                Position = new Vector3(-3.65f, -1.14f, 0),
                LocalScale = Vector3.one * 0.6f,
            });

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 200)
            {
                Position = new Vector3(3.58f, -1.14f, 0),
                LocalScale = Vector3.one * 0.6f,
            });

            _cardSelectionSystem = new CardSelectionSystem();
            _deckDrawingSystem = new DeckDrawingSystem();
            _handPresentationSystem = new HandPresentationSystem();
            _effectResolutionSystem = new EffectResolutionSystem();

            _system.Add(_effectResolutionSystem);
            _system.Add(_cardSelectionSystem);
            _system.Add(_deckDrawingSystem);
            _system.Add(_handPresentationSystem);

            foreach (var sys in _system)
            {
                sys.Init();
            }

            _prepareCharacter = false;
            _prepareUI = false;
            GameOver = false;
            _mPlayerLogic = null;
            _enemyLogic = new List<EnemyLogic>();
        }

        public virtual void Shutdown()
        {
            foreach (var sys in _system)
            {
                sys.Shutdown();
            }

            _system.Clear();
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSecondes)
        {
            if (_mPlayerLogic != null && _mPlayerLogic.IsDead)
            {
                GameOver = true;
                return;
            }

            if (_enemyLogic.Count <= 0)
            {
                Victory = true;
                return;
            }

            foreach (var sys in _system)
            {
                sys.Update(elapseSeconds, realElapseSecondes);
            }

            if (_prepareCharacter && _prepareUI)
            {
                foreach (var sys in _system)
                {
                    sys.SetUI(m_GameForm);
                }

                _cardSelectionSystem.DeckDrawingSystem = _deckDrawingSystem;
                _cardSelectionSystem.HandPresentationSystem = _handPresentationSystem;
                _cardSelectionSystem.effectResolutionSystem = _effectResolutionSystem;
                _deckDrawingSystem.handPresentatation = _handPresentationSystem;
                _handPresentationSystem.cardSelectionSystem = _cardSelectionSystem;


                _deckDrawingSystem.LoadDeck(staringDeck);
                _deckDrawingSystem.ShuffleDeck();
                _deckDrawingSystem.DrawCardsFromDeck(5);
                _prepareCharacter = _prepareUI = false;
            }
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs) e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }


        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
            if (ne.EntityLogicType == typeof(PlayerLogic))
            {
                _mPlayerLogic = (PlayerLogic) ne.Entity.Logic;
                foreach (var sys in _system)
                {
                    sys.player = _mPlayerLogic;
                }
                staringDeck.AddRange(_mPlayerLogic.PlayerData.allCards);
            }

            if (ne.EntityLogicType == typeof(EnemyLogic))
            {
                _enemyLogic.Add((EnemyLogic) ne.Entity.Logic);
                foreach (var sys in _system)
                {
                    sys.enemys.AddRange(_enemyLogic);
                }
            }

            _prepareCharacter = true;
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs) e;
            if (ne.UserData != this)
            {
                return;
            }

            m_GameForm = (GameForm) ne.UIForm.Logic;

            _prepareUI = true;
        }
    }
}