using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public bool ShouldMove = false;

    private float rotationSpeed = 5;
    private GameObject Player;
    
    private SphereCollider TriggerCollider;
    private float startRadius;
    public float DistanceFromPlayerToExplode = 7f;
    private DetectWaitThenExplode explodeScript;


    //To scale object variables
    public GameObject ObjectToScale;
    private Vector3 currentScale;
    private Vector3 scaleToVector;
    public float lerpSpeed;
    public float scaleToNumber=3.5f;
    public float explosionTime;
    private bool timeToLerp;
    private float timeToScale;
    private float scaleCovered;
    private float AmountOfScaleCompleted;
    private float startTime;
    public bool AmILerping() { return timeToLerp; }
    public bool activateMovement(bool isActive)
    {
        enabled = isActive;

        return ShouldMove = isActive;
    }

    void Awake()
    {
        currentScale = ObjectToScale.transform.localScale;
        scaleToVector = new Vector3(scaleToNumber, scaleToNumber, scaleToNumber);
        timeToScale = Vector3.Distance(currentScale, scaleToVector);
        enabled = false;
        explodeScript = GetComponent<DetectWaitThenExplode>();
        TriggerCollider = GetComponent<SphereCollider>();
        if(TriggerCollider)
        {
            TriggerCollider.enabled = true;
            startRadius = TriggerCollider.radius;
        }
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    //move towards the plaey
    void FixedUpdate()
    {

        if (timeToLerp)
        {
            scaleCovered = (Time.time - startTime) * lerpSpeed;
            AmountOfScaleCompleted = scaleCovered / timeToScale;
            ObjectToScale.transform.localScale = Vector3.Lerp(currentScale, scaleToVector, scaleCovered);
        }
        else if (ShouldMove)
        {
            //calculate distance between player and rogue
            float curDistance = Vector3.Distance(transform.position, Player.transform.position);
            //check if player is close enough, if not then pursue
            if (curDistance > DistanceFromPlayerToExplode)
            {
                Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

                transform.parent.position += moveSpeed * transform.forward * Time.deltaTime;
                transform.parent.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
            else
            {
                EnableLerp();
            }
        }
    }

    public void EnableLerp()
    {
        StartCoroutine(ExpandMothafucka());   
    }

    IEnumerator ExpandMothafucka()
    {
        yield return new WaitForSeconds(0.25f);
        startTime = Time.time;
        timeToLerp = true;
        gameObject.tag = "Wall";
        yield return new WaitForSeconds(explosionTime);
        if (explodeScript)
        {
            explodeScript.TriggeredExplosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //disable movement if player is near
        string curString = other.gameObject.tag;
        if (curString == "Player"||curString == "BreakableWall"||curString=="EnvironmentObstacle"||curString=="BigAsteroid")
        {
            activateMovement(false);
        }
    }

}
