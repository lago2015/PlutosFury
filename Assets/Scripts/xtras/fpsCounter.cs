using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fpsCounter : MonoBehaviour {
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    public Text fpsText;
    void Update()
    {
        
        if(fpsText && fpsText.name == "fpsText")
        {
            fpsText.text="FPS: " + 1/Time.deltaTime;
        }
    }
}
