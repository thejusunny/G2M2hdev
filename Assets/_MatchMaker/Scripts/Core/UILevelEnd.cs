using TMPro;
using UnityEngine;

public class UILevelEnd : MonoBehaviour
{
    [SerializeField]private GameObject _root;
    [SerializeField]private TMP_Text _levelName;
    [SerializeField]private TMP_Text _correctText;
    [SerializeField]private TMP_Text _totalTurnsText;
    [SerializeField]private TMP_Text _wrongText;
    [SerializeField]private TMP_Text _pointsText;
    [SerializeField]private TMP_Text _bestStreakText;
    public void Show(Score score, Streak streak)
    {
        _root.SetActive(true);
        _levelName.text = "GAME";
        _correctText.text = score.correct.ToString();
        _totalTurnsText.text = score.turns.ToString();
        _wrongText.text = score.wrong.ToString();
        _pointsText.text = score.totalPoints.ToString();
        _bestStreakText.text = streak.bestStreak.ToString();
    }
}
