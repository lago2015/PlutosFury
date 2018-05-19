﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour {
    [Header("BackgroundMusic")]
    public AudioSource VoiceIntro;
    public AudioSource BgMusicSource;
    public AudioSource LoopSource;
    private float bgMusicDelay;
    private float bgMusicStartDelay;

    // Use this for initialization
    void Start () {
        //instance = this;
        if(BgMusicSource!=null)
        {
            bgMusicDelay = BgMusicSource.clip.length;
        }
        if(VoiceIntro!=null)
        {
            bgMusicStartDelay = VoiceIntro.clip.length-1f;
        }
        StartCoroutine(DelayMusic());
	}

    IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(bgMusicStartDelay);
        BackgroundMusic();
    }

    public void BackgroundMusic()
    {
        if (BgMusicSource != null)
        {
            BgMusicSource.priority = 200;
            BgMusicSource.volume = 0.25f;
            BgMusicSource.minDistance = 1000f;
            BgMusicSource.loop = false;
            BgMusicSource.Play();
            StartCoroutine(waitToLoop());
        }
    }

    IEnumerator waitToLoop()
    {
        yield return new WaitForSeconds(bgMusicDelay);
        BgMusicSource.Stop();
        LoopSource.loop = true;
        LoopSource.Play();
        
    }
}
