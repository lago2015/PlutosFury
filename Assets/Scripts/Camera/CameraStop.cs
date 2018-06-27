using UnityEngine;
using System.Collections;

public class CameraStop : MonoBehaviour {
	private Vector3 topLeft;
	private Vector3 topRight;
	private Vector3 botLeft;
	private Vector3 botRight;
    public float CameraSpeed=1;
	public float maxX;
	public float maxY;
	public float minX;
	public float minY;
    bool bossChange;
    public float bossXMax = 2360;
    public float bossXMin = 1590;
    public bool cameraPanning;
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public GameObject target;
    //Rigidbody targetBody;
    //private float targetMass;
    //private float cachedMass;

    public GameObject[] cameraStopLocations;
    private float curMaxX;
    private float curMinX;
    public float OffsetX;
    public int curSection;
    //private int maxNumSections;
	private Vector3 cachedCameraPosition;
    private Vector3 wallTransform;
    private Vector3 BossPosition = new Vector3(583, 0, 0);
    private float CameraOffset;
    public Vector3 delta;

    private Vector3 point;
    private Vector3 delta1;
    private Vector3 destination;
    private AsteroidSpawner spawnScript;
    private Camera myCamera;
    private bool levelWallActive;
    public bool isWallActive(bool isActive) { return levelWallActive = isActive; }
	// Use this for initialization
	void Awake () 
	{
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        myCamera = GetComponent<Camera>();
        CameraOffset = transform.position.z;
        if(target)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, CameraOffset);
        }
		cachedCameraPosition = transform.position;
        curSection = 0;
        if(cameraStopLocations.Length>0)
        {

            curMaxX = cameraStopLocations[curSection + 1].transform.position.x - OffsetX;
            curMinX = cameraStopLocations[curSection].transform.position.x + OffsetX;
            maxX = curMaxX;
            minX = curMinX;
            //maxNumSections = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>().NumOfSections();
        }
        //targetBody = target.GetComponent<Rigidbody> ();
        //cachedMass = targetBody.mass;
	}

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

    }

    void FixedUpdate () 
	{
        if (target)
        {
            point = GetComponent<Camera>().WorldToViewportPoint(target.transform.position);
            delta1 = target.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            destination = transform.position + delta1;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        //Camera fitting to viewport
        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));
        botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        botRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));


        if (topLeft.y > maxY || topRight.y > maxY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, cachedCameraPosition.y, transform.position.z);
        }
        if (topRight.x > maxX+OffsetX || botRight.x > maxX+OffsetX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(cachedCameraPosition.x, transform.position.y, transform.position.z);
        }
        if (topLeft.x < minX-OffsetX || botLeft.x < minX - OffsetX)
        {
            if (levelWallActive)
            {
                transform.position = new Vector3(minX + OffsetX, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(cachedCameraPosition.x, transform.position.y, transform.position.z);
            }
        }

        if (botLeft.y < minY || botRight.y < minY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, cachedCameraPosition.y, transform.position.z);
        }
        cachedCameraPosition = transform.position;
    }
 
    public void EnableCamera()
    {
        OffsetX = 50;
    }   
    
    public float incrementCurSection()
    {
        curSection++;
        
        return curSection;
    }
    public void MoveCamera(Vector3 curPosition)
    {
        transform.position = new Vector3(curPosition.x, curPosition.y, CameraOffset);
    }
    public float ChangeCamMin()
    {
        //get Min X from the left wall
        minX = cameraStopLocations[curSection].transform.position.x + OffsetX;
        //update cache camera position
        cachedCameraPosition.x = minX;
        //update orbs where to spawn
        spawnScript.newMinX(minX);
        return minX;
    }
    public float ChangeCamMax()
    {

        //get max X from the right wall
        maxX = cameraStopLocations[curSection + 1].transform.position.x - OffsetX;
        //update orbs where to spawn
        spawnScript.newMaxX(maxX);
        return maxX;
    }

}
