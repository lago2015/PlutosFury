using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class OptionsMenu : MonoBehaviour {

	
	public bool VibrationHit = true;
	public bool InvertControls = false;

    public Slider Music;
    public Slider SFX;
    public Toggle vHit;
	public Toggle iControls;
    

    public Animator Anim;
    bool mopen = false;

    // Use this for initialization
    void Awake ()
    {
        if (PlayerPrefs.HasKey("VibrationHit"))
        {
            if (PlayerPrefs.GetInt("VibrationHit") == 1)
            {
                vHit.isOn = true;
            }
            else
            {
                if(vHit)
                {
                    vHit.isOn = false;
                }
            }
        }

  
        
	}


	public void UpdateValues()
    {
		int VibrationHit;

        PlayerPrefs.SetFloat("musicParam", Music.value);
        PlayerPrefs.SetFloat("sfxParam", SFX.value);

        if (vHit.isOn) {
			VibrationHit = 1;
        } else {
			VibrationHit = 0;
        }

		
        
		PlayerPrefs.SetInt ("VibrationHit", VibrationHit);
	}

   
    

    public void WndowAnimation(bool forward)
    {
        if (forward)
        {
            if (forward == mopen)
            {
                Debug.Log("Stuck");
                return;        
            }

            Anim.SetBool("show", true);
            Debug.Log("Open");
        }
        else
        {
            Anim.SetBool("show", false);
            Debug.Log("Close");
        }
        mopen = forward;

        Debug.Log(mopen);
    }

    public bool isOpen()
    {
        return mopen;
    }
}
