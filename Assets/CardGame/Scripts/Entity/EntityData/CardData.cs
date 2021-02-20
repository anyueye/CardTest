using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CardGame
{
    public class CardData : EntityData
    {
        public enum CardType
        {
            Attack = 0,
            Power,
            Skill,
        }


        [SerializeField] private readonly int _cardId;
        [SerializeField] private readonly string _cardName;
        [SerializeField] private int _cost;
        [SerializeField] private Material _material;
        [SerializeField] private Sprite _icon;
        [SerializeField] private readonly CardType _type;
        [SerializeField] private readonly string _description;

        readonly List<IntegerEffect> _effects = new List<IntegerEffect>();

        public IEnumerable<IntegerEffect> Effects
        {
            get => _effects;
        }


        // public IEnumerable<CardEffectData> CardEffectDatas => _cardEffectDatas;


        public CardData(int entityId, int typeId, int cardID) : base(entityId, typeId)
        {
            var dtCard = GameEntry.DataTable.GetDataTable<DRCards>();
            DRCards drCard = dtCard.GetDataRow(cardID);
            if (drCard == null)
            {
                return;
            }

            string effName;
            List<int> values = new List<int>();
            for (int index = 0; index < drCard.EffectCount && (effName = drCard.GetEffectAt(index)) != "null"; index++)
            {
                var eff = Utility.Assembly.GetType($"CardGame.{effName}");
                var value = drCard.GetValueAt(index);
                EffectTargetType targetType = (EffectTargetType) drCard.GetTargetAt(index);
                var effect = (IntegerEffect) Activator.CreateInstance(eff, value, targetType);
                _effects.Add(effect);
                if (value > 0)
                {
                    values.Add(value);
                }
            }

            _cardId = cardID;
            _cardName = drCard.Name;
            _cost = drCard.Cost;
            _type = (CardType) drCard.Type;
            if (drCard.Material == "Default")
            {
            }

            GameEntry.Resource.LoadAsset(AssetUtility.GetCardIconAsset(drCard.Picture), typeof(Sprite), Constant.AssetPriority.DictionaryAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load Sprite '{0}' OK.", assetName);
                    _icon = asset as Sprite;
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", drCard.Picture, assetName, errorMessage);
                }));
            
            object[] temp = Array.ConvertAll<int, object>(values.ToArray(), input => input);
            _description = string.Format(drCard.Describe, temp);
        }


        public int CardId
        {
            get => _cardId;
        }

        public string CardName
        {
            get => _cardName;
        }

        public int Cost
        {
            get => _cost;
            set => _cost = value;
        }

        public Material Material
        {
            get => _material;
            set => _material = value;
        }

        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public CardType Type
        {
            get => _type;
        }

        public string Description
        {
            get => _description;
        }
    }
}