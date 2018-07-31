using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicScript : MonoBehaviour {
    [Header("BackgroundMusic")]
    public AudioSource VoiceIntro;
    public AudioSource BgMusicSource;
    
    private float bgMusicStartDelay;

    // Use this for initialization
    void Start () {

        
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
            BgMusicSource.loop = true;
            BgMusicSource.Play();

        }
    }


}
