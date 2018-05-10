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
    void Start ()
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

        if (PlayerPrefs.HasKey("InvertControls"))
        {
            if (PlayerPrefs.GetInt("InvertControls") == 1)
            {
                iControls.isOn = true;
            }
            else
            {
                if(iControls)
                {
                    iControls.isOn = false;
                }
            }
        }
        
	}


	public void UpdateValues()
    {
		int VibrationHit;
		int InvertControls;

        PlayerPrefs.SetFloat("musicParam", Music.value);
        PlayerPrefs.SetFloat("sfxParam", SFX.value);

        if (vHit.isOn) {
			VibrationHit = 1;
        } else {
			VibrationHit = 0;
        }

		if (iControls.isOn) {
			InvertControls = 1;
		} else {
			InvertControls = 0;
		}
        
		PlayerPrefs.SetInt ("VibrationHit", VibrationHit);
		PlayerPrefs.SetInt ("InvertControls", InvertControls);
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
