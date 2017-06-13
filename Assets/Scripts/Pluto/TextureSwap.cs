using UnityEngine;
using System.Collections;

public class TextureSwap : MonoBehaviour {

    public enum PlutoState { Idol,Damaged,Pickup,Smash,Win,Lose}
    public PlutoState curState;

    public Material[] materialArray;
    public float transitionDelay = 0.2f;


    private bool doOnce;
    private MeshRenderer meshComp;

    private Shield shieldScript;



    void Awake()
    {
        //referencing shield script
        shieldScript = GetComponent<Shield>();

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
            case PlutoState.Pickup:
                meshComp.material = materialArray[2];
                StartCoroutine(TransitionToBase());
                break;
            case PlutoState.Smash:
                meshComp.material = materialArray[3];
                StartCoroutine(TransitionToBase());
                break;
            case PlutoState.Win:
                meshComp.material = materialArray[4];
                break;
            case PlutoState.Lose:
                meshComp.material = materialArray[5];
                break;
        }
    }

    IEnumerator TransitionToBase()
    {
        yield return new WaitForSeconds(transitionDelay);
        //Switch back to idol
        meshComp.material = materialArray[0];
    }
}
