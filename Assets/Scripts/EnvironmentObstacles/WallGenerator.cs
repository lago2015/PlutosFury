using UnityEngine;
using System.Collections;

public class WallGenerator : MonoBehaviour {

    public GameObject ManagerModel;
    private WallGenManager managerScript;


    void Awake()
    {
        if(ManagerModel)
        {
            managerScript = ManagerModel.GetComponent<WallGenManager>();
        }
    }

	void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            bool isDashing = col.gameObject.GetComponent<Movement>().DashStatus();
            if(isDashing)
            {
                managerScript.WallDestroyed();
            }
        }
    }
}
