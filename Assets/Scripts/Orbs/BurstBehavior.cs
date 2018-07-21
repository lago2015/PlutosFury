using UnityEngine;
using System.Collections;

public class BurstBehavior : MonoBehaviour {

    private float curLifeTime;
    public float startDisappearTime=3f;
    public float lifeTime=2f;
    private float moveSpeed = 1000;
    private float BurstTimeout = 0.5f;
    public bool ShouldBurst=true;
    public float wallBump = 20f;
    private SphereCollider myCollider;
    private Rigidbody myBody;
    public SpriteRenderer spriteComp;
    private AsteroidSpawner spawnerScript;

    private Color visibleColor;
    private Color nearInvisibleColor;
    private bool isNewAsteroid=true;
    public bool ReadyToConsume;
    public bool newSpawnedAsteroid(bool isNew) { return isNewAsteroid = isNew; }
    public bool asteroidStatus() { return isNewAsteroid; }
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myCollider = GetComponent<SphereCollider>();
        spawnerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AsteroidSpawner>();
        visibleColor = spriteComp.color;
        nearInvisibleColor = spriteComp.color;
        nearInvisibleColor.a = 0.7f;
    }

    void OnEnable()
    {
        if(ShouldBurst)
        {
            StartCoroutine(StartBurst());
        }
        StartCoroutine(OrbExpiring());
    }
    
    IEnumerator StartBurst()
    {
        gameObject.tag = "Untagged";
        //myBody.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
        //myCollider.enabled = false;
        transform.position += moveSpeed * transform.forward * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        yield return new WaitForSeconds(BurstTimeout);
        //myCollider.enabled = true;
        //ResetVelocity();
        ChangeTag();
    }

    IEnumerator OrbExpiring()
    {
        curLifeTime = lifeTime * 2 / 9;
        yield return new WaitForSeconds(startDisappearTime);
        spriteComp.color = nearInvisibleColor;
        
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = visibleColor;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = nearInvisibleColor;
        nearInvisibleColor.a = 0.5f;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = visibleColor;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = nearInvisibleColor;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = visibleColor;
        nearInvisibleColor.a = 0.3f;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = nearInvisibleColor;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = visibleColor;
        yield return new WaitForSeconds(curLifeTime);
        spriteComp.color = nearInvisibleColor;
        nearInvisibleColor.a = 0.7f;
        spawnerScript.ReturnPooledAsteroid(gameObject);
    }

    void ChangeTag()
    {
        gameObject.tag = "Asteroid";
    }

    private void OnCollisionEnter(Collision collision)
    {
        string curTag = collision.gameObject.tag;
        if(curTag=="Wall")
        {
            myBody.AddForce(collision.contacts[0].normal * wallBump, ForceMode.VelocityChange);
        }
    }

    public void ResetVelocity()
    {
        if (myBody)
        {
            myBody.velocity = Vector3.zero;
        }
    }

    public bool GoBurst()
    {
        return ShouldBurst = true;
    }
}
