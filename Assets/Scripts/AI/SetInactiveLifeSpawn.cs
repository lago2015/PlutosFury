using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactiveLifeSpawn : MonoBehaviour {


    public float LifeDuration;
    
    public GameObject Explosion;
    public GameObject parent;
    void OnEnable()
    {
        StartCoroutine(CountdownToLife());
    }

    IEnumerator CountdownToLife()
    {
        yield return new WaitForSeconds(LifeDuration);
        if (Explosion)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        if (parent != null)
        {
            parent.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
