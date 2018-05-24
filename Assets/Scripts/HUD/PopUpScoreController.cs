using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScoreController : MonoBehaviour
{

    // Reference to score pool
    public ObjectPool scorePool;

    //reference animatied gameobject
    public PopUpScore popUpTextComp;
    private static GameObject canvasComp;
    //load the asset from folder
    void Awake()
    {
        canvasComp = GameObject.FindGameObjectWithTag("HUD");
    }
	
    public void CreateFloatingText(string text,Vector3 Location)
    {
        //Create gameobject text within world
        //PopUpScore instance = Instantiate(popUpTextComp);
        PopUpScore instance = scorePool.GetObject().GetComponent<PopUpScore>();

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(Location);
            
        instance.transform.SetParent(canvasComp.transform, false);
        instance.transform.position = screenPosition;
        //Set text to gameobject animation
        instance.SetText(text);
        instance.gameObject.SetActive(true);

        StartCoroutine(LifeTime(1.5f, instance.gameObject));

    }

    private IEnumerator LifeTime(float seconds, GameObject instance)
    {

        yield return new WaitForSeconds(seconds);
        scorePool.PlaceObject(instance);
        instance.SetActive(false);
    }

}
