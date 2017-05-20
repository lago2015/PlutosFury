using UnityEngine;
using System.Collections;

public class ModelSwitch : MonoBehaviour {

    public enum Models { Idol,Inflate,Damaged,PickUp,Smash,Win,Lose,Dash}
    public Models curModel;

    public float InflateDelay = 2f;
    public float transitionDelay = 0.2f;
    public float statusChangeDelay = 1.2f;
    public bool isDashActive;
    private float idolDelay = 0;
    private float curDelay;
    private int modelIndex;
    public GameObject[] PlutoModels;
    private bool isPoweredUp;
    private bool doOnce;
    public int tempIndex;
    public bool gameOver;

    Coroutine cor;
    void Start()
    {
        //Start as idol
        RunModels(0);
    }
    

    //start model change 
    public Models ChangeModel(Models NewModel)
    {
        //update model to switch to 
        curModel = NewModel;
        //update model
        DetermineModel();
        return curModel;
    }

    void DetermineModel()
    {
        //controls which model to switch to and when
        switch(curModel)
        {
            case Models.Idol:
                curDelay = idolDelay;
                if(gameOver)
                {
                    ChangeModel(Models.Lose);
                }
                RunModels(0);
                break;
            case Models.Inflate:
                curDelay = InflateDelay;
                isPoweredUp = true;
                RunModels(1);
                break;
            case Models.Damaged:
                curDelay = statusChangeDelay;
                RunModels(3);
                break;
            case Models.PickUp:
                curDelay = statusChangeDelay;
                RunModels(4);
                break;
            case Models.Smash:
                curDelay = statusChangeDelay;
                RunModels(5);
                break;
            case Models.Win:
                RunModels(6);
                break;
            case Models.Lose:
                curDelay = 1000;
                gameOver = true;
                RunModels(2);
                break;
            case Models.Dash:
                ActivateDash();
                break;
        }
    }

    void RunModels(int Index)
    {
        if(cor!=null)
        {
            if (isPoweredUp == true && gameOver == true)
            {
                StopCoroutine(cor);
            }
        }
        if (!doOnce)
        {
            doOnce = true;
            //iterate through to turn off models and turn on the current model
            for (int i = 0; i <= PlutoModels.Length - 1; ++i)
            {
                if (i == Index)
                {
                    PlutoModels[i].SetActive(true);
                    modelIndex = Index;

                }
                else
                {
                    PlutoModels[i].SetActive(false);
                }
            }
            //ensure dash glow is active with model if active
            if (isDashActive)
            {
                PlutoModels[7].SetActive(true);
            }
           //Start transition
            cor=StartCoroutine(TransitionModel());
        }
    }

    //Dash needs its own function because its a background glow compared to model switch
    void ActivateDash()
    {
        //turn on dash glow
        PlutoModels[7].SetActive(true);
        isDashActive = true;
        if(!gameOver)
        {
            curModel = Models.Idol;
        }
        else
        {
            ChangeModel(Models.Lose);
        }
    }

    IEnumerator TransitionModel()
    {
        yield return new WaitForSeconds(curDelay);
        doOnce = false;
        //Switch back to idol
        PlutoModels[modelIndex].SetActive(false);
        modelIndex = 0;
        PlutoModels[modelIndex].SetActive(true);

        //check for game over
        if (!gameOver)
        {
            curModel = Models.Idol;
        }
        else
        {
            ChangeModel(Models.Lose);
        }
    }

}
