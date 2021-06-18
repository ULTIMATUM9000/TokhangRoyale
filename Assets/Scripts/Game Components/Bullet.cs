using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private PhotonView photonView;

    public static Bullet instance;
    private Health player;
    public Rigidbody2D playerRB;
    //public Rigidbody playerRigidbody;
    [SerializeField]
    private Photon.Realtime.Player owner;
    [SerializeField]
    private string ownerNickname;

    public int baseBulletDamage;
    public int bulletDamage;

    private bool BulletisDestroyed;

    public GameObject hiteffect;
    public Vector3 moveDirection;

    private float yMax, yMin, xMax, xMin;
    private void Awake()
    {
        instance = this;
        //bulletDamageChecker = bulletDamage;
        bulletDamage = baseBulletDamage;
        photonView = GetComponent<PhotonView>();

    }
    void Start()
    {
        player = gameObject.GetComponent<Health>();
        
        //Destroy(gameObject, LifeTime);
    }
    private void OnEnable()
    {
        CalculateScreenRestrictions();
        if (!photonView.IsMine)
            return;
    }

    private void OnDisable()
    {
        if (!photonView.IsMine)
            return;
    }

    void Update()
    {
        CheckIfOutOfBounds();
    }
    
    public void changeBulletDamage(int damage)
    {
        bulletDamage = damage;
    }

    //private void calculatedTransform()
    //{
    //    Vector3 playerpos = playerRB.transform.position;
    //}
    //[PunRPC]
    //public void BulletEffect()
    //{
        
    //}
    IEnumerator OnTriggerEnter2D(Collider2D hitInfo) //hitting something
    {
        Health health = hitInfo.GetComponent<Health>();
        //Rigidbody2D enemy = hitInfo.GetComponent<Rigidbody2D>(); //access the 

        //if (enemy == null)
        //   yield return null;


        //Debug.Log(enemy.transform.position);
        if (health != null && hitInfo.CompareTag("Player"))
        {
            health.TakeDamage(owner, bulletDamage);
            Destroy(Instantiate(hiteffect, transform.position, Quaternion.identity), 1);
            //photonView.RPC("BulletEffect", RpcTarget.AllBufferedViaServer);
            Destroy(gameObject);

        }
        if(hitInfo.CompareTag("Player"))//pushback
        {
           // moveDirection = playerRB.transform.position - enemy.transform.position;
          //  enemy.AddForce(moveDirection.normalized * -25f,ForceMode2D.Impulse);
        }
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("Ground")) //avoids powerup
        {
            yield return null;
            //photonView.RPC("BulletEffect", RpcTarget.AllBufferedViaServer);
            Destroy(Instantiate(hiteffect, transform.position, Quaternion.identity), 1);
            Destroy(gameObject);
        }

        yield return null;
    }
    protected void CalculateScreenRestrictions()
    {
        Vector3 upperLeft = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 lowerLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        yMax = upperLeft.y;
        yMin = lowerLeft.y;
        xMin = lowerLeft.x;
        xMax = upperLeft.x;
    }

    protected void CheckIfOutOfBounds()
    {
        //Disable the object when off screen
        if ((transform.position.x > xMax || transform.position.x < xMin) || (transform.position.y > yMax || transform.position.y < yMin))
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(Photon.Realtime.Player from, float damage)
    {
        if (BulletisDestroyed)
        { return; }

        //Remove the object in the network
        NetworkDestroy();

    }
    public void NetworkDestroy()
    {
        //Check if the client owns this instance
        if (photonView.IsMine)
            DestroyGlobally();
        else
            DestroyLocally();
    }
    private void DestroyGlobally()
    {
        BulletisDestroyed = true;
        PhotonNetwork.Destroy(this.gameObject);
    }
    private void DestroyLocally()
    {
        BulletisDestroyed = true;
    }
}
