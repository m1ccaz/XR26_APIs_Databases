using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Databases;

namespace Databases.UI
{
    public class DatabaseTestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button addHighScoreButton;
        [SerializeField] private Button showHighScoresButton;
        [SerializeField] private Button clearDataButton;
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private TMP_InputField scoreInput;

        private void Start()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            if (addHighScoreButton != null)
                addHighScoreButton.onClick.AddListener(OnAddHighScore);

            if (showHighScoresButton != null)
                showHighScoresButton.onClick.AddListener(OnShowHighScores);

            if (clearDataButton != null)
                clearDataButton.onClick.AddListener(OnClearData);

            if (playerNameInput != null)
                playerNameInput.text = "TestPlayer";

            if (scoreInput != null)
                scoreInput.text = "1000";

            UpdateDisplay("Database Test UI Ready\nClick buttons to test database operations");
        }

        private void OnAddHighScore()
        {
            try
            {
                string playerName = playerNameInput?.text ?? "Unknown";
                string scoreText = scoreInput?.text ?? "0";

                if (int.TryParse(scoreText, out int score))
                {
                    GameDataManager.Instance.AddHighScore(playerName, score);

                    UpdateDisplay($"High score added: {playerName} - {score} points");

                    if (scoreInput != null)
                        scoreInput.text = Random.Range(500, 2000).ToString();
                }
                else
                {
                    UpdateDisplay("Error: Invalid score format");
                }
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error adding high score: {ex.Message}");
            }
        }

        private void OnShowHighScores()
        {
            try
            {
                var scores = GameDataManager.Instance.GetTopHighScores();

                if (scores.Count == 0)
                {
                    UpdateDisplay("No high scores found in database");
                }
                else
                {
                    string text = "Top High Scores:\n";
                    for (int i = 0; i < scores.Count; i++)
                    {
                        var score = scores[i];
                        text += $"{i + 1}. {score.PlayerName}: {score.Score} pts\n";
                    }
                    UpdateDisplay(text);
                }
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error loading high scores: {ex.Message}");
            }
        }

        private void OnClearData()
        {
            try
            {
                GameDataManager.Instance.ClearAllHighScores();
                UpdateDisplay("All high scores cleared from database");
            }
            catch (System.Exception ex)
            {
                UpdateDisplay($"Error clearing data: {ex.Message}");
            }
        }

        private void UpdateDisplay(string message)
        {
            if (displayText != null)
                displayText.text = message;
        }
    }
}