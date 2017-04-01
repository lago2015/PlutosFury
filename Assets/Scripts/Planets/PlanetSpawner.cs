using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject[] planets;
    public Queue<GameObject> myQueue = new Queue<GameObject>();

    public int waveNum;
    public int planetNum;
	void Awake ()
	{
        planets = new GameObject[transform.childCount];
        int i = 0;
        foreach(Transform childTransform in this.transform)
        {
            planets[i] = childTransform.gameObject;
            SpawnPlanets(planets[i]);
            i++;
        }
       SpawnPlanet();
   	}
	void Start()
	{
		//planetNum=-1;
        Debug.Log("Queue List" + myQueue.Peek());

	}
    void SpawnPlanets(GameObject obj)
    {
        myQueue.Enqueue(obj);
        obj.SetActive(false);
    }

    public void resetObject(GameObject obj)
    {
        //GameObject curPlanet = myQueue.Peek();
        //myQueue.Enqueue(curPlanet);   //sets gameobject to the end of list

        //Debug.Log("Queue List"+myQueue.Peek());
        //curPlanet.SetActive(false);
        obj.SetActive(false);
        myQueue.Enqueue(obj);

    }

	public void DespawnPlanet()
	{
        if (planetNum>=planets.Length)
        {
            waveNum++;
            planetNum = 0;
        }
        //GameObject curPlanet = myQueue.Peek();
        resetObject(planets[planetNum]);
        Debug.Log("Despawn");
        planetNum++;
        SpawnPlanet();
	}
    public void SpawnPlanet()
    {
		//planetNum++;
        if (myQueue.Count>=0)
        {
			//myQueue.Dequeue();      //returns the oldest (at the top of list)
            GameObject curPlanet = myQueue.Dequeue();   
            Debug.Log("Current Planet: " + curPlanet);
            curPlanet.SetActive(true);
            
            //Debug.Log("PlanetNumber: " +planetNum);
			//planets[planetNum].SetActive(true);
            
        }


    }

}
