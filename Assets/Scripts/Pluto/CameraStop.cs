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
	GameObject target;
    //Rigidbody targetBody;
    //private float targetMass;
    //private float cachedMass;

    public GameObject[] cameraStopLocations;
    private float curMaxX;
    private float curMinX;
    public float OffsetX;
    private int curSection;
    private int maxNumSections;
	private Vector3 cachedCameraPosition;
    private Vector3 wallTransform;
    private Vector3 BossPosition = new Vector3(583, 0, 0);
    private float CameraOffset;
    public Vector3 delta;
    private AsteroidSpawner spawnScript;
    private Camera myCamera;

	// Use this for initialization
	void Awake () 
	{
        target = GameObject.FindGameObjectWithTag("Player");
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
            maxNumSections = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SectionManager>().NumOfSections();
        }
        //targetBody = target.GetComponent<Rigidbody> ();
        //cachedMass = targetBody.mass;
	}
	
 
	void FixedUpdate () 
	{
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.transform.position);

            Vector3 delta1 = target.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta1;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        //Camera fitting to viewport
        topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));
        botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        botRight = Camera.main.ScreenToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));
        if (topRight.x > maxX || botRight.x > maxX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(cachedCameraPosition.x, transform.position.y, transform.position.z);
        }


        if (topLeft.y > maxY || topRight.y > maxY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, cachedCameraPosition.y, transform.position.z);
        }
        if (topLeft.x < minX || botLeft.x < minX )
        { 
            transform.position = new Vector3(minX+OffsetX, transform.position.y, transform.position.z);
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
    public void ChangeToBoss(Vector3 curPos)
    {
        if(curSection<=maxNumSections)
        {
            //increment section numbe
            curSection++;
            //calculate new min and max for X axis for camera stop
            curMaxX = cameraStopLocations[curSection + 1].transform.position.x - OffsetX;
            curMinX = cameraStopLocations[curSection].transform.position.x + OffsetX;
            //apply new min and max for X axis
            maxX = curMaxX;
            minX = curMinX;
            spawnScript.SpawnIntoNewSection(minX, maxX);
            cachedCameraPosition = curPos;
            

        }
    }
}
