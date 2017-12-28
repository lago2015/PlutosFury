using UnityEngine;
using System.Collections;

public class BurstBehavior : MonoBehaviour {

    private float moveSpeed = 7;
    private float BurstTimeout = 0.25f;
    public bool ShouldBurst=false;
    private SphereCollider myCollider;
    private Rigidbody myBody;
    private bool isNewAsteroid=true;
    public bool ReadyToConsume;
    public bool newSpawnedAsteroid(bool isNew) { return isNewAsteroid = isNew; }
    public bool asteroidStatus() { return isNewAsteroid; }
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myCollider = GetComponent<SphereCollider>();
    }

    void Start()
    {
        //myBody.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);

        StartCoroutine(StartBurst());
    }
    
    IEnumerator StartBurst()
    {
        transform.position += moveSpeed * transform.forward * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        yield return new WaitForSeconds(BurstTimeout);
        ResetVelocity();
        ChangeTag();
    }

    void ChangeTag()
    {
        gameObject.tag = "Asteroid";
    }

    

    public void ResetVelocity()
    {
        if (myBody)
        {
            myBody.velocity = Vector3.zero;
        }
    }

    public bool GoBurst()
    {
        return ShouldBurst = true;
    }
}
