using Assets._MatchMaker.Scripts.Data;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakerLayoutBuilder : MonoBehaviour
{
    [SerializeField] private MatchMakerData _matchMakerData;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Image _image;
    [SerializeField]private Vector2 _startOffset;
    [SerializeField]private Vector2 _extraPadding;
    
    private Vector2 _gridSize;
    private readonly Color TransparentColor = new  Color(1, 1, 1, 0);
    
    public void BuildLayout()
    {
        _gridSize = _matchMakerData._dimensions;
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
        float paddingX = 0f;
        float paddingY = 0f;
        Vector2 pos = Vector2.zero;
        Vector2 startPosition = new Vector2((-_panel.sizeDelta.x / 2)+ cellSize.x/2, (_panel.sizeDelta.y / 2 )- cellSize.y/2) + _startOffset;
        for (int y = 0; y < _gridSize.y; y++)
        {
            paddingY += cellPadding.y;
            pos.y = startPosition.y  + (cellSize.x * -y)- paddingY ;
            for (int x =0;x< _gridSize.x; x++)
            {
                paddingX += cellPadding.x;
                pos.x = startPosition.x + (cellSize.x * x)+ paddingX;
                Image newImage = Instantiate(_image, _panel);
                Sprite sprite = _matchMakerData.spriteMatrix[y + (int)(_gridSize.x * x)];
                if (sprite == null)
                {
                    newImage.color = TransparentColor;
                }
                else
                    newImage.sprite = sprite;
                newImage.transform.localPosition = pos;
                newImage.rectTransform.sizeDelta = cellSize;
                newImage.gameObject.SetActive(true);
            }
            paddingX = 0;
        }
    }
    public void ClearLayout()
    {
        for (int i = 0; i < _panel.childCount; i++)
        {
            Transform child = _panel.GetChild(i);
            if(child.gameObject.activeSelf)
            {
                if (EditorApplication.isPlaying)
                    Destroy(child.gameObject);
            }
        }
    }
}
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
                builder.BuildLayout();
            }
        }
    }
}
