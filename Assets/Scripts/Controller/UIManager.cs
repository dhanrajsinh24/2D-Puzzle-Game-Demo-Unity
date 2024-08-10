using IG.Controller;
using IG.UI;
using UnityEngine;

/// <summary>
/// Managers communication between all UI scripts and manager scripts
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameUI gameUI;
    public LevelScrollView levelScrollView;
    public ResultUI resultUI;

    public LevelManager LevelManager {get; private set;}
    private DatabaseManager _databaseManager;

    public void Initialize(LevelManager levelManager, DatabaseManager databaseManager) 
    {
        LevelManager = levelManager;
        _databaseManager = databaseManager;

        levelScrollView.Initialize(this, _databaseManager.LastUnlockedLevel);
        resultUI.Initialize(this);
    }
}
