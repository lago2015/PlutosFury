using UnityEngine;
using System.Collections;

public class FlyingRock : MonoBehaviour {

    public GameObject regularState;
    public GameObject explosionState;
    private Rigidbody myBody;
    private ProjectileMovement moveScript;
    void Awake()
    {
        moveScript = GetComponent<ProjectileMovement>();
        myBody = GetComponent<Rigidbody>();
        if (regularState)
        {
            regularState.SetActive(true);
        }
        if (explosionState)
        {
            explosionState.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (regularState && explosionState)
        {
            regularState.SetActive(false);
            explosionState.SetActive(true);
            myBody.velocity = Vector3.zero;
            moveScript.ShouldMove = false;
        }
    }
}
