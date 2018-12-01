using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using IvoryCrow.Utilities;

[Serializable]
public class LevelMapping
{
    public string LevelName;
    public int BuildIndex;
}

public class LevelManager : SimpleEntity {

    public List<LevelMapping> Levels;

    private int _currentSceneIndex;

    private bool _hasLoadedSave = false;
    private bool _noSaveData = true;
    private SaveGameData _currentSaveGameData;

    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        AddActionHandler<LevelChangeData>(ChangeLevel);
        AddActionHandler<CheckpointData>(StoreCheckpoint);
    }

    public void ChangeLevel(LevelChangeData data)
    {
        int targetScene = data.LevelBuildIndex;
        if (data.UpdateSaveFile)
        {
            SaveGameData newSave = new SaveGameData(CurrentSavegameVersion, targetScene);
            updateSavegameData(newSave);
        }

        SceneManager.LoadScene(targetScene);
    }

    private void StoreCheckpoint(CheckpointData data)
    {
        refreshSavedataCache();

        if (_noSaveData)
        {
            updateSavegameData(newSaveForCurrentScene());
        }

        _currentSaveGameData.hasCheckpointInLevel = true;
        _currentSaveGameData.checkpointPosition = data.RespawnPosition.transform.position;
        updateSavegameData(_currentSaveGameData);
    }

    public void StoreNote(string noteName)
    {
        refreshSavedataCache();

        if (_noSaveData)
        {
            updateSavegameData(newSaveForCurrentScene());
        }

        if (!_currentSaveGameData.collectedNotes.Contains(noteName))
        {
            _currentSaveGameData.collectedNotes.Add(noteName);
        }
       
        updateSavegameData(_currentSaveGameData);
    }

    private SaveGameData newSaveForCurrentScene()
    {
        int curScene = SceneManager.GetActiveScene().buildIndex;
        SaveGameData newSave = new SaveGameData(CurrentSavegameVersion, curScene);
        return newSave;
    }

    public bool GetLastCheckpoint(out Vector2 pos)
    {
        pos = Vector2.zero;
        refreshSavedataCache();

        if (!_noSaveData && _currentSaveGameData.hasCheckpointInLevel)
        {
            pos = _currentSaveGameData.checkpointPosition;
            return true;
        }

        return false;
    }

    public bool GetCollectedNotes(out List<string> notes)
    {
        notes = new List<string>();
        refreshSavedataCache();

        if (!_noSaveData)
        {
            notes = _currentSaveGameData.collectedNotes;
            return true;
        }

        return false;
    }

    public void DeleteSavegame()
    {
        PlayerPrefs.DeleteKey(playerSavegameKey);
        refreshSavedataCache(true);
    }

    public int GetCurrentLevel()
    {
        refreshSavedataCache();
        if (_noSaveData)
        {
            Throw.IfTrue(Levels.Count == 0, "No levels in level manager");
            return Levels[0].BuildIndex;
        }

        return _currentSaveGameData.sceneBuildIndex;
    }

    public void ReloadActiveLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    private void updateSavegameData(SaveGameData data)
    {
        PlayerPrefs.SetString(playerSavegameKey, JsonUtility.ToJson(data));
        refreshSavedataCache(true);
    }

    private void refreshSavedataCache(bool forceRefresh = false)
    {
        if (!_hasLoadedSave || forceRefresh)
        {
            _currentSaveGameData = default(SaveGameData);
            if (PlayerPrefs.HasKey(playerSavegameKey))
            {
                string saveGame = PlayerPrefs.GetString(playerSavegameKey);
                _currentSaveGameData = JsonUtility.FromJson<SaveGameData>(saveGame);
                _noSaveData = false;
            }
            else
            {
                _noSaveData = true;
            }

            _hasLoadedSave = true;
        }
    }

    private const string playerSavegameKey = "savedata";
    private const int CurrentSavegameVersion = 1;

    [Serializable]
    struct SaveGameData
    {
        public int saveGameVersion;
        public int sceneBuildIndex;

        public bool hasCheckpointInLevel;
        public Vector2 checkpointPosition;

        public List<string> collectedNotes;

        public SaveGameData(int version, int sceneIndex)
        {
            saveGameVersion = version;
            sceneBuildIndex = sceneIndex;
            hasCheckpointInLevel = false;
            checkpointPosition = Vector2.zero;
            collectedNotes = new List<string>();
        }
    }
}
