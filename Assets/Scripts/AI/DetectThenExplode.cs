using UnityEngine;
using System.Collections;

public class DetectThenExplode : MonoBehaviour {

    
    public GameObject regularState;
    public GameObject explosionState;
    private bool doOnce;

    void Awake()
    {
        if(regularState)
        {
            regularState.SetActive(true);
        }
        if(explosionState)
        {
            explosionState.SetActive(false);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(!col.isTrigger)
            {
                if (regularState && explosionState)
                {
                    if(!doOnce)
                    {
                        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().DestructionSmall(transform.position);
                        doOnce = true;
                    }
                    StartCoroutine(SwitchModels());
                }
            }
        }
    }

    IEnumerator SwitchModels()
    {
        regularState.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        explosionState.SetActive(true);
    }
}
