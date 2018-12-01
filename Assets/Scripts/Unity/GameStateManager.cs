using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Constants;

namespace IvoryCrow.Unity
{
    public enum GameState
    {
        Invalid,
        MainMenu,
        Tutorial,
        InGame,
        GameOver,
        Highscores,
    }

    [System.Serializable]
    public class StateSpecificGameObject
    {
        public GameObject Object;
        public List<GameState> VisibleStates;
    }

    public class GameStateManager : MonoBehaviour
    {
        public GameState InitialGameState;
        public bool StartTutorialIfNoScore = true;

        private GameState currentGameState = GameState.Invalid;
        public GameState CurrentGameState
        {
            get
            {
                return currentGameState;
            }

            set
            {
                changeGameStateInternal(value);
            }
        }

        public List<StateSpecificGameObject> StateSpecificGameObjects;
        private List<GameStateListener> gameStateListeners = new List<GameStateListener>();

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentGameState == GameState.Invalid)
            {
                if (StartTutorialIfNoScore && !PlayerPrefs.HasKey(ScoreManager.HighscoreKey))
                {
                    CurrentGameState = GameState.Tutorial;
                }
                else
                {
                    CurrentGameState = InitialGameState;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void MainMenuState()
        {
            CurrentGameState = GameState.MainMenu;
        }

        public void TutorialState()
        {
            CurrentGameState = GameState.Tutorial;
        }

        public void InGameState()
        {
            CurrentGameState = GameState.InGame;
        }

        public void GameOverState()
        {
            CurrentGameState = GameState.GameOver;
        }

        public void HighScoreState()
        {
            CurrentGameState = GameState.Highscores;
        }

        public void AddGameStateListener(GameStateListener listener)
        {
            gameStateListeners.Add(listener);
        }

        private void changeGameStateInternal(GameState newState)
        {
            foreach (GameStateListener listener in gameStateListeners)
            {
                listener.OnStateEnded(currentGameState);
            }

            currentGameState = newState;

            foreach (StateSpecificGameObject ssgo in StateSpecificGameObjects)
            {
                bool visibleInCurrentState = false;
                foreach (GameState visibleState in ssgo.VisibleStates)
                {
                    if (currentGameState == visibleState)
                    {
                        visibleInCurrentState = true;
                        break;
                    }
                }

                ssgo.Object.SetActive(visibleInCurrentState);
            }

            foreach (GameStateListener listener in gameStateListeners)
            {
                listener.OnStateStarted(currentGameState);
            }
        }
    }
}
