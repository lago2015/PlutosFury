using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAbility : MonoBehaviour {


    private Rigidbody myBody;
    //******Shockwave Variables
    //Shockwave radius
    private float shockwaveRadius = 20f;
    private float power = 50f;
    //if player has pick up
    private bool ShockChargeActive;
    private PlayerAppearance appearanceScript;
    public bool ShockChargeStatus() { return ShockChargeActive; }


    // Use this for initialization
    void Awake ()
    {
        appearanceScript = GetComponent<PlayerAppearance>();
        myBody = GetComponent<Rigidbody>();
	}
	
	


    public bool ActivateShockCharge()
    {
        return ShockChargeActive = true;
    }

    public void Shockwave()
    {
        appearanceScript.BusterChange(PlayerAppearance.BusterStates.Shockwave);

        Vector3 curPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(curPosition, shockwaveRadius);

        foreach (Collider col in colliders)
        {
            //Get collied gameobject's rigidbody
            Rigidbody hitBody = col.GetComponent<Rigidbody>();
            if (hitBody != null && hitBody != myBody)
            {
                //getting distance from current point and collided object
                Vector3 points = hitBody.position - transform.position;
                //Get distance from the two points
                float distance = points.magnitude;

                Vector3 direction = points / distance;
                hitBody.AddForce(direction * power);
            }

            DetectThenExplode explodeScript = col.GetComponent<DetectThenExplode>();
            if (explodeScript)
            {
                explodeScript.TriggeredExplosion();

            }
            AIHealth enemyScript = col.GetComponent<AIHealth>();
            if (enemyScript)
            {
                enemyScript.IncrementDamage("Player");

            }
            BigAsteroid asteroidScript = col.GetComponent<BigAsteroid>();
            if (asteroidScript)
            {
                asteroidScript.SpawnAsteroids();
            }
            if (col.gameObject.tag == "LazerWall")
            {
                WallGenManager wallScript = col.transform.parent.transform.parent.GetComponent<WallGenManager>();
                if (wallScript)
                {
                    wallScript.WallDestroyed();


                }
            }
            ShockChargeActive = false;
        }
    }
}
