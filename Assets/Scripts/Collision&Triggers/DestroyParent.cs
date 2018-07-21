using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour {

	void OnDestroy()
    {
        if(gameObject.transform.parent)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
