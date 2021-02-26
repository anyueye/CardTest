using System;
using System.Collections.Generic;
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

        protected CardSelectionSystem _cardSelectionSystem;
        protected DeckDrawingSystem _deckDrawingSystem;
        protected HandPresentationSystem _handPresentationSystem;
        protected EffectResolutionSystem _effectResolutionSystem;
        public List<int> staringDeck = new List<int>(){1001,1001,1001,1003,1002,1002,1002,1002,1000};
        public bool GameOver { get; protected set; }
        public bool Victory { get; protected set; }


        private bool _prepareCharacter;
        private bool _prepareUI;

        private void Test(object sender, GameEventArgs e)
        {
            EventTest ne = (EventTest) e;
            
            Debug.LogError($"args={ne.t0},t1={ne.t1},sender={sender}");
        }
        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            gameTurn = GameTurn.None;
            
            GameEntry.Event.Subscribe(EventTest.EventId,Test);

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
            
            // GameEntry.Event.Fire(this,EventTest.Create0(12));
            // GameEntry.Event.Fire(this,EventTest.Create1(45));

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
            GameEntry.Event.Unsubscribe(EventTest.EventId,Test);

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
            
            switch (gameTurn)
            {
                case GameTurn.None:
                    break;
                case GameTurn.PlayerTurnBegan:
                    break;
                case GameTurn.PlayerTurnEnd:
                    _deckDrawingSystem.MoveCardToDiscardPile();
                    _handPresentationSystem.MoveHandToDiscardPile();
                    gameTurn = GameTurn.EnemyTurnBegan;
                    break;
                case GameTurn.EnemyTurnBegan:
                    break;
                case GameTurn.EnemyTurnEnd:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!_prepareCharacter || !_prepareUI) return;
            {
                foreach (var sys in _system)
                {
                    sys.SetUI(m_GameForm);
                }
                
                _handPresentationSystem.cardSelectionSystem = _cardSelectionSystem;
                
                _deckDrawingSystem.LoadDeck(staringDeck);
                _deckDrawingSystem.ShuffleDeck();
                
                
                GameEntry.Event.FireNow(this,DrawnCardEventArgs.Create(5));
                
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