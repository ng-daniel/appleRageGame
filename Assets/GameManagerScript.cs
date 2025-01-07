using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    GameBoardScript game;
    public bool helpActive { get; private set; }
    public bool gameStart { get; private set; }
    bool gameEnd;
    UIManager ui;
    DifficultyScript difficulty;
    float score;

    float startTime;

    JukeboxScript jukebox;
    AudioPlayerScript audioplayer;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameBoardScript>();
        ui = FindObjectOfType<UIManager>();
        jukebox = FindObjectOfType<JukeboxScript>();
        difficulty = FindObjectOfType<DifficultyScript>();
        audioplayer = GetComponentInChildren<AudioPlayerScript>();

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioplayer.PlayClip(0);
                if (!helpActive)
                {
                    ui.HelpUI();
                    helpActive = true;
                }
                else
                {
                    StartGame();
                    helpActive = false;
                }
            }
            if (!helpActive && Input.GetKeyDown(KeyCode.Escape))
            {
                audioplayer.PlayClip(0);
                print("quitting!");
                Application.Quit();
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
                audioplayer.PlayClip(0);
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
                startTime = 45f;
                break;
            case 2:
                startTime = 5f;
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
