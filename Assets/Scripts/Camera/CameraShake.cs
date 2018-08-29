using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{


    float shake = 0.0f;
    public float ShakeAmount;
    public float DecreaseFactor;
    Vector3 StartPosition;
    Vector3 ShakeVector;
    float PosZ;

    void Start()
    {
        PosZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake > 0.0f)
        {
            StartPosition = transform.localPosition;
            ShakeVector = StartPosition + Random.insideUnitSphere * ShakeAmount;
            ShakeVector.z = PosZ;
            transform.localPosition = ShakeVector;
            shake -= Time.deltaTime * DecreaseFactor;
            //Handheld.Vibrate();
        }
        else
        {
            shake = 0.0f;
        }
    }

    private IEnumerator Hit()
    {
        Time.timeScale = 0.7f;
        float EndShake = Time.realtimeSinceStartup + 0.3f;

        while (Time.realtimeSinceStartup < EndShake)
        {
            yield return 0;
        }
        Time.timeScale = 1.0f;
    }

    public void EnableCameraShake()
    {
        shake = 0.3f;
        StartCoroutine("Hit");
    }

}