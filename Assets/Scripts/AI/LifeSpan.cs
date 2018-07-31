using UnityEngine;
using System.Collections;

public class LifeSpan : MonoBehaviour {

    public float LifeDuration;
    public string poolName;

    void Start()
    {
        if (poolName.Length == 0)
        {
            StartCoroutine(CountdownToLife());
        }
    }

    void OnEnable()
    {
        if (poolName.Length > 0)
        {
            StartCoroutine(CountdownToLife());
        }
    }

    IEnumerator CountdownToLife()
    {
        yield return new WaitForSeconds(LifeDuration);
        if (poolName.Length > 0)
        {
            GameObject.FindObjectOfType<ObjectPoolManager>().PutBackObject(poolName, gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

 
}
