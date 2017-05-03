using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public Texture2D fadeOutTexture;	// the texture that will overlay the screen. This can be a black image or a loading graphic
    public float fadeSpeed = 0.8f;		// the fading speed

    private int drawDepth = -1000;		// the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;			// the texture's alpha value between 0 and 1
    public int fadeDir=-1;
    bool isOpen;
    public float fadeTime=2;
    private int keyObtained;
    private int numKeyRequired=3;
    private GameManager gameScript;
    private AudioController audioScript;
    void Awake()
    {
        gameScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<GameManager>();
        audioScript = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        isOpen = false;
    }

    public void OpenDoor(Vector3 curPosition)
    {
    
        if(keyObtained==numKeyRequired)
        {
            isOpen = true;
            audioScript.WormholeOpen(curPosition);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player")
        {
            if(isOpen)
            {
                //do something winning here
                fadeDir = 1;
                StartCoroutine(ChangeScene());
            }
        }
    }
		// the direction to fade: in = -1 or out = 1
    IEnumerator ChangeScene()
    {
        Vector3 moveToBoss = GameObject.FindGameObjectWithTag("Door2").transform.position;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera"); ;
        //gameScript.YouWin();
        yield return new WaitForSeconds(fadeTime);
        if (player && moveToBoss != Vector3.zero&&camera)
        {
            player.SetActive(false);
            moveToBoss.z = camera.transform.position.z;
            camera.transform.position = moveToBoss;
            moveToBoss.z = 0;
            player.transform.position = moveToBoss;
            
            player.SetActive(true);
        }

        fadeDir = -1;
       
    }

    public void KeyAcquired(Vector3 curPosition)
    {
        keyObtained++;
        if(curPosition!=Vector3.zero)
        {
            audioScript.MoonAcquiredSound(curPosition);
        }
        OpenDoor(curPosition);
    }

    void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;																// make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);		// draw the texture to fit the entire screen area
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
    void OnLevelWasLoaded()
    {
        // alpha = 1;		// use this if the alpha is not set to 1 by default
        BeginFade(-1);		// call the fade in function
    }
}
