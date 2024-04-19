using Assets._MatchMaker.Scripts.Data;
using UnityEditor;
using UnityEngine;

namespace Assets._MatchMaker.Scripts.Editor
{
    [CustomEditor(typeof(MatchMakerData))]
    public class MatchMakerDataInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var data = target as MatchMakerData;
            if (GUILayout.Button("Edit"))
            {
                MatchMakerEditor.Open(data);
            }
        }
    }
}