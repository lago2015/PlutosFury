using UnityEngine;
using System.Collections;

public class TextureSwap : MonoBehaviour {

    public enum PlutoState { Idol,Damaged,Pickup,Smash,Win,Lose}
    public PlutoState curState;

    public Material[] materialArray;
    private float defaultDelay;
    public float transitionDelay = 0.2f;
    public float disableRenderTimer = 0.7f;

    private bool doOnce;
    private MeshRenderer meshComp;
    private Material curMaterial;
    private Shield shieldScript;
    


    void Awake()
    {
        //referencing shield script
        shieldScript = GetComponent<Shield>();
        defaultDelay = transitionDelay;
        //referencing the mesh renderer 
        Transform baseObject = transform.GetChild(0);
        meshComp = baseObject.GetChild(0).GetComponent<MeshRenderer>();

        //initialize material
        SwapMaterial(PlutoState.Idol);
    }

    //public function for switching materials depending on parameter
    public void SwapMaterial(PlutoState nextState)
    {
        curState = nextState;
        switch(curState)
        {
            case PlutoState.Idol:
                meshComp.material = materialArray[0];        
                break;
            case PlutoState.Damaged:
                //Check if shielded
                if(shieldScript.PlutoShieldStatus()==false)
                {
                    //apply damage material
                    meshComp.material = materialArray[1];
                    StartCoroutine(TransitionToBase());
                }
                break;
            case PlutoState.Smash:
                meshComp.material = materialArray[3];
                transitionDelay = 0.75f;
                StartCoroutine(TransitionToBase());
                break;
            case PlutoState.Win:
                transitionDelay = 5f;
                meshComp.material = materialArray[2];
                break;
        }
    }
    public void StartRender()
    {
        StopCoroutine(RenderTimeout());
        meshComp.enabled = true;
    }
    public void DisableRender()
    {
        StartCoroutine(RenderTimeout());
    }

    IEnumerator RenderTimeout()
    {
        meshComp.enabled = false;
        yield return new WaitForSeconds(disableRenderTimer);
        meshComp.enabled = true;
    }

    IEnumerator TransitionToBase()
    {
        yield return new WaitForSeconds(transitionDelay);
        transitionDelay = defaultDelay;
        //Switch back to idol
        meshComp.material = materialArray[0];
    }
}
