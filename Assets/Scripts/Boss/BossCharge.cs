using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharge : MonoBehaviour
{

    public Transform bossTransform;
    public Animation bossAnim;
    public float upMark;
    public float downMark;
    public float moveSpeed;
    public float[] chargeMarks;

    float targetMark;

    bool isPatrolling = true;
    bool moveToLock = false;
    bool goingUp = true;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(LockTargert());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isPatrolling)
        {
            if (goingUp)
            {
                bossTransform.transform.Translate(Vector3.up * moveSpeed);

                if (bossTransform.position.y >= upMark)
                {
                    goingUp = false;
                }
            }
            else
            {
                bossTransform.transform.Translate(Vector3.down * moveSpeed);

                if (bossTransform.position.y <= downMark)
                {
                    goingUp = true;
                }
            }
        }
        

        if(moveToLock)
        {
           if(goingUp)
            {
                bossTransform.transform.Translate(Vector3.up * moveSpeed);

                if (bossTransform.position.y >= targetMark)
                {
                    // Charge
                    moveToLock = false;
                    StartCoroutine(chargeWait());
                    //bossAnim.Play();
                }
            }
            else
            {
                bossTransform.transform.Translate(Vector3.down * moveSpeed);

                if (bossTransform.position.y <= targetMark)
                {
                    // Charge
                    moveToLock = false;
                    StartCoroutine(chargeWait());
                    //bossAnim.Play(bossAnim.clip.name);
                }
            }
        }
	}

    IEnumerator LockTargert()
    {
        yield return new WaitForSeconds(3.0f);
        isPatrolling = false;
        moveToLock = true;
        targetMark = chargeMarks[Random.Range(0, chargeMarks.Length)];
        if(bossTransform.position.y < targetMark)
        {
            goingUp = true;
        }
        else
        {
            goingUp = false;
        }
    }

    IEnumerator chargeWait()
    {
        yield return new WaitForSeconds(3.0f);
        isPatrolling = true;
        StartCoroutine(LockTargert());
    }
}
