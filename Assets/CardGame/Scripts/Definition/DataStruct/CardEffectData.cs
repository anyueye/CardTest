using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace CardGame
{
    public readonly struct CardEffectData
    {
        [SerializeField] private readonly EffectTargetType _target;

        public EffectTargetType Target
        {
            get => _target;
        }

        public string Describe
        {
            get => _describe;
        }

        public int Value
        {
            get => _value;
        }

        public List<int> SourceActions
        {
            get => sourceActions;
        }

        public List<int> TargetActions
        {
            get => targetActions;
        }

        public readonly TargetableEffect effect;

        [SerializeField] private readonly string _describe;
        [SerializeField] private readonly int _value;
        [SerializeField] private readonly List<int> sourceActions;
        [SerializeField] private readonly List<int> targetActions;

        public CardEffectData(int typeId)
        {
            var dtEffect = GameEntry.DataTable.GetDataTable<DRCardEffects>();
            var drEffect = dtEffect.GetDataRow(typeId);
            _target = (EffectTargetType) drEffect.Target;
            _describe = Utility.Text.Format(drEffect.Describe, drEffect.Value);
            _value = drEffect.Value;
            sourceActions = drEffect.SourceActions;
            targetActions = drEffect.TargetActions;
            var eff = Utility.Assembly.GetType($"CardGame.{drEffect.Effect}");
            effect = (TargetableEffect) Activator.CreateInstance(eff, _value);
        }

    }
}