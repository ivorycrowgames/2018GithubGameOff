using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using IvoryCrow.Utilities;
using IvoryCrow.Constants;

namespace IvoryCrow.Unity
{
    public class ScoreManager : MonoBehaviour, GameStateListener
    {
        public GameStateManager Manager;

        public const string HighscoreKey = "score";
        private int currentScore;

        // Use this for initialization
        void Start()
        {
            Throw.IfNull(Manager, "Score Manager needs a reference to game manager");
            Manager.AddGameStateListener(this);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnStateEnded(GameState state)
        {
        }

        public void OnStateStarted(GameState state)
        {
            if (state == GameState.InGame)
            {
                currentScore = 0;
            }
            else if (state == GameState.GameOver)
            {
                int oldHighscore = PlayerPrefs.GetInt(HighscoreKey, 0);
                if (currentScore > oldHighscore)
                {
                    PlayerPrefs.SetInt(HighscoreKey, currentScore);
                }
            }
        }

        public void ResetScore()
        {
            currentScore = 0;
        }

        public void AddScore(int points)
        {
            currentScore += points;
        }

        public int GetScore()
        {
            return currentScore;
        }

        public int GetHighscore()
        {
            return PlayerPrefs.GetInt(HighscoreKey, 0);
        }

        public bool HasHighscore()
        {
            return PlayerPrefs.HasKey(HighscoreKey);
        }
    }
}


