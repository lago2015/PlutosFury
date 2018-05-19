using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoonballCollision : MonoBehaviour {

    private BossChargeAnimations animScript;

	private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="MoonBall")
        {
            animScript = GetComponent<BossChargeAnimations>();
            if(animScript)
            {
                animScript.PlayBossDeadAnimation();
            }
        }
    }
}
