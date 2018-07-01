using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerContact : MonoBehaviour
{
    private void OnCollisionEnter(Collision c)
    {
        Debug.Log("HITa!");
        if (c.gameObject.tag == "MoonBall")
        {
            Debug.Log("HITb!");


            if (true)
            {
                Vector3 dir = c.contacts[0].point - transform.position;

                dir = dir.normalized;

                MoonBall ball = c.gameObject.GetComponent<MoonBall>();

                ball.MoveBall(dir, ball.hitSpeed);

                Debug.Log("HITc!");
            }
        }
    }
}
