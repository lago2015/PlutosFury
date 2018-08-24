using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
	/*****Main Menu, Sub-Menus, Buttons and Texts*****/

	public Canvas HTPMenu; //the how to play screen
	public Canvas CreditsMenu; //the credits menu

	public Button StartText; //the start button
	public Button HTPText; //the how to play button
	public Button CreditsText; //the credits text button

    //Audio Variables
    public AudioClip[] sounds;
    [HideInInspector]
    private new AudioSource audio;

	void Start ()
	{
		HTPMenu = HTPMenu.GetComponent<Canvas>();
		HTPMenu.enabled = false;
		StartText = StartText.GetComponent<Button>();

		CreditsMenu = CreditsMenu.GetComponent<Canvas>();
		CreditsMenu.enabled = false;
		CreditsText = CreditsText.GetComponent<Button>();

		HTPText = HTPText.GetComponent<Button>();

        audio = GetComponent<AudioSource>();
        audio.Stop();
	}

	public void HTPPress()
	{
		HTPMenu.enabled = true;
		HTPText.enabled = true;

		CreditsMenu.enabled = false;
		CreditsText.enabled = false;

		StartText.enabled = false;

        audio.clip = sounds[0];
        audio.Play();
	}

	public void CreditsPress()
	{
        audio.clip = sounds[0];
        audio.Play();
		HTPMenu.enabled = false;
		HTPText.enabled = false;

		CreditsMenu.enabled = true;
		CreditsText.enabled = true;

		StartText.enabled = false;
	}

	public void OnBackPress()
	{
        audio.clip = sounds[1];
        audio.Play();
		HTPMenu.enabled = false;
		HTPText.enabled = true;

		CreditsMenu.enabled = false;
		CreditsText.enabled = true;

		StartText.enabled = true;
	}

	public void OnStart()
	{
        //audio.clip = sounds[0];
        //audio.Play();
        SceneManager.LoadScene(1);
		
	}

}