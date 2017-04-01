using UnityEngine;
using System.Collections;

public class FollowPlanet : MonoBehaviour {

    //*********Just meant to follow another planet without making the gameobject a child.

    public GameObject PlanetToFollow;

    void FixedUpdate()
    {
        if(PlanetToFollow)
        {
            transform.position = PlanetToFollow.transform.position;

        }
    }
}
