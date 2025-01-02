using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyScript : MonoBehaviour
{

    int difficulty = 0;
    int maxDifficulty = 2;
    [SerializeField] TMP_Text display; //assign in inspector
    [SerializeField] List<string> levels; //assign in inspector

    const string red = "<color=#8E0033>";
    const string white = "<color=#FFFFFF>";

    void Start()
    {
        DisplayDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {

            MoveDifficulty(-1 * (int)Input.GetAxisRaw("Vertical"));
            DisplayDifficulty();
            print(difficulty);
        }
    }
    public int GetDifficulty()
    {
        return difficulty;
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
