using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject start;
    [SerializeField] GameObject help;
    [SerializeField] GameObject end;
    [SerializeField] GameObject game;
    [SerializeField] TMP_Text time;
    [SerializeField] TMP_Text score_game;
    [SerializeField] TMP_Text score_end;

    const string red = "<color=#8E0033>";
    const string white = "<color=#FFFFFF>";


    public void StartUI()
    {
        start.SetActive(true);
        help.SetActive(false);
        end.SetActive(false);
        game.SetActive(false);
    }
    public void HelpUI()
    {
        start.SetActive(false);
        help.SetActive(true);
        end.SetActive(false);
        game.SetActive(false);
    }
    public void EndUI()
    {
        start.SetActive(false);
        help.SetActive(false);
        end.SetActive(true);
        game.SetActive(false);
    }
    public void GameUI()
    {
        start.SetActive(false);
        help.SetActive(false);
        end.SetActive(false);
        game.SetActive(true);
    }
    public void UpdateScore(float s)
    {
        score_game.text = s.ToString();
        score_end.text = s.ToString();
    }
    public void UpdateTime(float t)
    {
        String timeStr = formatTime(t);
        time.text = t <= 6f ? red + timeStr : timeStr;
    }
    static string formatTime(float time)
    {
        int timeInt = (int)time;
        int minutes = timeInt / 60;
        int seconds = timeInt % 60;

        string zero = "";
        if (seconds < 10)
        {
            zero = "0";
        }

        string str = minutes + ":" + zero + seconds;
        return str;
    }
}
