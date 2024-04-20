using Assets._MatchMaker.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakerLayoutBuilder : MonoBehaviour
{
    [SerializeField]private MatchMakerData _matchMakerData;
    [SerializeField]private RectTransform _panel;
    [SerializeField]private Card _card;
    [SerializeField] TMP_Text _levelNameText;
    [SerializeField]private Vector2 _startOffset;
    [SerializeField]private Vector2 _extraPadding;
    List<Vector2> _placementPositions = new List<Vector2>();
    public MatchMakerData MatchMakerData => _matchMakerData;
    /// <summary>
    /// Callback when build is completed and returns the cards that were built
    /// </summary>
    public event Action<List<Card>> BuildCompleted;
    
    private Vector2 _gridSize;

    public void BuildLayout(MatchMakerData _data)
    {
        _placementPositions.Clear();
        ClearLayout();
        _levelNameText.text = _data.name.ToUpper();
        _gridSize = _data._dimensions;
        float width = _panel.sizeDelta.x *0.8f; //20% for padding
        float height = _panel.sizeDelta.y * 0.8f;
        Vector2 totalPadding = new Vector2(_panel.sizeDelta.x, _panel.sizeDelta.y)*0.2f;
        Vector2 cellPadding = new Vector2(totalPadding.x/ (_gridSize.x), totalPadding.y/ (_gridSize.y+1)) + _extraPadding;
        Vector2 cellSize = Vector2.zero;
        cellSize.x = width / (_gridSize.x);
        cellSize.y = height /( _gridSize.y);
        if (cellSize.x > cellSize.y)
        {
            cellSize.x = cellSize.y;
        }
        else
        {
            cellSize.y = cellSize.x;
        }
        List<Card> cards = new List<Card>();
        float paddingX = 0f;
        float paddingY = 0f;
        Vector2 pos = Vector2.zero;
        Vector2 startPosition = new Vector2((-_panel.sizeDelta.x / 2)+ cellSize.x/2, (_panel.sizeDelta.y / 2 )- cellSize.y/2) + _startOffset;
        for (int y = 0; y < _gridSize.y; y++)
        {
            paddingY += cellPadding.y;
            pos.y = startPosition.y  + (cellSize.x * -y)- paddingY;
            for (int x =0;x< _gridSize.x; x++)
            {
                paddingX += cellPadding.x;
                pos.x = startPosition.x + (cellSize.x * x)+ paddingX;
                _placementPositions.Add(pos);
            }
            paddingX = 0;
        }
        for (int i = 0; i < _data.spriteMatrix.Length; i++)
        {
            if (_data.spriteMatrix[i] == null)
                continue;
            Card card = Instantiate(_card, _panel);
            card.Init(_data.spriteMatrix[i], cellSize, _placementPositions[i]);
            cards.Add(card);
        }
        BuildCompleted?.Invoke(cards);
        cards.Clear();
    }
    public void ClearLayout()
    {
        for (int i = 0; i < _panel.childCount; i++)
        {
            Transform child = _panel.GetChild(i);
            if(child.gameObject.activeSelf)
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(MatchMakerLayoutBuilder))]
public class MatchMakerLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Build"))
            {
                var builder = target as MatchMakerLayoutBuilder;
                builder.ClearLayout();
                builder.BuildLayout(builder.MatchMakerData);
            }
        }
    }
}
#endif
