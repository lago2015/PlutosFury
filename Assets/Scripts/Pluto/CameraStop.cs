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

    float bossXMax = 830;
    float bossXMin = 490;

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	GameObject target;
	//Rigidbody targetBody;
	//private float targetMass;
	//private float cachedMass;

	private Vector3 cachedCameraPosition;
    private Vector3 BossPosition = new Vector3(583, 0, 0);
    private float CameraOffset;

	// Use this for initialization
	void Start () 
	{
        target = GameObject.FindGameObjectWithTag("Player");

        CameraOffset = transform.position.z;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, CameraOffset);
		cachedCameraPosition = transform.position;
        

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
    
    public void ChangeToBoss()
    {
        maxX = bossXMax;
        minX = bossXMin;
        float zAxis = gameObject.transform.position.z;
        if(target)
        {
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, zAxis);
        }

        
    }
}
