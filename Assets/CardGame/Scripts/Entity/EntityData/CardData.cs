using UnityEngine;

namespace CardGame
{
    public class CardData:EntityData
    {
        [SerializeField] private int _cardId;
        [SerializeField] private string _CardName;
        [SerializeField] private string _Descirbe;
        [SerializeField] private float _Damage;
        [SerializeField] private float _Recover;

        
        
        public CardData(int entityId, int typeId,int cardID) : base(entityId, typeId)
        {
            var dtCard = GameEntry.DataTable.GetDataTable<DRCards>();
            var drCard = dtCard.GetDataRow(typeId);
            if (drCard==null)
            {
                return;
            }
            _cardId = cardID;
            _CardName = drCard.Name;
            _Descirbe = drCard.Describe;
            _Damage = drCard.Damage;
            _Recover = drCard.Recover;
        }

        public int CardId
        {
            get => _cardId;
            set => _cardId = value;
        }

        public string CardName
        {
            get => _CardName;
            set => _CardName = value;
        }

        public string Descirbe
        {
            get => _Descirbe;
            set => _Descirbe = value;
        }

        public float Damage
        {
            get => _Damage;
            set => _Damage = value;
        }

        public float Recover
        {
            get => _Recover;
            set => _Recover = value;
        }
        
    }
}