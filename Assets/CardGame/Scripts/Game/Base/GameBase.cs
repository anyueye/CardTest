using System;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
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
        private List<EnemyLogic> _enemyLogics;

        private readonly List<GameSystem> _system = new List<GameSystem>();

        protected CardSelectionSystem _cardSelectionSystem;
        protected DeckDrawingSystem _deckDrawingSystem;
        protected HandPresentationSystem _handPresentationSystem;
        protected EffectResolutionSystem _effectResolutionSystem;
        public List<int> staringDeck = new List<int>() ;
        public bool GameOver { get; protected set; }
        public bool Victory { get; protected set; }


        private int _prepareCharacter;
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
            GameEntry.UI.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;
            GameEntry.Widget.CachedCanvas.worldCamera = GameEntry.Scene.MainCamera;
            
            GameEntry.Event.Subscribe(EventTest.EventId, Test);

            GameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
           
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 1)
            {
                Position = new Vector3(-5f, -1f, 0),
                LocalScale = Vector3.one * 0.4f,
            });

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 10)
            {
                Position = new Vector3(1f, -1f, 0),
                LocalScale = Vector3.one * 0.35f,
            });

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 11)
            {
                Position = new Vector3(4f, -1f, 0),
                LocalScale = Vector3.one * 0.35f,
            });
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 11)
            {
                Position = new Vector3(7f, -1f, 0),
                LocalScale = Vector3.one * 0.35f,
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

            _prepareCharacter = 0;
            _prepareUI = false;
            GameOver = false;
            _mPlayerLogic = null;
            _enemyLogics = new List<EnemyLogic>();
        }


        public virtual void Shutdown()
        {
            foreach (var sys in _system)
            {
                sys.Shutdown();
            }

            _system.Clear();
            GameEntry.Event.Unsubscribe(EventTest.EventId, Test);

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

            if (_enemyLogics.Count <= 0)
            {
                Victory = true;
                return;
            }

            foreach (var sys in _system)
            {
                sys.Update(elapseSeconds, realElapseSecondes);
            }

            if (_prepareCharacter == 4 && _prepareUI)
            {
                _enemyLogics.Sort((en0, en1) => en1.Id - en0.Id);
                foreach (var sys in _system)
                {
                    sys.SetUI(m_GameForm);
                    sys.enemys.AddRange(_enemyLogics);
                }

                _handPresentationSystem.cardSelectionSystem = _cardSelectionSystem;

                _deckDrawingSystem.LoadDeck(staringDeck);
                _deckDrawingSystem.ShuffleDeck();

                gameTurn = GameTurn.PlayerTurnBegan;
                
                _prepareUI = false;
                _prepareCharacter = 0;
            }
            
            switch (gameTurn)
            {
                case GameTurn.None:
                    break;
                case GameTurn.PlayerTurnBegan:
                    foreach (var enemyLogic in _enemyLogics)
                    {
                        enemyLogic.complateReset = true;
                        enemyLogic.prepareAttack = false;
                    }
                    m_GameForm.OnPlayerTurnBegan();
                    GameEntry.Event.FireNow(this, DrawnCardEventArgs.Create(5));
                    gameTurn = GameTurn.PlayerTurnUpdate;
                    break;
                case GameTurn.PlayerTurnEnd:
                    _deckDrawingSystem.MoveCardToDiscardPile();
                    _handPresentationSystem.MoveHandToDiscardPile();
                    gameTurn = GameTurn.EnemyTurnBegan;
                    actionEnemyIndex = 0;
                    EnemyAction(actionEnemyIndex);
                    break;
                case GameTurn.EnemyTurnBegan:
                    if (_enemyLogics[actionEnemyIndex].enemyFsm.CurrentState.GetType()==typeof(EnemyResetState))
                    {
                        actionEnemyIndex++;
                        if (actionEnemyIndex>=_enemyLogics.Count)
                        {
                            gameTurn = GameTurn.EnemyTurnEnd;
                            return;
                        }
                        EnemyAction(actionEnemyIndex);
                    }
                    break;
                case GameTurn.EnemyTurnEnd:
                    gameTurn = GameTurn.PlayerTurnBegan;
                    break;
            }
        }

        private int actionEnemyIndex = 0;


        public void EnemyAction(int enemyIndex)
        {
            _enemyLogics[enemyIndex].prepareAttack = true;
            _enemyLogics[enemyIndex].complateReset = false;
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
                _prepareCharacter++;
            }

            if (ne.EntityLogicType == typeof(EnemyLogic))
            {
                _enemyLogics.Add((EnemyLogic) ne.Entity.Logic);
                _prepareCharacter++;
            }
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