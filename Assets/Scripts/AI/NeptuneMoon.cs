using UnityEngine;
using System.Collections;

public class NeptuneMoon : MonoBehaviour {

	public int MoonState;
    public GameObject trailModel;
	public GameObject RotateToGameObject;
	public float RotateSpeed=50;
    //public Vector3 MoonOffset;

    //Shake properties
    private float shake = 0.0f;
    public float shakeAmount;
    public float decreaseFactor;
    private Vector3 startPosition;
    private Vector3 shakeVector;



    private AudioController audioScript;
    Transform Player;
    GameObject Neptune;
    bool CapturedLocation;
    bool resetTime;
    bool GetLocation;
    bool doOnce;
    bool shootOnce;
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
    bool isCharging;
    float chargeTimer = 0.5f;
    bool LaunchReady=true;
    bool MoonReady;
    public float AttackRate;
    public float AttackTime;

	//variables for wrap around
	public float maxX;
	public float minX;

	public float maxY;
	public float minY;


    void Awake()
    {
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }

	// Use this for initialization
	void Start () 
	{
        if(!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
		if(trailModel)
        {
            trailModel.SetActive(false);
        }
        if(!Neptune)
        {
            Neptune = GameObject.FindGameObjectWithTag("Neptune").gameObject;
            CurrentOffset = Neptune.GetComponent<NepMoonReturn>().GetCurrentLocation();
        }

        
        LaunchReady = true;
        MoonReady = false;
        startTime = Time.time;

    }

    // Update is called once per frame
    void FixedUpdate () 
	{
		switch (MoonState) 
		{
		//Rotating around Neptune
		case 0:
			RotateToNeptune ();
			break;
            //Charge up moon
		case 1:
                ChargeMoon();
			break;
            //Launching at Pluto
            case 2:
                LaunchMoon();
            break;
            //Returning to Neptune to then rotate around
            case 3:
                RetractMoon();
                break;
		}
    }

	void RotateToNeptune()
	{
		if(RotateToGameObject)
		{
			this.gameObject.transform.RotateAround(RotateToGameObject.transform.position, gameObject.transform.right, RotateSpeed * Time.deltaTime);
		}
        if(MoonReady==false)
        {
            if (AttackTime >= AttackRate)
            {
                LaunchReady = true;
                MoonReady = true;
                AttackTime = 0;
            }
            else
            {
                AttackTime += Time.deltaTime;
            }
        }

        
        GetLocation = false;
        CapturedLocation = false;
        resetTime = false;
	}
    public void ActivateMoon()
    {
        if(MoonReady)
        {
            
            if (MoonState == 0)
            {
                MoonState = 2;
            }
        }
    }

    void ChargeMoon()
    {
        if(isCharging)
        {
            isCharging = true;
            StartCoroutine(MoonCharging());
        }
        if (shake > 0.0f)
        {
            startPosition = transform.localPosition;
            shakeVector = startPosition + Random.insideUnitSphere * shakeAmount;
            shakeVector.z = 0f;
            transform.localPosition = shakeVector;
            shake -= Time.deltaTime * decreaseFactor;
        }
        transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);

    }

    IEnumerator MoonCharging()
    {
        yield return new WaitForSeconds(chargeTimer);

        shake = 0.0f;
        startPosition = Vector3.zero;
        shakeVector = Vector3.zero;

        MoonState = 2;
    }

    void LaunchMoon()
	{
        if(LaunchReady)
        {
            if(audioScript)
            {
                if(!shootOnce)
                {
                    audioScript.NeptunesMoonShot(transform.position);
                    shootOnce = true;
                }
            }
            if (!CapturedLocation)
            {
                endMarker = Player.transform.position;
                CapturedLocation = true;
                MoonReady = false;
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

    void ReturnCoordinates()
    {
        CurrentOffset=Neptune.GetComponent<NepMoonReturn>().SetCurrentLocation(CurrentOffset);
    }

	void RetractMoon()
	{
        if(audioScript)
        {
            audioScript.NeptunesMoonRetract(transform.position);
        }
        if(!resetTime)
        {
            startTime = Time.time;
            resetTime = true;
        }
        if(!GetLocation)
        {
            ReturnCoordinates();
            
            GetLocation = true;
        }
        startMarker = transform.position;
        returnMarker = RotateToGameObject.transform.position + CurrentOffset;
        returnTime = Vector3.Distance(startMarker, returnMarker);
        float distCov = (Time.time - startTime) * attackSpeed;
        float fracJour = distCov / returnTime;
        transform.position = Vector3.Lerp(startMarker, returnMarker, fracJour);
        CapturedLocation = false;
        shootOnce = false;
        if (returnTime <= 0.5f)
        {
            MoonState = 0;
        }
    }



    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(WaitTime);
        
        MoonState = 3;
    }

	void OnTriggerEnter(Collider col)
	{
        if(col.gameObject.tag==("Player"))
        {
            MoonState = 2;
            if(!doOnce)
            {
                if(audioScript)
                {
                    audioScript.NeptunesMoonHit(transform.position);
                }
                col.gameObject.GetComponent<Movement>().DamagePluto();
              
                doOnce = true;
            }
        }
	}

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            doOnce = false;
        }
    }

	
    
}
