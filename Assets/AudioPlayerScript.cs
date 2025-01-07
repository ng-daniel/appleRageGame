using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerScript : MonoBehaviour
{
    [SerializeField] public AudioSource src;
    [SerializeField] List<AudioClip> clips;

    public void PlayClip(int num)
    {
        src.clip = clips[num];
        src.Play();
    }
    public void PlayClip(int num, float volume)
    {
        src.clip = clips[num];
        src.volume = volume;
        src.Play();
    }
    public void ToggleLoop(bool val)
    {
        src.loop = val;
    }
}
