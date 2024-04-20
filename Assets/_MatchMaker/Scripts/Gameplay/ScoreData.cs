using UnityEngine;

[CreateAssetMenu(fileName ="ScoreData", menuName ="MatchMaker/ScoreData")]
public class ScoreData : ScriptableObject
{
    [SerializeField]private Score _score;
    [SerializeField]private Streak _streak;
    public Score Score => _score;
    public Streak Streak => _streak;
    public void Save(Score score, Streak streak)
    {
        _score = score;
        _streak = streak;
        //TODO: save to JSON
    }
}
