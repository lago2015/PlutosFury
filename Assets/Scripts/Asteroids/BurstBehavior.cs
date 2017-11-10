using UnityEngine;
using System.Collections;

public class BurstBehavior : MonoBehaviour {

    private float moveSpeed = 5;
    private float BurstTime;
    private float BurstTimeout = 2;
    private float TimeIncrement = 1;
    public bool ShouldBurst=false;
    private Rigidbody myBody;
    private bool isNewAsteroid=true;

    public bool newSpawnedAsteroid(bool isNew) { return isNewAsteroid = isNew; }
    public bool asteroidStatus() { return isNewAsteroid; }
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void LateStart()
    {
        StartCoroutine(StartBurst());
    }
    
    IEnumerator StartBurst()
    {
        transform.position += moveSpeed * transform.forward * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        yield return new WaitForSeconds(BurstTimeout);
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
