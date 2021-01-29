// using System.Collections.Generic;
// using CardGame;
// using GameFramework.DataTable;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class CardManager : MonoBehaviour
// {
//     /// <summary>
//     /// 当局所有牌
//     /// </summary>
//     public List<int> allCards = new List<int>();
//
//     /// <summary>
//     /// 抽排堆
//     /// </summary>
//     public List<int> deckPile = new List<int>();
//特朗普成首位遭两次弹劾美总统
//     /// <summary>
//     /// 弃牌堆
//     /// </summary>
//     public List<int> discardPile = new List<int>();
//
//     /// <summary>
//     /// 手牌
//     /// </summary>
//     public List<int> handPile = new List<int>();
//
//
//     public List<Card> currentCards = new List<Card>();
//
//     public int handLimit;
//     private float cardWight => 3f;
//
//     float cardWidth => 0.5f;
//
//     public Card cardPrefab;
//
//
//     public void InitGame()
//     {
//         deckPile.AddRange(allCards);
//         discardPile.Clear();
//         handPile.Clear();
//     }
//
//     public void DrawCardsFromDeck(int drawCount)
//     {
//         while (true)
//         {
//             // var canDrawCount = handLimit-handPile.Count;
//             if (deckPile.Count >= drawCount)
//             {
//                 var drawCardList = deckPile.GetRandom(drawCount);
//                 ReflushCard(drawCardList);
//                 foreach (var cardId in drawCardList)
//                 {
//                     deckPile.Remove(cardId);
//                 }
//                 
//             }
//             else
//             {
//                 if (deckPile.Count<=0)
//                 {
//                     return;
//                 }
//                 if (allCards.Count < drawCount)
//                 {
//                     ReflushCard(allCards.GetRandom(allCards.Count));
//                     deckPile.Clear();
//                     return;
//                 }
//                 
//                 ReflushCard(deckPile);
//                 deckPile.Clear();
//                 deckPile.AddRange(allCards);
//                 foreach (var cardId in handPile)
//                 {
//                     deckPile.Remove(cardId);
//                 }
//                 discardPile.Clear();
//                 drawCount -= handPile.Count;
//                 continue;
//             }
//             break;
//         }
//     }
//
//     void ReflushCard(List<int> cards)
//     {
//         int len = cards.Count;
//         for (int i = 0; i < len; i++)
//         {
//             var card = Instantiate(cardPrefab, transform, false);
//             card.InitCard(cards[i]);
//             card.cm = this;
//             currentCards.Add(card);
//         }
//         handPile.AddRange(cards);
//         ReflushCardPos();
//     }
//
//     void ReflushCardPos()
//     {
//         int len = currentCards.Count;
//         bool cardsWidthourRange = (cardWidth * 2f) * len > cardWight;
//         float cardsLength = cardsWidthourRange ? cardWight : cardWidth * 2f * len - cardWidth * 2;
//         float space = cardsWidthourRange ? cardWight / (len - 1) : cardWidth * 2f;
//         for (int i = 0; i < len; i++)
//         {
//             float posX = i * space - cardsLength / 2f;
//             var pos = new Vector3(posX, 0, 0.01f * i);
//             currentCards[i].transform.localPosition = pos;
//             currentCards[i].InitPos();
//         }
//     }
//
//     void OnDiscard()
//     {
//         discardPile.AddRange(handPile);
//         handPile.Clear();
//         if (deckPile.Count > 0) return;
//         deckPile.AddRange(allCards);
//         discardPile.Clear();
//     }
//
//     public void PushCard(Card c)
//     {
//         currentCards.Remove(c);
//         handPile.Remove(c.cardId);
//         discardPile.Add(c.cardId);
//         Destroy(c.gameObject);
//         ReflushCardPos();
//     }
//
//     public void EndOfRound()
//     {
//         for (int i = currentCards.Count-1; i >=0; i--)
//         {
//             Destroy(currentCards[i].gameObject);
//         }
//         currentCards.Clear();
//         OnDiscard();
//     }
//
//
//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             DrawCardsFromDeck(handLimit);
//         }
//
//         if (Input.GetKeyDown(KeyCode.W))
//         {
//             EndOfRound();
//         }
//     }
//
//     private IDataTable<DRCards> dtCards;
//     // Start is called before the first frame update
//     void Start()
//     {
//         // dtCards= GameEntry.DataTable.GetDataTable<DRCards>();
//         InitGame();
//     }
// }
//
