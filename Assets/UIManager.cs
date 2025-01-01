using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject start;
    [SerializeField] GameObject end;
    [SerializeField] GameObject game;
    [SerializeField] TMP_Text time;
    [SerializeField] TMP_Text score_game;
    [SerializeField] TMP_Text score_end;


    public void StartUI()
    {
        start.SetActive(true);
        end.SetActive(false);
        game.SetActive(false);
    }
    public void EndUI()
    {
        start.SetActive(false);
        end.SetActive(true);
        game.SetActive(false);
    }
    public void GameUI()
    {
        start.SetActive(false);
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
        time.text = formatTime(t);
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
