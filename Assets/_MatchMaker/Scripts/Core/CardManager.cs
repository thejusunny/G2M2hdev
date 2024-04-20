using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private MatchMakerLayoutBuilder _layoutBuilder;
    [SerializeField] AudioClip _correctClip;
    [SerializeField] AudioClip _wrongClip;
    [SerializeField] AudioClip _completionClip;
    [SerializeField] private float _revealDuration = 1.5f;
    private List<Card> _cards;
    //private List<Card> _stagedCards = new List<Card>();
    private List<(Card, Card)> _stagedCards = new List<(Card, Card)>();
    private List<Card> _tempCards = new List<Card>();
    public event Action<bool> FlipEvaluated;
    public event Action AllCardsFlipped;
    private int _flipCount;
    private void Awake()
    {
        _layoutBuilder.BuildCompleted += Initialize;
        LevelManager.LoadedLevel += Reset;
        LevelManager.StartLevel += QuickReveal;
        LevelManager.LoadedLevel += QuickReveal;
    }
    private void OnDestroy()
    {
        LevelManager.LoadedLevel -= Reset;
        LevelManager.StartLevel -= QuickReveal;
        LevelManager.LoadedLevel -= QuickReveal;
    }
    private void Reset()
    {
        _flipCount = 0;
        _stagedCards.Clear();
        _tempCards.Clear();
    }
    private void Initialize(List<Card> cards)
    {
        _cards = new List<Card>(cards);
        _tempCards.Clear();
        _stagedCards.Clear();
        foreach (Card card in _cards)
        {
            card.Clicked.AddListener(() =>
            {
                StageCard(card);
            });
        }
    }
    public void QuickReveal()
    {
        StartCoroutine(ShowAndHide());
    }
    private IEnumerator ShowAndHide()
    {
        foreach (Card card in _cards)
        {
            card.Flip();
        }
        yield return new WaitForSeconds(_revealDuration);
        foreach (Card card in _cards)
        {
            card.Flip();
        }
    }
    private void StageCard(Card card)
    {
        _tempCards.Add(card);
        if (_tempCards.Count >= 2)
        {
            //Debug.Log(_tempCards[0].FrontSprite.name + "::" + _tempCards[^1].FrontSprite.name);
            _stagedCards.Add((_tempCards[0], _tempCards[^1]));
            _tempCards.Clear();
        }
        card.FlipAnimationCompleted += Evaluate;
    }
    [ContextMenu("Print")]
    private void DebugPrints()
    {
        Debug.Log(_stagedCards.Count);
        foreach (var item in _stagedCards)
        {
            Debug.Log(item);
        }
    }
    private void Evaluate()
    {
        for (int i = _stagedCards.Count - 1; i >= 0; i--)
        {
            var latestBatch = _stagedCards[i];
            if (latestBatch.Item1.Flipped && latestBatch.Item2.Flipped)
            {
                if (latestBatch.Item1.FrontSprite == latestBatch.Item2.FrontSprite)
                {
                    _flipCount += 2;
                    FlipEvaluated?.Invoke(true);
                    if (_flipCount >= _cards.Count)
                    {
                        //Debug.Log("All Cards Flipped");
                        AllCardsFlipped?.Invoke();
                        _layoutBuilder.ClearLayout();
                        AudioManager.Instance.PlayClip(_completionClip, AudioManager.AudioType.SFX, false, 0.5f);
                    }
                    AudioManager.Instance.PlayClip(_correctClip, AudioManager.AudioType.SFX, false, 0.5f);
                    //Debug.Log("GoodFlip" + latestBatch.Item1.FrontSprite.name + "::" + latestBatch.Item2.FrontSprite.name);
                }
                else
                {
                    latestBatch.Item1.Flip();
                    latestBatch.Item2.Flip();
                    FlipEvaluated?.Invoke(false);
                    AudioManager.Instance.PlayClip(_wrongClip, AudioManager.AudioType.SFX, false, 0.5f);
                    //Debug.Log("Wrong flip::" + latestBatch.Item1.FrontSprite.name + "::" + latestBatch.Item2.FrontSprite.name);
                }
                latestBatch.Item1.FlipAnimationCompleted -= Evaluate;
                latestBatch.Item2.FlipAnimationCompleted -= Evaluate;
                _stagedCards.RemoveAt(i);
            }
        }

    }
}
