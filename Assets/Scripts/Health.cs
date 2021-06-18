using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
 
    //  private PhotonView photonView;//multiplayer
    public Text lives;

    public int nooflives;

    [SerializeField]
    private int spawnPicker;
    public Transform[] respawnPoint;

    public int maxPlayerHealth;
    public int currentPlayerHealth;

    public GameObject deadEffect;

    public HealthBar healthbar;

    public GameObject playerIcon;

    public static int life;

    private bool isReviving = false;
    public static bool isDead = false;

    //for killed
    //public ParticleSystem Destruction //ghost;
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;
    private new SpriteRenderer renderer;

    [SerializeField]
    public Sprite tempSprite; //for ghost sprite
    public Sprite ghostSprite;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        healthbar.SetMaxHealth(maxPlayerHealth);
    }

    private void Update()
    {
      
        lives.text = nooflives.ToString();
     
        spawnPicker = Random.Range(0, respawnPoint.Length);
        checkLives();
    }

    public void TakeDamage(Photon.Realtime.Player from, int value) //enemy.cs
    {

        currentPlayerHealth -= value;
        Debug.Log("Health" + currentPlayerHealth);

        if (currentPlayerHealth <= 0 && nooflives > 0)//LevelManager.instance.Player1Lives > 0) //dying with lives
        {
            //checkLives();
            DestroySpaceship();
         // photonView.RPC("DestroySpaceship", RpcTarget.AllViaServer);
          
        }
        if(currentPlayerHealth <= 0 && nooflives==0)//LevelManager.instance.Player1Lives == 0) //perma ded
        {
            //checkLives();
            gameOver();
        }

        healthbar.SetHealth(currentPlayerHealth);
    }

   
    private void checkLives()
    {
        //minus in the text of lives
    }

  
    //private void Dead()
    //{
        
    //    //LevelManager.instance.Player1Lives -= 1;
    //     nooflives --;
       

    //    this.gameObject.transform.position = respawnPoint[spawnPicker].transform.position;

    //    this.currentPlayerHealth = maxPlayerHealth; 
    //    //LevelManager.instance.Respawn();
    //    Debug.Log("IsDead");
    //}

    private void gameOver()
    {
        Debug.Log("No more lives");
        gameObject.SetActive(false);
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //test geo, hover at the function, gud description
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.currentPlayerHealth);
            stream.SendNext(this.nooflives);
            stream.SendNext(this.spawnPicker);
        }
        else
        {
            // Network player, receive data
            this.currentPlayerHealth = (int)stream.ReceiveNext();
            this.nooflives = (int)stream.ReceiveNext();
            this.spawnPicker = (int)stream.ReceiveNext();
        }
    }
    private IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(TokhangGame.PLAYER_RESPAWN_TIME);

        photonView.RPC("RespawnSpaceship", RpcTarget.AllBuffered,null);
        //RespawnSpaceship();
    }

    //[PunRPC]
    public void DestroySpaceship() //not working
    {
        isDead = true;
        nooflives -= 1;
        //this.renderer.enabled = false;
        Destroy(Instantiate(deadEffect, transform.position, Quaternion.identity), 1);


        //collider.enabled = false;
        //rigidbody.enabled = false;

        //GetComponent<SpriteRenderer>().enabled = false;
        

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;//disabled
        

        photonView.RPC("ChangeSpriteToGhost", RpcTarget.AllBuffered,null);
        this.gameObject.tag = "Ghost";
        this.gameObject.layer = 11;
        
        
        this.currentPlayerHealth = maxPlayerHealth;

        

        if(isReviving)
        {
            return;
        }   
        
        if(!isReviving)
        {
            isReviving = true;
            if (nooflives > -1)
             {
                StartCoroutine("WaitForRespawn");
                isReviving = false;
               
            }
        }

  
            object lives;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(TokhangGame.PLAYER_LIVES, out lives))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { TokhangGame.PLAYER_LIVES, ((int)lives <= 1) ? 0 : ((int)lives - 1) } });
            }
    }

    [PunRPC]
    public void RespawnSpaceship()
    {
        renderer.sprite = NetworkManager.Instance.GetPlayerSprite(photonView.Owner.GetPlayerNumber());

        this.gameObject.transform.position = respawnPoint[spawnPicker].transform.position;
       
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        //this.renderer.enabled = true;
        isDead = false;

        

        this.gameObject.tag = "Player";
        this.gameObject.layer = 10;
        //EngineTrail.SetActive(true);
        //Destruction.Stop();
    }

    [PunRPC]
    public void ChangeSpriteToGhost()
    {
        this.renderer.sprite = ghostSprite;
    }


}
