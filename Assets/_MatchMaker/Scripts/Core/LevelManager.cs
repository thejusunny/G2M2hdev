using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UILevelEnd _uiLevelEnd;
    // Start is called before the first frame update
    void Start()
    {
        _scoreManager.ScoreFinalized += ShowLevelEnd;
    }
    private void OnDestroy()
    {
        _scoreManager.ScoreFinalized -= ShowLevelEnd;
    }
    private void ShowLevelEnd(Score score, Streak streak)
    {
        _uiLevelEnd.Show(score, streak);
    }
    private void SaveLevelProgress()
    { 
    
    }
    public void NextLevel()
    { 
        
    }
}
