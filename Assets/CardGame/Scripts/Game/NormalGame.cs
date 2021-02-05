using System;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class NormalGame:GameBase
    {
        public override GameMode GameMode { get=>GameMode.Normal; }
        public override void Update(float elapseSeconds, float realElapseSecondes)
        {
            base.Update(elapseSeconds, realElapseSecondes);
            switch (gameTurn)
            {
                case GameTurn.None:
                    break;
                case GameTurn.PlayerTurnBegan:
                    break;
                case GameTurn.PlayerTurnEnd:
                    gameTurn = GameTurn.EnemyTurnBegan;
                    break;
                case GameTurn.EnemyTurnBegan:
                    break;
                case GameTurn.EnemyTurnEnd:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}