using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class OptionsMenu : MonoBehaviour {

    private int isJoystickFixed;
    
    public Slider Music;
    public Slider SFX;
    public Toggle vHit;
	public Toggle iControls;
    public Toggle floatingJoystick;
    

    public Animator Anim;
    bool mopen = false;

    // Use this for initialization
    void Awake ()
    {
        if (PlayerPrefs.HasKey("VibrationHit")&&vHit)
        {
            if (PlayerPrefs.GetInt("VibrationHit") == 1)
            {
                vHit.isOn = true;
            }
            else
            {
                vHit.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("godMode")&&iControls)
        {
            if (PlayerPrefs.GetInt("godMode") == 1)
            {
                iControls.isOn = true;
            }
            else
            {
                iControls.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("joystickPref") && floatingJoystick)
        {
            if (PlayerPrefs.GetInt("joystickPref") == 1)
            {
                floatingJoystick.isOn = true;
                isJoystickFixed = 1;
            }
            else
            {
                floatingJoystick.isOn = true;
                isJoystickFixed = 0;
                
            }
        }

    }


	public void UpdateValues()
    {
		int VibrationHit;
        int InvertControls;
        PlayerPrefs.SetFloat("musicParam", Music.value);
        PlayerPrefs.SetFloat("sfxParam", SFX.value);
        PlayerPrefs.SetInt("joystickPref", isJoystickFixed);
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
    
    public void UpdateJoystick()
    {
        if(isJoystickFixed==1)
        {
            isJoystickFixed = 0;
            floatingJoystick.isOn = false;    
        }
        else
        {
            isJoystickFixed = 1;
            floatingJoystick.isOn = true;
        }
        
        PlayerPrefs.SetInt("joystickPref", isJoystickFixed);
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
