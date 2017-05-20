using UnityEngine;
using System.Collections;

public class Inflation : MonoBehaviour {

    public float InflateTimeout;
    public float InflatedSize;
    private float NormalSize;
    private SphereCollider myCollider;
    

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
            NormalSize = myCollider.radius;
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
                myCollider.radius = InflatedSize;
                currInflated = true;
                StartCoroutine(InflateDuration());
            }
            
        }
        
    }
    public bool Inflate() {  return isInflated=true; }
    IEnumerator InflateDuration()
    {
        yield return new WaitForSeconds(InflateTimeout);
        isInflated = false;
        currInflated = false;
        myCollider.radius = NormalSize;
    }
}
