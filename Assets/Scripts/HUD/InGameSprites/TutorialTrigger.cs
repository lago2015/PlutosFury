using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            GameObject.FindObjectOfType<TutorialTip>().TipDown();

            Destroy(this.gameObject);
        }
    }
}
