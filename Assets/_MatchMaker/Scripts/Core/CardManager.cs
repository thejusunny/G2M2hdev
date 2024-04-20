using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]private MatchMakerLayoutBuilder _layoutBuilder;
    private List<Card> _cards ;
    //private List<Card> _stagedCards = new List<Card>();
    private Queue<(Card, Card)> _stagedCards = new Queue<(Card, Card)> ();
    private List<Card> _tempCards = new List<Card> ();
    public event Action<bool> FlipEvaluated;
    public event Action AllCardsFlipped;
    private int _flipCount;
    private void Awake()
    {
        _layoutBuilder.BuildCompleted += Initialize;
        LevelManager.LoadedNewLevel += Reset;
    }
    private void OnDestroy()
    {
        LevelManager.LoadedNewLevel -= Reset;
    }
    private void Reset()
    {
        _flipCount = 0;
        _stagedCards.Clear ();
        _tempCards.Clear ();
    }
    private void Initialize(List<Card> cards)
    {
        _cards = new List<Card> (cards);
        _tempCards.Clear ();
        _stagedCards.Clear ();
        foreach (Card card in _cards)
        {
            card.Clicked.AddListener(() =>
            {
                StageCard(card);
            });
        }
    }
    private void StageCard(Card card)
    {
        _tempCards.Add(card);
        if (_tempCards.Count >= 2)
        {
            _stagedCards.Enqueue((_tempCards[0], _tempCards[^1]));
            _tempCards.Clear();
        }
        card.FlipAnimationCompleted += Evaluate;
    }
    private void Evaluate()
    {
        if (_stagedCards.Count>0)
        {
            var latestBatch = _stagedCards.Peek();
            if (latestBatch.Item1.Flipped && latestBatch.Item2.Flipped)
            {
                if (latestBatch.Item1.FrontSprite == latestBatch.Item2.FrontSprite)
                {
                    _flipCount +=2;
                    FlipEvaluated?.Invoke(true);
                    if (_flipCount >= _cards.Count)
                    {
                        Debug.Log("All Cards Flipped");
                        AllCardsFlipped?.Invoke();
                        _layoutBuilder.ClearLayout();
                    }
                    Debug.Log("GoodFlip" + latestBatch.Item1.FrontSprite.name + "::" + latestBatch.Item2.FrontSprite.name);
                }
                else
                {
                    latestBatch.Item1.Flip();
                    latestBatch.Item2.Flip();
                    FlipEvaluated?.Invoke(false);
                    Debug.Log("Wrong flip::"+latestBatch.Item1.FrontSprite.name +"::"+ latestBatch.Item2.FrontSprite.name);
                }
                latestBatch.Item1.FlipAnimationCompleted -= Evaluate;
                latestBatch.Item2.FlipAnimationCompleted -= Evaluate;
                _stagedCards.Dequeue();
            }
        }
    }
}
