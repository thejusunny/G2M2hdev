using TMPro;
using UnityEngine;

public class UIScoreHUD : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _correctText;
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private TMP_Text _streakText;
    [SerializeField] private TMP_Text _bestStreakText;
    private void Start()
    {
        LevelManager.LoadedLevel += Show;
        LevelManager.StartLevel += Show;
    }
    private void OnDestroy()
    {
        LevelManager.LoadedLevel -= Show;
        LevelManager.StartLevel -= Show;
    }
    public void Refresh(Score score, Streak streak)
    { 
        _correctText.text = score.correct.ToString();
        _turnsText.text = score.turns.ToString();
        _streakText.text = streak.streakCount.ToString();
        _bestStreakText.text = streak.bestStreak.ToString();   
    }
    public void Hide()
    {
        _root.SetActive(false);
    }
    public void Show()
    {
        _root.SetActive(true);
    }
}
