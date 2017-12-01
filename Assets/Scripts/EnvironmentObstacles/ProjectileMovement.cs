using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public bool ShouldMove = true;

    void FixedUpdate()
    {
        if(ShouldMove)
        {
            transform.position += moveSpeed * transform.forward * Time.deltaTime;
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    //upon impact call this to stop movement for explosion to spawn
    public bool StopMovement()
    {
        ShouldMove = false;
        return enabled = false;
    }

}
