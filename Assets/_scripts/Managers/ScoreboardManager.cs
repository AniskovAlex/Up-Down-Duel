using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
    [SerializeField] Text scoreboard;
    PlayerController player;
    BotController bot;
    int playerScore = 0; 
    int botScore = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        bot = FindObjectOfType<BotController>();
        player.ScoredAction += OnScore;
        bot.ScoredAction += OnScore;
    }

    public void OnScore(GameObject target)
    {
        if (target == player.gameObject)
        {
            botScore++;
            RedrawScore();
        }
        if (target == bot.gameObject)
        {
            playerScore++;
            RedrawScore();
        }
    }

    void RedrawScore()
    {
        scoreboard.text = "Ñ÷¸ò " + playerScore + ":" + botScore;
    }
}
