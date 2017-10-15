using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class OptionsMenu : MonoBehaviour {

	public float MusicVol = 0.5f;
	public float SFXVol = 0.5f;
	public bool VibrationHit = true;
	public bool InvertControls = false;

	public Slider Music;
	public Slider SFX;
	public Toggle vHit;
	public Toggle iControls;
    public Sprite onImage;
    public Sprite offImage;

	// Use this for initialization
	void Start () {
		Music.value = PlayerPrefs.GetFloat("MusicVol");
		SFX.value = PlayerPrefs.GetFloat("SFXVol");
		if (PlayerPrefs.GetInt ("VibrationHit") == 1) {
			vHit.isOn = true;
		} else {
			vHit.isOn = false;
        }
		if (PlayerPrefs.GetInt ("InvertControls") == 1) {
			iControls.isOn = true;
		} else {
			iControls.isOn = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UpdateValues() {
		int VibrationHit;
		int InvertControls;

		PlayerPrefs.SetFloat ("MusicVol", Music.value);
		PlayerPrefs.SetFloat ("SFXVol", SFX.value);

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
}
