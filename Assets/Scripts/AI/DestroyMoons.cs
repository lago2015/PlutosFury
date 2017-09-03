using UnityEngine;
using System.Collections;

public class DestroyMoons : MonoBehaviour {

    public GameObject[] PlanetMoons;

    public void DestroyAllMoons()
    {
        foreach(GameObject Moon in PlanetMoons)
        {
            Destroy(Moon);
        }
    }
}
