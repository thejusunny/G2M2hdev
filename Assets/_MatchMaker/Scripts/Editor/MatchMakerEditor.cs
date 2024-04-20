using UnityEngine;
using UnityEditor;
using System.IO;
using Assets._MatchMaker.Scripts.Data;

namespace Assets._MatchMaker.Scripts.Editor
{
    public class MatchMakerEditor : EditorWindow
    {
        private Vector2 _gridSize = new Vector2(3, 3);
        private Vector2 currentGridSize = new Vector2(3, 3);
        private Sprite[] sprites;
        private bool _show = false;
        private Vector2 tileSize = new Vector2(64, 64);
        private Vector2 refScreenSize = new Vector2(360, 480);
        private Vector2 scaledTileSize;
        private string _savelFileName = "Game1";
        private string _rootPath = "Assets/_MatchMaker/Data/Games/";
        private string FullPath => _rootPath + _savelFileName + ".asset";

        private string _buildText = "Build";
        [MenuItem("Tools/MatchMaker")]
        public static void Open()
        {
            MatchMakerEditor window = CreateInstance<MatchMakerEditor>();
            window.Show();
        }
        private void OnDestroy()
        {
            _show = false;
        }
        public static void Open(MatchMakerData data)
        {
            MatchMakerEditor window = CreateInstance<MatchMakerEditor>();
            window.Show();
            window.Init(data);
        }
        public void Init(MatchMakerData data)
        {
            sprites = data.spriteMatrix;
            currentGridSize = _gridSize = data._dimensions;
            scaledTileSize = tileSize * Mathf.Min(position.width / refScreenSize.x, 2f);
            _show = true;
            _savelFileName = data.name;
        }
        private void OnGUI()
        {
            GUILayout.Label("Match Maker Editor Tool");
            currentGridSize.x = EditorGUILayout.FloatField("X Dimension", currentGridSize.x);
            currentGridSize.y = EditorGUILayout.FloatField("Y Dimension", currentGridSize.y);
            GUILayout.Space(10);
            if (GUILayout.Button(_buildText))
            {
                _show = true;
                _gridSize = currentGridSize;
                sprites = new Sprite[(int)(_gridSize.x * _gridSize.y)];
                _buildText = "Rebuild";
                scaledTileSize = tileSize * Mathf.Min(position.width / refScreenSize.x, 2f);
            }
            if (_show)
            {
                GUILayout.Space(10);
                for (int y = 0; y < _gridSize.y; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    for (int x = 0; x < _gridSize.x; x++)
                    {

                        //Debug.Log(x * (int)_gridSize.y + y);
                        sprites[y * (int)_gridSize.x + x] = EditorGUILayout.ObjectField(sprites[y * (int)_gridSize.x + x], typeof(Sprite), false, GUILayout.Width(scaledTileSize.x), GUILayout.Height(scaledTileSize.y)) as Sprite;
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                }
                GUILayout.Space(10);
                _savelFileName = EditorGUILayout.TextField("FileName", _savelFileName);
                if (GUILayout.Button("Save"))
                {
                    MatchMakerData matchMakerData = CreateInstance<MatchMakerData>();
                    matchMakerData.spriteMatrix = sprites;
                    matchMakerData._dimensions = _gridSize;
                    matchMakerData.name = _savelFileName;
                    string uniquePath = "";
                    if (!Directory.Exists(_rootPath))
                    {
                        Directory.CreateDirectory(_rootPath);
                    }
                    if (File.Exists(FullPath))
                    {
                        AssetDatabase.DeleteAsset(FullPath);
                    }
                    uniquePath = AssetDatabase.GenerateUniqueAssetPath(FullPath);
                    AssetDatabase.CreateAsset(matchMakerData, uniquePath);
                    AssetDatabase.SaveAssets();
                }
                GUI.enabled = false;
                EditorGUILayout.TextField("FullPath", FullPath);
                GUI.enabled = false;
            }
        }
    }
}