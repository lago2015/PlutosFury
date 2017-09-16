using UnityEngine;
using System.Collections;

public class CameraStop : MonoBehaviour {
	private Vector3 topLeft;
	private Vector3 topRight;
	private Vector3 botLeft;
	private Vector3 botRight;

	public float maxX;
	public float maxY;
	public float minX;
	public float minY;
    bool bossChange;
    public float bossXMax = 2360;
    public float bossXMin = 1590;

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	GameObject target;
    //Rigidbody targetBody;
    //private float targetMass;
    //private float cachedMass;

    public GameObject[] cameraStopLocations;
    private float curMaxX;
    private float curMinX;
    private float OffsetX = 5f;
    private int curSection;
    private int maxNumSections;
	private Vector3 cachedCameraPosition;
    private Vector3 BossPosition = new Vector3(583, 0, 0);
    private float CameraOffset;
    private AsteroidSpawner spawnScript;
    
	// Use this for initialization
	void Awake () 
	{
        target = GameObject.FindGameObjectWithTag("Player");
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
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
	
 
	// Update is called once per frame
	void FixedUpdate () 
	{
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.transform.position);

            Vector3 delta = target.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));
        botLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
        botRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, -Camera.main.transform.position.z));

        if (topLeft.y > maxY || topRight.y > maxY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, cachedCameraPosition.y, transform.position.z);
        }
        if (topLeft.x < minX || botLeft.x < minX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(cachedCameraPosition.x, transform.position.y, transform.position.z);
        }
        if (topRight.x > maxX || botRight.x > maxX)
        {
            //Clamp Camera along the X axis
            transform.position = new Vector3(cachedCameraPosition.x, transform.position.y, transform.position.z);
        }
        if (botLeft.y < minY || botRight.y < minY)
        {
            //Clamp Camera along the Y axis
            transform.position = new Vector3(transform.position.x, cachedCameraPosition.y, transform.position.z);
        }
            cachedCameraPosition = transform.position;
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
