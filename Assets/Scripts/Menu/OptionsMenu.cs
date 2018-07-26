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
        if (PlayerPrefs.HasKey("godMode"))
        {
            if (PlayerPrefs.GetInt("godMode") == 1)
            {
                vHit.isOn = true;
            }
            else
            {
                if (vHit)
                {
                    vHit.isOn = false;
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

        if (iControls.isOn)
        {
            InvertControls = 1;
        }
        else
        {
            InvertControls = 0;
        }

        PlayerPrefs.SetInt ("VibrationHit", VibrationHit);
        PlayerPrefs.SetInt("godMode", InvertControls);
    }

   
    

    public void WndowAnimation(bool forward)
    {
        if (forward)
        {
            if (forward == mopen)
            {

                return;        
            }

            Anim.SetBool("show", true);

        }
        else
        {
            Anim.SetBool("show", false);

        }
        mopen = forward;

    }

    public bool isOpen()
    {
        return mopen;
    }
}
