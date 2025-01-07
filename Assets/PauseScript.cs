using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    GameBoardScript board;
    [SerializeField] GameObject pauseUI;
    bool paused;
    [SerializeField] AudioSource jukebox;
    void Start()
    {
        board = FindObjectOfType<GameBoardScript>();

        UnPause();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && board.gamestart && !board.gameover)
        {
            paused = paused ? UnPause() : Pause();
        }

        if (!paused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            paused = UnPause();
            board.ShiftTime(-999999999);
        }
    }
    bool Pause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        jukebox.volume = jukebox.volume / 2;
        print("Paused");
        return true;
    }
    bool UnPause()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        jukebox.volume = jukebox.volume * 2;
        print("UnPaused");
        return false;
    }
}
