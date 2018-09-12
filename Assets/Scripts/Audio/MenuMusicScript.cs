using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour {
    [Header("BackgroundMusic")]
    public AudioSource VoiceIntro;
    public AudioSource BgMusicSource;
    private float bgMusicStartDelay;
    private bool isHubOn;
    private Coroutine myCoroutine;
    //This will be called from window manager to determine what song to player
    public bool TurnMusicOn(bool isHubActive)
    {
        return isHubOn = isHubActive;
    }

    // Use this for initialization
    void Start ()
    {
        //if hub is on then delay any sound for a second then play music
        if(!isHubOn)
        {
            VoiceIntro.Play();
            StartBackgroundMusic();
           
        }
        else

        {
            VoiceIntro.Stop();
            BackgroundMusic();
        }
        
	}
 
    //when coming back from the hub use this function to start title screen music
    public void StartBackgroundMusic()
    {
        if(myCoroutine!=null)
        {
            StopCoroutine(myCoroutine);
        }
        TurnMusicOn(false);
        BackgroundMusic();
        BgMusicSource.volume = 0.11f;
        if (VoiceIntro != null)
        {
            bgMusicStartDelay = VoiceIntro.clip.length - 0.5f;
        }
        myCoroutine = StartCoroutine(DelayMusic());
    }
    


    IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(bgMusicStartDelay);
        BgMusicSource.volume = 0.25f;

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
