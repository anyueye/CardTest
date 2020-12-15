using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    /// <summary>
    /// 当局所有牌
    /// </summary>
    public List<int> allCards = new List<int>();

    /// <summary>
    /// 抽排堆
    /// </summary>
    public List<int> drawPile = new List<int>();

    /// <summary>
    /// 弃牌堆
    /// </summary>
    public List<int> discardPile = new List<int>();

    /// <summary>
    /// 手牌
    /// </summary>
    public List<int> handPile = new List<int>();


    public List<Card> currentCards = new List<Card>();

    public int handLimit;
    private float cardWight => 3f;

    float cardWidth => 0.5f;

    public Card cardPrefab;


    void InitGame()
    {
        drawPile.AddRange(allCards);
        discardPile.Clear();
        handPile.Clear();
    }

    void DrawCard(int drawCount)
    {
        while (true)
        {
            // var canDrawCount = handLimit-handPile.Count;
            if (drawPile.Count >= drawCount)
            {
                var drawCardList = drawPile.GetRandom(drawCount);
                ReflushCard(drawCardList);
                foreach (var cardId in drawCardList)
                {
                    drawPile.Remove(cardId);
                }
                
            }
            else
            {
                if (drawPile.Count<=0)
                {
                    return;
                }
                if (allCards.Count < drawCount)
                {
                    ReflushCard(allCards.GetRandom(allCards.Count));
                    drawPile.Clear();
                    return;
                }
                
                ReflushCard(drawPile);
                drawPile.Clear();
                drawPile.AddRange(allCards);
                foreach (var cardId in handPile)
                {
                    drawPile.Remove(cardId);
                }
                discardPile.Clear();
                drawCount -= handPile.Count;
                continue;
            }
            break;
        }
    }

    void ReflushCard(List<int> cards)
    {
        int len = cards.Count;
        for (int i = 0; i < len; i++)
        {
            var card = Instantiate(cardPrefab, transform, false);
            card.InitCard(cards[i]);
            card.cm = this;
            currentCards.Add(card);
        }
        handPile.AddRange(cards);
        ReflushCardPos();
    }

    void ReflushCardPos()
    {
        int len = currentCards.Count;
        bool cardsWidthourRange = (cardWidth * 2f) * len > cardWight;
        float cardsLength = cardsWidthourRange ? cardWight : cardWidth * 2f * len - cardWidth * 2;
        float space = cardsWidthourRange ? cardWight / (len - 1) : cardWidth * 2f;
        for (int i = 0; i < len; i++)
        {
            float posX = i * space - cardsLength / 2f;
            var pos = new Vector3(posX, 0, 0.01f * i);
            currentCards[i].transform.localPosition = pos;
            currentCards[i].InitPos();
        }
    }

    void OnDiscard()
    {
        discardPile.AddRange(handPile);
        handPile.Clear();
        if (drawPile.Count > 0) return;
        drawPile.AddRange(allCards);
        discardPile.Clear();
    }

    public void PushCard(Card c)
    {
        currentCards.Remove(c);
        handPile.Remove(c.cardId);
        discardPile.Add(c.cardId);
        Destroy(c.gameObject);
        ReflushCardPos();
    }

    public void EndOfRound()
    {
        for (int i = currentCards.Count-1; i >=0; i--)
        {
            Destroy(currentCards[i].gameObject);
        }
        currentCards.Clear();
        OnDiscard();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DrawCard(handLimit);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EndOfRound();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }
}

static class RandomExt
{
    /// <summary>
    /// 固定数组中的不重复随机
    /// </summary>
    /// <param name="nums">数组</param>
    /// <param name="count">要随机的个数</param>
    /// <returns></returns>
    public static List<T> GetRandom<T>(this List<T> nums, int count)
    {
        if (count > nums.Count)
        {
            Debug.LogError("要取的个数大于数组长度！");
            return null;
        }

        List<T> result = new List<T>();
        List<int> id = new List<int>();

        for (int i = 0; i < nums.Count; i++)
        {
            id.Add(i);
        }

        while (id.Count > nums.Count - count)
        {
            var r = Random.Range(0, id.Count);
            result.Add(nums[id[r]]);
            id.Remove(id[r]);
        }

        return (result);
    }
}