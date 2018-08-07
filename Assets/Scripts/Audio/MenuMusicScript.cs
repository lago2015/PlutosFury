using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour {
    [Header("BackgroundMusic")]
    public AudioSource VoiceIntro;
    public AudioSource BgMusicSource;
    public AudioSource hubMusicSource;
    private float bgMusicStartDelay;
    private float hubMusicDelay = 1;
    private bool isHubOn;
    private Coroutine myCoroutine;
    //This will be called from window manager to determine what song to player
    public bool TurnMusicOn(bool isHubActive)
    {
        //turn on or off the voice intro
        if(isHubActive)
        {
            VoiceIntro.playOnAwake = false;
            VoiceIntro.Stop();
        }
        else
        {
            VoiceIntro.playOnAwake = true;
            VoiceIntro.Play();
        }
        return isHubOn = isHubActive;
    }

    // Use this for initialization
    void Start ()
    {
        //if hub is on then delay any sound for a second then play music
        if(isHubOn)
        {
            bgMusicStartDelay = hubMusicDelay;
            myCoroutine = StartCoroutine(DelayHub());
        }
        //otherwise play voice intro and background music
        else
        {

            BackgroundMusic();
            BgMusicSource.volume = 0.11f;
            if (VoiceIntro != null)
            {
                bgMusicStartDelay = VoiceIntro.clip.length - 1f;
            }
            myCoroutine = StartCoroutine(DelayMusic());
        }
        
	}
    //this will be called from onClick in menu
    public void StartHubMusic()
    {
        BgMusicSource.Stop();
        VoiceIntro.Stop();
        myCoroutine = StartCoroutine(DelayHub());
    }
    //when coming back from the hub use this function to start title screen music
    public void StartBackgroundMusic()
    {

        hubMusicSource.Stop();
        TurnMusicOn(false);
        BackgroundMusic();
        BgMusicSource.volume = 0.11f;
        if (VoiceIntro != null)
        {
            bgMusicStartDelay = VoiceIntro.clip.length - 1f;
        }
        myCoroutine = StartCoroutine(DelayMusic());
    }
    IEnumerator DelayHub()
    {
        yield return new WaitForSeconds(hubMusicDelay);
        HubMusic();
    }
    IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(bgMusicStartDelay);
        if(isHubOn)
        {
            hubMusicSource.Play();
        }
        else
        {
            BgMusicSource.volume = 0.25f;
        }
    }

    //Functions to play hub or background music
    public void HubMusic()
    {
        if (hubMusicSource != null)
        {
            hubMusicSource.loop = true;
            hubMusicSource.Play();
        }
    }
    public void BackgroundMusic()
    {
        if (BgMusicSource != null)
        {
            BgMusicSource.loop = true;
            BgMusicSource.Play();
        }
    }


}
