/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer mixer;
    public AudioSetting[] audioSettings;
    private enum AudioGroups {Music, SFX};

    void Awake()
    {
        instance = GetComponent<AudioManager>();
    }

	void Start()
    {
        for (int i = 0; i < audioSettings.Length; i++)
        {
            audioSettings[i].Initialize();

        }

    }

    
    public void SetMusicVolume(float value)
    {
        audioSettings[(int)AudioGroups.Music].SetExposedParam(value);
    }

    public void SetSFXVolume(float value)
    {
        audioSettings[(int)AudioGroups.SFX].SetExposedParam(value);
    }
}

[System.Serializable]
public class AudioSetting
{
    public Slider slider;
    public string exposedParam;

    public void Initialize()
    {
        if(slider)
        {
            slider.value = PlayerPrefs.GetFloat(exposedParam);
            
        }
    }
    public void SetExposedParam(float value) // 1
    {
        AudioManager.instance.mixer.SetFloat(exposedParam, value); // 3
        
        PlayerPrefs.SetFloat(exposedParam, value); // 4
    }
}
