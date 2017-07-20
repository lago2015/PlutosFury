using UnityEngine;
using System.Collections;

public class ShatterPiece : MonoBehaviour {

    //for land marker
    public GameObject landGameObject;

    //piece state
    public int PieceState;
    //Markers to let the piece know where to go
    private Vector3 startMarker;
    private Vector3 landMarker;
    private Vector3 returnMarker;
    //turn on and off collider
    private SphereCollider Collider;
    private AudioController audioScript;
    //getter for shatter manager
    public GameObject shatterParent;
    public GameObject shatterModel;
    private ShatterPieceManager shatterManager; 

    //trigger bools
    private bool CapturedLocation;
    private bool attackReady;
    private bool doOnce;
    private bool resetTime;
    private bool shootOnce;
    private bool AttackLanded;
    bool LaunchReady;
    //timers and rates
    private float attackSpeed;
    private float retractSpeed;
    private float startTime;
    private float travelTime;
    private float returnTime;
    private float WaitTime;
    private float curTime;

    void Awake()
    {
        returnMarker = transform.localPosition;
        //assign land marker position
        if(landGameObject)
        {
            landMarker = landGameObject.transform.position;
        }
        if(shatterModel)
        {
            //getter for collider
            Collider = shatterModel.GetComponent<SphereCollider>();
        }
        //getter for audio script
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        //getter for variables and shatter manager
        if(shatterParent)
        {
            shatterManager = shatterParent.GetComponent<ShatterPieceManager>();
            WaitTime = shatterManager.WaitDelay;
            attackSpeed = shatterManager.attackSpeed;
            retractSpeed = shatterManager.retractSpeed;
        }
    }

    void FixedUpdate()
    {
        switch(PieceState)
        {
            case 0://idle?
                resetTime = false;
                break;

            case 1:
                LaunchPiece();
                break;

            case 2:
                RetractPiece();
                break;
        }
    }

    public void SetPiece()
    {
        PieceState = 1;
    }

    public void LaunchPiece()
    {
        Collider.isTrigger = false;

        startMarker = transform.position;

        travelTime = Vector3.Distance(startMarker, landMarker);
        float distanceCovered = (Time.time-startTime) * attackSpeed;
        float fracJourney = distanceCovered / travelTime;
        transform.position = Vector3.Lerp(startMarker, landMarker, fracJourney);
        if (travelTime <= 0.5f)
        {
            
            Collider.isTrigger = true;
            StartCoroutine(WaitToReturn());
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            
            if (!doOnce)
            {
                if (audioScript)
                {
                    audioScript.NeptunesMoonHit(transform.position);
                }
                col.gameObject.GetComponent<Movement>().DamagePluto();

                doOnce = true;
            }
        }
    }

    public void RetractPiece()
    {
       if(!resetTime)
        {
            startTime = Time.time;
            resetTime = true;
        }

        startMarker = transform.position;
        returnTime = Vector3.Distance(startMarker, returnMarker);
        curTime = Time.time;
        float distCov = (curTime - startTime) * attackSpeed;
        float fracJour = distCov / returnTime;
        transform.position = Vector3.Lerp(startMarker, returnMarker, fracJour);
        CapturedLocation = false;
        shootOnce = false;
        if (returnTime <= 0.5f)
        {
            //notify manager the piece has returned
            PieceState = 0;
            shatterManager.PieceReturned();
        }
    }
     IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(WaitTime);
        PieceState = 2;
    
    }
}
