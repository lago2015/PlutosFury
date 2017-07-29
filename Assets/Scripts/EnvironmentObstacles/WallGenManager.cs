using UnityEngine;
using System.Collections;

public class WallGenManager : MonoBehaviour {

    public GameObject Models;
    public GameObject ExplosionArt;

    void Awake()
    {
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
    }
}
