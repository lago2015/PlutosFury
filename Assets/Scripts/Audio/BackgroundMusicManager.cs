using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField]
    [Header("Background-Music")]
    public AudioSource BgMusicSource;
    [SerializeField]
    [Header("Background-WinMusic")]
    public AudioSource BgMusicWinSource;
    [SerializeField]
    [Header("Intro-Ready")]
    public AudioSource ReadyAudioSource;

    [SerializeField]
    [Header("Intro-Go")]
    public AudioSource GoAudioSource;
    [SerializeField]
    [Header("Complete Level Voice Cue")]
    public AudioSource CompleteSource;
    [SerializeField]
    [Header("Game Over Voice Cue")]
    public AudioSource gameOverVoiceSource;


    private void Start()
    {
        if(BgMusicSource)
        {
            BgMusicSource.volume = 0.07f;
            BackgroundMusic();
            StartCoroutine(DelayVolume());
        }
    }

    IEnumerator DelayVolume()
    {
        yield return new WaitForSeconds(2f);
        BgMusicSource.volume = 0.25f;
    }

    public void BackgroundMusic()
    {
        if (BgMusicSource != null)
        {
            BgMusicSource.priority = 200;
            BgMusicSource.minDistance = 1000f;
            BgMusicSource.loop = true;
            BgMusicSource.Play();
        }
    }

    public void BackgroundWinMusic()
    {
        if (BgMusicWinSource != null)
        {
            BgMusicSource.Stop();
            BgMusicWinSource.priority = 200;
            BgMusicWinSource.minDistance = 1000f;
            BgMusicWinSource.loop = false;
            BgMusicWinSource.Play();
        }
    }


    public void StartReadyIntro()
    {
        if (ReadyAudioSource != null)
        {
            ReadyAudioSource.priority = 200;
            ReadyAudioSource.minDistance = 1000f;
            ReadyAudioSource.loop = false;
            ReadyAudioSource.Play();
        }
    }
    public void StartGoIntro()
    {
        if (GoAudioSource != null)
        {
            GoAudioSource.priority = 200;

            GoAudioSource.minDistance = 1000f;
            GoAudioSource.loop = false;
            GoAudioSource.Play();
        }
    }
    public void CompleteLevel()
    {
        if (CompleteSource != null)
        {
            CompleteSource.priority = 200;

            CompleteSource.minDistance = 1000f;
            CompleteSource.loop = false;
            CompleteSource.Play();
        }
    }
    public void GameOverVoiceCue()
    {
        if (gameOverVoiceSource != null)
        {
            BgMusicSource.Stop();
            gameOverVoiceSource.priority = 200;

            gameOverVoiceSource.minDistance = 1000f;
            gameOverVoiceSource.loop = false;
            gameOverVoiceSource.Play();
        }
    }

}
