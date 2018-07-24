using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : MonoBehaviour {

    //these states are for particles system effects on player
    public enum BusterStates { Pickup, Shockwave, Death }
    private BusterStates busterState;
    [Tooltip("0=Pickup, 1=ShockwaveBurst, 2=Death")]
    public GameObject[] busterStates;

    //animation things
    public Animator animComp;
    private Movement moveScript;
    //audio
    private AudioController audioScript;

    //Game manager
    private GameManager gameManager;
    private void Awake()
    {
        foreach (GameObject prefab in busterStates)
        {
            prefab.SetActive(false);
        }
        gameManager = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
    }
    private void Start()
    {
        moveScript = GetComponent<Movement>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();

    }
    public void PlayerDied()
    {
        animComp.enabled = false;
        BusterChange(BusterStates.Death);
        gameManager.GameEnded(true);
        audioScript.PlutoDeath(transform.position);
    }

    //function for changing buster states depending on overload
    public void BusterChange(BusterStates nextState)
    {
        busterState = nextState;


        switch (busterState)
        {
            //share delay with shockwave
            case BusterStates.Pickup:
                if (busterStates[0])
                {
                    busterStates[0].SetActive(true);
                    StartCoroutine(BusterTransition(busterStates[0]));
                }
                break;
            //turn off after shockwave so delay
            case BusterStates.Shockwave:
                if (busterStates[1])
                {
                    busterStates[1].SetActive(true);
                    StartCoroutine(BusterTransition(busterStates[1]));

                }
                break;
            //doesnt turn off
            case BusterStates.Death:
                if (busterStates[2])
                {
                    moveScript.DisableMovement(true);

                    foreach (SphereCollider col in GetComponents<SphereCollider>())
                    {
                        if (!col.isTrigger)
                        {
                            col.isTrigger = true;
                        }
                    }
                    busterStates[2].SetActive(true);
                }
                break;
        }
    }

    IEnumerator BusterTransition(GameObject curObject)
    {
        yield return new WaitForSeconds(1f);
        curObject.SetActive(false);
    }
}
