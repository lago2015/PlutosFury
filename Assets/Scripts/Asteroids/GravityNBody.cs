using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityNBody : MonoBehaviour 
{
	public double G = 6.67e-11;
	public GameObject[] asteroids;
	public int ASTEROID_SIZE = 5;
	List<GameObject> asteroidPool;
	
	void Awake()
	{
		// Populate the list with various asteroids
		asteroidPool = new List<GameObject>();
		for (int i = 0; i < ASTEROID_SIZE; i++) 
		{
			int j = Random.Range(0, asteroids.Length);
			GameObject asteroid = (GameObject)Instantiate (asteroids[j]);
			asteroid.SetActive (false);
			asteroidPool.Add (asteroid);
		}
		for (int i = 0; i < ASTEROID_SIZE; i++) 
		{
			SpawnAsteroid();
		}
	}

	void FixedUpdate()
	{
		// We have two nested loops.
		for (int indexOfA = 0; indexOfA < asteroidPool.Count; ++indexOfA) {
			for (int indexOfB = indexOfA + 1; indexOfB < asteroidPool.Count; ++indexOfB) {
				GameObject bodyA = asteroidPool [indexOfA];
				GameObject bodyB = asteroidPool [indexOfB];
				
				Vector3 forceOnA = ForceOfGravity (bodyA.GetComponent<Rigidbody> ().mass, bodyA.transform.position, 
				                                  bodyB.GetComponent<Rigidbody> ().mass, bodyB.transform.position);
				
				Vector3 forceOnB = -forceOnA;	// Newton's Third Law!!!!!!!!

				if (!float.IsNaN(forceOnA.x) ||
				    !float.IsNaN(forceOnA.y) ||
				    !float.IsNaN(forceOnA.z) ||
				    !float.IsNaN(forceOnB.x) ||
				    !float.IsNaN(forceOnB.y) ||
				    !float.IsNaN(forceOnB.z) )
				{
					bodyA.GetComponent<Rigidbody> ().AddForce (forceOnA);
					bodyB.GetComponent<Rigidbody> ().AddForce (forceOnB);
				}
			}
		}
	}
	
	GameObject GetPooledAsteroid()
	{
		// Grab asteroid from pool
		for (int i = 0; i < asteroidPool.Count; i++) 
		{
			if (!asteroidPool[i].activeInHierarchy)
			{
				return asteroidPool [i];
			}
		}

		int j = Random.Range(0, asteroids.Length);
		GameObject asteroid = (GameObject) Instantiate(asteroids[j]);
		asteroid.SetActive (false);
		asteroidPool.Add (asteroid);
		return asteroid;
	}
	
	public void SpawnAsteroid()
	{
		// Grab the pooled asteroid
		GameObject asteroid = GetPooledAsteroid ();
		if (asteroid == null)
			return;
		
		// Place asteroid in a random location
		// Play area is from (-100, -100) to (100, 100)
		float x = Random.Range (-95, 95);
		float y = Random.Range (-95, 95);
		float roll = Random.Range (0, 359);
		asteroid.transform.position = new Vector3 (x, y, 0.0f);
		asteroid.transform.rotation = Quaternion.Euler (0.0f, 0.0f, roll);
		asteroid.SetActive (true);
	}
	
	public void ReturnPooledAsteroid(GameObject asteroid)
	{
		// Return asteroid to the list
		asteroid.SetActive (false);
		asteroidPool.Add (asteroid);
	}

	public Vector3 ForceOfGravity(float mass1, Vector3 position1, float mass2, Vector3 position2)
	{
		Vector3 directionFromBody1ToBody2 = (position2 - position1).normalized;
		
		double distance = Vector3.Distance(position1, position2);
		
		double strength = G * mass1 * mass2 / (distance * distance);
		
		return (float)strength * directionFromBody1ToBody2;
	}
}
