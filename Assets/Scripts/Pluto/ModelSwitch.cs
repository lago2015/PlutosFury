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
    void Start()
    {
        RunModels(0);
    }
    


    public Models ChangeModel(Models NewModel)
    {
        curModel = NewModel;
        DetermineModel();
        return curModel;
    }

    void DetermineModel()
    {

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
        if(!doOnce)
        {
            tempIndex = Index;
            doOnce = true;
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
            if (isDashActive)
            {
                PlutoModels[7].SetActive(true);
            }
            StartCoroutine(TransitionModel());
        }
    }

    void ActivateDash()
    {
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
        PlutoModels[modelIndex].SetActive(false);
        modelIndex = 0;
        PlutoModels[modelIndex].SetActive(true);

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
