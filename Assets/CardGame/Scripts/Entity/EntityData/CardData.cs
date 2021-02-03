using System.Collections.Generic;
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

        public enum CardTarget
        {
            Self = 0,
            Target,
            AllTarget,
            All
        }

        [SerializeField] private int _cardId;
        [SerializeField] private string _cardName;
        [SerializeField] private int _cost;
        [SerializeField] private Material _material;
        [SerializeField] private Sprite _icon;
        [SerializeField] private CardType _type;
        [SerializeField] private List<CardTarget> _target = new List<CardTarget>();
        [SerializeField] private string _description;


        public CardData(int entityId, int typeId, int cardID) : base(entityId, typeId)
        {
            var dtCard = GameEntry.DataTable.GetDataTable<DRCards>();
            DRCards drCard = dtCard.GetDataRow(cardID);
            if (drCard == null)
            {
                return;
            }

            var dtCardeffects = GameEntry.DataTable.GetDataTable<DRCardEffects>();
            var builder = new StringBuilder();
            for (int i = 0; i < drCard.Effects.Count; i++)
            {
                var effectId = drCard.Effects[i];
                builder.Append(Utility.Text.Format(dtCardeffects[effectId].Describe, dtCardeffects[effectId].Value));
                _target.Add((CardTarget) dtCardeffects[effectId].Target);
            }

            _cardId = cardID;
            _cardName = drCard.Name;
            _cost = drCard.Cost;
            _type = (CardType) drCard.Type;
            if (drCard.Material == "Default")
            {
            }

            GameEntry.Resource.LoadAsset(AssetUtility.GetCardIconAsset(drCard.Picture),typeof(Sprite), Constant.AssetPriority.DictionaryAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load Sprite '{0}' OK.", assetName);
                    _icon = asset as Sprite;
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", drCard.Picture, assetName, errorMessage);
                }));
            _description = builder.ToString();
        }


        public int CardId
        {
            get => _cardId;
            set => _cardId = value;
        }

        public string CardName
        {
            get => _cardName;
            set => _cardName = value;
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
            set => _type = value;
        }

        public List<CardTarget> Target
        {
            get => _target;
            set => _target = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}