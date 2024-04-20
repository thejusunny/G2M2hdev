using TMPro;
using UnityEngine;

public class UIScoreHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _correctText;
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private TMP_Text _streakText;
    [SerializeField] private TMP_Text _bestStreakText;
    public void Refresh(Score score, Streak streak)
    { 
        _correctText.text = score.correct.ToString();
        _turnsText.text = score.turns.ToString();
        _streakText.text = streak.streakCount.ToString();
        _bestStreakText.text = streak.bestStreak.ToString();   
    }
}
