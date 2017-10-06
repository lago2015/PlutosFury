using UnityEngine;
using System.Collections;

public class ShatterPieceManager : MonoBehaviour
{

    private AudioController audioScript;
    public GameObject[] ShatterPieces;

    public int PieceState;
    private int piecesReturned;

    public float attackSpeed;
    public float retractSpeed;
    public float attackRate;
    private float attackTimer;
    public float WaitDelay;
    private bool shootOnce;
    private bool playerNear;
    private bool LaunchReady;

    void Awake()
    {
        LaunchReady = true;
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }
    void LaunchPieces()
    {
        for (int i = 0; i <= ShatterPieces.Length-1; ++i)
        {
            ShatterPieces[i].GetComponent<ShatterPiece>().SetPiece();
        }
        LaunchReady = false;
        if (audioScript)
        {
            //if(!shootOnce)
            //{
            //    shootOnce = true;
            //    audioScript.NeptunesMoonShot(transform.position);
            //}
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            playerNear = true;
            if(LaunchReady)
            {
                StartCoroutine(WaitToAttack());
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            playerNear = false;
        }
    }

    public void PieceReturned()
    {
        piecesReturned++;
        if(piecesReturned==ShatterPieces.Length)
        {
            piecesReturned = 0;
            LaunchReady = true;
            if(playerNear)
            {
                StartCoroutine(WaitToAttack());
            }
        }
    }

    IEnumerator WaitToAttack()
    {
        shootOnce = false;
        yield return new WaitForSeconds(attackRate);
        LaunchPieces();


    }
}

