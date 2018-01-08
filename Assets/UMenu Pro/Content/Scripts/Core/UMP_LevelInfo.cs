using UnityEngine;
using UnityEngine.UI;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class UMP_LevelInfo : MonoBehaviour {

    public Text Title;
    //public Text Description;
    public Text PlayText;
    public Image Preview;
    //Name of scene of build setting
    private int LevelNum;

    /// <summary>
    /// Level Info
    /// </summary>
    /// <param name="title"></param>
    /// <param name="desc"></param>
    /// <param name="preview"></param>
    /// <param name="scene"></param>
    public void GetInfo(string title,Sprite preview,int sceneNum,string pn)
    {
        Title.text = title;
       // Description.text = desc;
        Preview.sprite = preview;
       // PlayText.text = pn;
        LevelNum = sceneNum;
    }
    /// <summary>
    /// 
    /// </summary>
   // public void OpenLevel() { LoadLevel(LevelName); }

    public void LoadTargetLevel()
    {
        FindObjectOfType<LoadTargetSceneButton>().LoadSceneNum(LevelNum);
    }


    public static void LoadLevel(string scene)
    {
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
 SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Forward"></param>
    public void EventMouse(bool Forward = true)
    {
        Animator a = this.GetComponent<Animator>();
        if (Forward)
        {
            a.SetBool("show", true);
        }
        else
        {
            a.SetBool("show", false);
        }
    }
}