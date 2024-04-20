using UnityEngine;

namespace Assets._MatchMaker.Scripts.Data
{
    [CreateAssetMenu(fileName = "GridData", menuName = "MatchMaker/GridData")]
    public class MatchMakerData : ScriptableObject
    {
        [HideInInspector] public Sprite[] spriteMatrix;
        [HideInInspector] public Vector2 _dimensions;
    }
}