using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePool : MonoBehaviour {

    public static GamePool instance;        //Singleton Instance

    [Header("Audio Pool")]
    public Transform audioSourcePrefab;     //Prefab containing audio source
    public AudioClip[] audioPoolItems;      //Audio Pool Prefabs
    public int[] audioPoolLength;           // Audio pool items count

    // Pooled items collections
    private Dictionary<AudioClip, AudioSource[]> audioPool;




    // Use this for initialization
    void Start ()
    {
        // Singleton instance
        instance = this;
        
        // Initialize audio pool
        if (audioPoolItems.Length > 0)
        {
            audioPool = new Dictionary<AudioClip, AudioSource[]>();

            for (int i = 0; i < audioPoolItems.Length; i++)
            {
                AudioSource[] audioArray = new AudioSource[audioPoolLength[i]];

                for (int x = 0; x < audioPoolLength[i]; x++)
                {
                    AudioSource newAudio = ((Transform)Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity)).GetComponent<AudioSource>();
                    newAudio.clip = audioPoolItems[i];

                    newAudio.gameObject.SetActive(false);
                    newAudio.transform.parent = transform;

                    audioArray[x] = newAudio;
                }

                audioPool.Add(audioPoolItems[i], audioArray);
            }
        }
    }

    // Spawn audio prefab and send OnSpawned message
    public AudioSource SpawnAudio(AudioClip clip, Vector3 pos, Transform parent)
    {
        for (int i = 0; i < audioPool[clip].Length; i++)
        {
            if (!audioPool[clip][i].gameObject.activeSelf)
            {
                AudioSource spawnItem = audioPool[clip][i];

                spawnItem.transform.parent = parent;
                spawnItem.transform.position = pos;

                spawnItem.gameObject.SetActive(true);

                return spawnItem;
            }
        }

        return null;
    }
    // Despawn effect or audio and send OnDespawned message
    public void Despawn(Transform obj)
    {
        obj.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(false);
    }

}
