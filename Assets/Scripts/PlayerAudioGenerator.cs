using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAudioIndex { PUNCH = 0, JUMP, KICK, DEATH };

public class PlayerAudioGenerator : MonoBehaviour
{

    public AudioClip[] audioClips;

    private AudioSource[] audioSources;

    void Start()
    {
        InitAudioSources();
    }

    void Update()
    {
        //
    }

    private void InitAudioSources()
    {
        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioSources[i] = GameObject.FindWithTag("Player").AddComponent<AudioSource>();
            audioSources[i].clip = audioClips[i];
        }
    }

    public void PlaySound(PlayerAudioIndex index)
    {
        audioSources[(int) index].Play();
    }

}



