using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    GameBoardScript game;
    bool gameStart;
    bool gameEnd;
    UIManager ui;
    float score;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameBoardScript>();
        ui = FindObjectOfType<UIManager>();

        ui.StartUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }


        if (gameStart && !gameEnd)
        {
            score = game.GetCycles() + game.GetBonus();
            ui.UpdateScore(score);
            ui.UpdateTime(game.GetTime());
        }


        if (game.gameover && !gameEnd)
        {
            EndGame();
        }


        if (gameEnd)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Reset();
            }
        }
    }
    void StartGame()
    {
        gameStart = true;
        game.StartGame();
        ui.GameUI();
    }
    void EndGame()
    {
        gameEnd = true;
        ui.EndUI();
    }
    void Reset()
    {
        gameStart = false;
        gameEnd = false;
        game.ResetGame();
        ui.StartUI();
    }
}
