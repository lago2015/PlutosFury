using UnityEngine;
using System.Collections;

public class SaturnRing : MonoBehaviour
{
    //State Machine
    public int RingState;
    
    Transform Player;
    public GameObject Saturn;
    bool CapturedLocation;
    bool resetTime;

    //attacking and returning positions
    Vector3 startMarker;
    Vector3 endMarker;
    Vector3 returnMarker;
    Vector3 CurrentOffset;
    //Determine location to attack and retract
    public float attackSpeed;
    float startTime;
    float travelTime;
    float returnTime;
    public float WaitTime;

    //Attack Timers
    bool LaunchReady = true;
    bool RingReady;
    public float AttackRate;
    public float AttackTime;

    //variables for wrap around
    public float maxX;
    public float minX;

    public float maxY;
    public float minY;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Saturn = GameObject.FindGameObjectWithTag("Saturn").gameObject;
        CurrentOffset = Saturn.GetComponent<NepMoonReturn>().GetCurrentLocation();
        LaunchReady = true;
        RingReady = false;
        startTime = Time.time;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (RingState)
        {
            //Launching at Pluto
            case 0:
                transform.position = Saturn.transform.position;
                break;
            case 1:
                LaunchRing();
                break;
            case 2:
                RetractRing();
                break;
            
        }
    }

    void RotateToNeptune()
    {

        if (!RingReady)
        {
            if (AttackTime >= AttackRate)
            {
                LaunchReady = true;
                RingReady = true;
                AttackTime = 0;
            }
            else
            {
                AttackTime += Time.deltaTime;
            }
        }


        CapturedLocation = false;
        resetTime = false;
    }
    public void ActivateRing()
    {
        if (RingReady)
        {
            if (RingState == 0)
            {
                RingState = 1;
            }
        }
    }

    void LaunchRing()
    {
        if (LaunchReady)
        {
            if (!CapturedLocation)
            {
                endMarker = Player.transform.position;
                CapturedLocation = true;
                RingReady = false;
            }

            startMarker = transform.position;

            travelTime = Vector3.Distance(startMarker, endMarker);
            float distanceCovered = (Time.time - startTime) * attackSpeed;
            float fracJourney = distanceCovered / travelTime;
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
            if (travelTime <= 0.5f)
            {
                StartCoroutine(WaitToReturn());
                //MoonState = 2;
            }
        }
    }



    void RetractRing()
    {
        if (!resetTime)
        {
            startTime = Time.time;
            resetTime = true;
        }

        startMarker = transform.position;
        returnMarker = Saturn.transform.position + CurrentOffset;
        returnTime = Vector3.Distance(startMarker, returnMarker);
        float distCov = (Time.time - startTime) * attackSpeed;
        float fracJour = distCov / returnTime;
        transform.position = Vector3.Lerp(startMarker, returnMarker, fracJour);
        CapturedLocation = false;
        if (returnTime <= 0.5f)
        {
            RingState = 0;
        }
    }


    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(WaitTime);

        RingState = 2;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            RingState = 2;
        }
    }



    void BorderWrap()
    {
        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, minY, 0.0f);
        }
        else if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, maxY, 0.0f);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(minX, transform.position.y, 0.0f);
        }
        else if (transform.position.x < minX)
        {
            transform.position = new Vector3(maxX, transform.position.y, 0.0f);
        }
    }
}
