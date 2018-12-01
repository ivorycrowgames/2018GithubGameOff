using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using IvoryCrow.Unity;

public class SceneReloader : Singleton<SceneReloader>
{
    public GameState ReloadGameState = GameState.Invalid;

    public void ReloadScene(GameState postReloadState)
    {
        ReloadGameState = postReloadState;

        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}


