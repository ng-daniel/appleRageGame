using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyScript : MonoBehaviour
{

    GameManagerScript game;
    int difficulty = 0;
    int maxDifficulty = 2;
    [SerializeField] TMP_Text display; //assign in inspector
    [SerializeField] TMP_Text game_display;
    [SerializeField] List<string> levels; //assign in inspector

    const string red = "<color=#8E0033>";
    const string white = "<color=#FFFFFF>";

    void Start()
    {
        game = FindFirstObjectByType<GameManagerScript>();
        DisplayDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Vertical") && !game.helpActive && !game.gameStart)
        {
            MoveDifficulty(-1 * (int)Input.GetAxisRaw("Vertical"));
            DisplayDifficulty();
            print(difficulty);
        }
        if (!game_display.text.Equals(GetDifficultyText()))
        {
            game_display.text = GetDifficultyText();
        }
    }
    public int GetDifficulty()
    {
        return difficulty;
    }
    public string GetDifficultyText()
    {
        return difficulty == 2 ? red + levels[difficulty] : levels[difficulty];
    }
    public void MoveDifficulty(int num)
    {
        difficulty += num;
        if (difficulty < 0)
        {
            difficulty = 0;
        }
        if (difficulty > maxDifficulty)
        {
            difficulty = maxDifficulty;
        }
    }
    public string DisplayDifficulty()
    {
        String result = "";
        for (int i = 0; i < levels.Count; i++)
        {
            result += i == difficulty ? red + "> " + levels[i] : white + "  " + levels[i];
            result += "\n";
        }
        display.text = result;
        return result;
    }
}
