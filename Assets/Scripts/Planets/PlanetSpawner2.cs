using UnityEngine;
using System.Collections;

public class PlanetSpawner2 : MonoBehaviour 
{
	public GameObject[] planets;
	private Movement playerMoveScript;
	private int i;

	public float TimeBeforeFirstSpawn;
	public float TimeBetweenSpawnCheck;
//	private GameManager gameManager;

	void Start ()
	{
//		gameManager = GetComponent<GameManager> ();
//		if (gameManager = null) 
//			Debug.Log ("You Done Fucked Up Now!");
		i = 0;
		//InvokeRepeating ("SpawnPlanet", TimeBeforeFirstSpawn, TimeBetweenSpawns);
		StartCoroutine ("CheckForPlanetInWorld");
	}

	IEnumerator CheckForPlanetInWorld()
	{
		if (i == 0) 
		{
			yield return new WaitForSeconds (TimeBeforeFirstSpawn);
		} 
		else 
		{
			yield return new WaitForSeconds (TimeBetweenSpawnCheck);
		}
		SpawnPlanet();
		StartCoroutine ("CheckForPlanetInWorld");
	}

	void SpawnPlanet ()
	{
		GameObject TempPlanet = GameObject.FindGameObjectWithTag ("Planets");
		if (!TempPlanet) 
		{
			if ( i < planets.Length)
			{
				float xRange = Random.Range (-90, 90);
				float yRange = Random.Range (- 90, 90);
				Instantiate (planets [i], new Vector3 (xRange, yRange, 0.0f), Quaternion.identity);
				i++;
			}
			else
			{
				Debug.Log ("Last Planet Has Been Spawned!");
				//gameManager.YouWin();
			}
		}
	}
}
