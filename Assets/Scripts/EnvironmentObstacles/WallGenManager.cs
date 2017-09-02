using UnityEngine;
using System.Collections;

public class WallGenManager : MonoBehaviour {

    public GameObject Models;
    public GameObject ExplosionArt;
    private AudioController audioScript;
    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        if(Models)
        {
            Models.SetActive(true);
        }
        if(ExplosionArt)
        {
            ExplosionArt.SetActive(false);
        }
    }

    public void WallDestroyed()
    {
        if (Models)
        {
            Models.SetActive(false);
        }
        if (ExplosionArt)
        {
            ExplosionArt.SetActive(true);
        }
        if(audioScript)
        {
            audioScript.DestructionSmall(transform.position);
        }
    }
}
