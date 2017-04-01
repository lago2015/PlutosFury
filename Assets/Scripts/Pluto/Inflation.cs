using UnityEngine;
using System.Collections;

public class Inflation : MonoBehaviour {

    public float InflateTimeout;
    public Vector3 InflatedSize;
    private Vector3 NormalSize;
    private SphereCollider myCollider;
    private float NormalCol;
    private float InflateCol=3;

    private AudioController audioCon;
    private ModelSwitch changeModel;
    private bool isInflated;
    private bool currInflated;
    void Awake()
    {
        audioCon = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        changeModel = GetComponent<ModelSwitch>();
        myCollider = GetComponent<SphereCollider>();
        if(myCollider)
        {
            NormalCol = myCollider.radius;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Inflate();
        }
        if(isInflated)
        {
            InflatePluto();
        }
    }

	public void InflatePluto()
    {
        
        if(changeModel)
        {
            if(!currInflated)
            {
                if (audioCon)
                {
                    audioCon.InflateActiv(transform.position);
                }       
                changeModel.ChangeModel(ModelSwitch.Models.Inflate);
                myCollider.radius = InflateCol;
                currInflated = true;
                StartCoroutine(InflateDuration());
            }
            
        }
        
    }
    public bool Inflate() { isInflated = true; return isInflated; }
    IEnumerator InflateDuration()
    {
        yield return new WaitForSeconds(InflateTimeout);
        isInflated = false;
        currInflated = false;
        myCollider.radius = NormalCol;
    }
}
