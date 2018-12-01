using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public LevelManager levelManager;
    public Button restart;
    public Button newGame;

    private void Start()
    {
        newGame.onClick.AddListener(() => StartGame());
        restart.onClick.AddListener(() => RestartGame());
    }

    private void StartGame()
    {
        int currentLevel = levelManager.GetCurrentLevel();
        if (currentLevel == 0)
        {
            currentLevel = 1;
        }

        var data = new LevelChangeData();
        data.LevelBuildIndex = currentLevel;
        data.UpdateSaveFile = true;
        levelManager.ChangeLevel(data);
    }

    private void RestartGame()
    {
        levelManager.DeleteSavegame();
        StartGame();
    }
}
