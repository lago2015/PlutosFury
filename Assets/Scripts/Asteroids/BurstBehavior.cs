using UnityEngine;
using System.Collections;

public class BurstBehavior : MonoBehaviour {

    private float moveSpeed = 5;
    private float BurstTime;
    private float BurstTimeout = 2;
    private float TimeIncrement = 1;
    public bool ShouldBurst=false;
    
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

	// Update is called once per frame
	void FixedUpdate ()
    {
        if(ShouldBurst)
        {
            if(BurstTime<=BurstTimeout)
            {
                BurstTime += Time.deltaTime * TimeIncrement;
                
            }
            else
            {
                ShouldBurst = false;
            }
            
        }
        
    }
    public bool GoBurst()
    {
        return ShouldBurst = true;
    }
}
