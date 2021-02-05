﻿using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class GameSystem:SystemBase
    {
        protected GameForm gameUI;

        public TargetableObject player;
        public List<TargetableObject> enemys=new List<TargetableObject>();
        
        public void SetUI(UGuiForm ui)
        {
            gameUI=ui as GameForm;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}