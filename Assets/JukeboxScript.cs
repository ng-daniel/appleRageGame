using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeboxScript : MonoBehaviour
{
    public AudioSource src;
    public AudioClip menuSong;
    public AudioClip gameSong;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.loop = true;
        src.playOnAwake = false;
    }
    public void Play(AudioClip clip)
    {
        src.clip = clip;
        src.Play();
    }
    public void PlayMenuSong()
    {
        Play(menuSong);
    }
    public void PlayGameSong()
    {
        Play(gameSong);
    }
}
