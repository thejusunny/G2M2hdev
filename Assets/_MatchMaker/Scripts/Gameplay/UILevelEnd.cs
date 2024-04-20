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
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _resetButton;
    public void Show(Score score, Streak streak, string levelName, bool showNext = true)
    {
        _root.SetActive(true);
        _levelName.text = levelName.ToUpper();
        _correctText.text = score.correct.ToString();
        _totalTurnsText.text = score.turns.ToString();
        _wrongText.text = score.wrong.ToString();
        _pointsText.text = score.totalPoints.ToString();
        _bestStreakText.text = streak.bestStreak.ToString();
        if (showNext)
        {
            _nextButton.SetActive(true);
            _resetButton.SetActive(false);
        }
        else
        {
            _nextButton.SetActive(false);
            _resetButton.SetActive(true);
        }
    }
    public void Hide()
    { 
        _root.SetActive(false); 
    }
}
