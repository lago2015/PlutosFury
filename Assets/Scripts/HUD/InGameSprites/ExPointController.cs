using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExPointController : MonoBehaviour {

    //reference animatied gameobject
    public GameObject exPointObject;
    private GameObject exPointChild;
    private GameObject hud;
    private void Awake()
    {
        if (exPointObject)
        {
            exPointObject = Instantiate(exPointObject, transform.position, Quaternion.identity);
            exPointChild = exPointObject.transform.GetChild(0).gameObject;
            hud = GameObject.FindGameObjectWithTag("HUDManager").gameObject;
            exPointObject.transform.SetParent(GameObject.Find("HUD").transform, false);
            exPointObject.transform.localPosition = hud.transform.localPosition;
            exPointObject.SetActive(false);
        }
    }
    public void CreateFloatingExPoint()
    {
        exPointObject.transform.localPosition = hud.transform.localPosition;
        exPointObject.SetActive(true);
        if (exPointChild.activeInHierarchy)
        {
            exPointChild.SetActive(false);
            exPointChild.SetActive(true);
        }
        else
        {
            exPointChild.SetActive(true);
        }

    }
}
