using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int CurrentLevel { get; private set; } = 1;
    private const int maxLevel = 5;

    private void Start()
    {
        LoadProgress();
        LoadLevel(CurrentLevel);
    }

    public void LoadLevel(int level)
    {
        // Load level scene
        SceneManager.LoadScene("Level" + level);
    }

    public void CompleteLevel()
    {
        CurrentLevel++;
        SaveProgress();
        LoadLevel(CurrentLevel);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
    }

    private void LoadProgress()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }
}
