using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{

    public int cardId;

    public CardManager cm;
    
    private SpriteRenderer sr=>GetComponent<SpriteRenderer>();
    private Vector3 onSelectCardPos=Vector3.zero;
    
    public enum CardState
    {
        onHover,
        onSelect,
        onUnSelect,
        onExit,
    }

    [SerializeField] private CardState _cardState = CardState.onExit;
    
    public void InitCard(int id)
    {
        cardId = id;
        GetComponentInChildren<TextMeshPro>().text = $"{cardId}";
    }

    public void InitPos()
    {
        onSelectCardPos =transform.localPosition;
    }
    



    private void OnMouseEnter()
    {
        if (_cardState!=CardState.onExit) return;
        _cardState = CardState.onHover;
        transform.DOScale(Vector3.one * 1.2f, 0.2f);
        transform.DOLocalMove(new Vector3(onSelectCardPos.x, sr.size.y*0.1f, -0.1f), 0.2f);
        
    }

    private void OnMouseExit()
    {
        if (_cardState == CardState.onHover)
        {
            _cardState = CardState.onExit;
            transform.DOScale(Vector3.one, 0.2f);
            transform.DOLocalMove(onSelectCardPos, 0.2f);
        }
    }

    private void OnMouseDown()
    {
        _cardState = CardState.onSelect;
    }

    private void OnMouseUp()
    {
        if (transform.position.y>-2f)
        {
            cm.PushCard(this);
            return;
        }
        _cardState = CardState.onUnSelect;
        transform.localPosition = onSelectCardPos;
        transform.localScale = Vector3.one;
        _cardState = CardState.onExit;
    }

    private void OnMouseDrag()
    {
        if (_cardState != CardState.onSelect) return;
        var pos = Input.mousePosition;
        pos.z = Mathf.Abs(transform.parent.position.z-Camera.main.transform.position.z)-0.1f;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
