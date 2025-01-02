using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    GameBoardScript game;
    bool gameStart;
    bool gameEnd;
    UIManager ui;
    DifficultyScript difficulty;
    float score;

    float startTime;

    JukeboxScript jukebox;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameBoardScript>();
        ui = FindObjectOfType<UIManager>();
        jukebox = FindObjectOfType<JukeboxScript>();
        difficulty = FindObjectOfType<DifficultyScript>();

        Reset();
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
        switch (difficulty.GetDifficulty())
        {
            case 0:
                startTime = 120f;
                break;
            case 1:
                startTime = 30f;
                break;
            case 2:
                startTime = 1f;
                break;
        }

        gameStart = true;
        game.StartGame(startTime);
        ui.GameUI();
        jukebox.PlayGameSong();
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
        jukebox.PlayMenuSong();
    }

}
