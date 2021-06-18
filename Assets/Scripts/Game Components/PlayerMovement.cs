using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView photonView;//multiplayer
    private SpriteRenderer spriteRenderer;

    //int var
    public Vector2 moveSpeed;
    public float dashing = 5.0f;
    public float jumpForce = 300f;

    public GameObject jumpeffect;
    public GameObject dasheffect;
    private Animator animator;
    private Rigidbody2D _rigidbody2D;

    //for flip
    private bool facingRight = true;

    //for move
    public float horizontalMovement;
    public float VerticalMovement;

    //for jump 
    public Transform groundCheck;
    public float checkRadius;
    private bool isGrounded;
    public LayerMask whatIsGround;
    private bool isJumping = false;

    //for double jump
    private int extraJumps;
    public int extraJumpsValue;

    //for dash
    private int extraDash;
    public int extraDashValue;
    public GameObject dashEffect;
    //dash 2.0

    public GameObject HasteEffect;
    public GameObject DDEffect;
    public float DashForce;
    public float StartDashTimer;
    bool isDashing;

    float currentDashTimer;
    float DashDirection;

    private SpriteRenderer mySprite;

    //for health
    private float Health;
    private float minHealth = 0;
    private float maxHealth = 100;

    private bool isHit = false; //to not get double hit
    private void Awake()
    {
        photonView = GetComponent<PhotonView>(); //multiplayer
        NetworkManager.OnNumberingUpdated += UpdatePlayerNumbering;

        animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();



        extraJumps = extraJumpsValue;
        extraDash = extraDashValue;
        moveSpeed = new Vector2(3f, 0f);

        HasteEffect.SetActive(false);
    }

    private void OnDestroy()
    {
        NetworkManager.OnNumberingUpdated -= UpdatePlayerNumbering;
    }

    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;
    }

    private void OnDisable()
    {
        if (!photonView.IsMine)
            return;
    }
    void Start()
    {
      
    }

    void FixedUpdate()
    {
        //healthtext.Text = Health; //For the text of health UI
        float characterSpeed = Mathf.Abs(_rigidbody2D.velocity.x);
        animator.SetFloat("Speed", characterSpeed);
        animator.SetBool("isGrounded", isGrounded);
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
            return;

        CheckForFlip();
        CheckForMovement();
        CheckForHealth();
       
    }

    void CheckForHealth()
	{
        if(Health > maxHealth)
		{
            Health = maxHealth;
		}
        if(Health < minHealth)
		{
            Health = minHealth;
		}
	}
    public void HasHaste()
    {
        HasteEffect.SetActive(true);
    } 
    public void HasNotHaste()
    {
        HasteEffect.SetActive(false);
    }

    public void HasDD()
    {
        DDEffect.SetActive(true);
    }
    public void HasNoDD()
    {
        DDEffect.SetActive(false);
    }

    void CheckForFlip()
	{
        if (horizontalMovement > 0)
        {
            photonView.RPC("FlipRight", RpcTarget.AllBufferedViaServer);
            // FlipRight();

        }
        else if (horizontalMovement < 0)
        {
            photonView.RPC("Flip", RpcTarget.AllBufferedViaServer);
            // Flip();
        }
    }
    [PunRPC]
    public void InstantiateEffect()
    {
        Destroy(Instantiate(jumpeffect, transform.position, Quaternion.identity), 1);
    }

    [PunRPC]
    public void InstantiateDash()
    {
        Destroy(Instantiate(dasheffect, transform.position, Quaternion.identity), 1);
    }
    void CheckForMovement()
	{
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed.x;
        VerticalMovement = Input.GetAxis("Vertical") * moveSpeed.y;

        _rigidbody2D.velocity = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)//jumping 
        {
            FindObjectOfType<AudioManager>().Play("jump");
            animator.SetBool("isJumping", true);
            // FindObjectOfType<AudioManagerScript>().Play("Jump_Sound");
            isJumping = true;
            photonView.RPC("InstantiateEffect",RpcTarget.AllViaServer);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(new Vector2(0, jumpForce));
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            //FindObjectOfType<AudioManagerScript>().Play("Jump_Sound");
            FindObjectOfType<AudioManager>().Play("jump");
            _rigidbody2D.velocity = Vector2.up * jumpForce;
        }

        //if (Input.GetKeyDown(KeyCode.LeftShift))//dashing
        //{
        //x
        //    moveSpeed.x = dashing;
        //}
        //if (Input.GetKeyUp(KeyCode.LeftShift))//not dashing
        //{
        //x
        //    moveSpeed.x = normalSpeed;
        //}

        //if (Input.GetKeyDown(KeyCode.LeftShift))//dash horizontal
        //{

        //    if (extraDash >= 1)
        //    {
        //        // FindObjectOfType<AudioManagerScript>().Play("slash_Sound");
        //        StartCoroutine("DashMove");
        //        //GameObject dash = Instantiate(dashEffect, transform.position, Quaternion.identity);
        //        //dash.transform.parent = transform;
        //        //Destroy(dash, 1);
        //        
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (extraDash >= 1)
            {
                isDashing = true;
                FindObjectOfType<AudioManager>().Play("dash");
                photonView.RPC("InstantiateDash",RpcTarget.AllViaServer);
                currentDashTimer = StartDashTimer;
                _rigidbody2D.velocity = Vector2.zero;
                DashDirection = (int)horizontalMovement;
                --extraDash;
            }

        }
        if(isDashing)
        {
            _rigidbody2D.velocity = transform.right * DashDirection * DashForce;
            currentDashTimer -= Time.deltaTime;

            if(currentDashTimer <0)
            {
                isDashing = false;
                --extraDash;
            }
        }

            if (isGrounded == true)//reset for jump
        {
            extraJumps = 1;
            extraDash = 1;
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }
    }

    [PunRPC]
    private void FlipRight()
    {
        //facingRight = true;
        mySprite.flipX = true;
    }

    [PunRPC]
    void Flip()
    {
        mySprite.flipX = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }

        if(collision.gameObject.tag == "Bullet")
		{
            StartCoroutine("HitFrame");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            bullet.NetworkDestroy();
        }
        
    }

    IEnumerator HitFrame() //fix bullet issue
    {
        if (isHit) //stack the timer
        {
            Bullet bullet = gameObject.GetComponent<Bullet>();
           int temp = bullet.bulletDamage;
            bullet.bulletDamage = 0;
            bullet.NetworkDestroy();
            bullet.bulletDamage = temp;

        }
        if (!isHit)// If not invincible already, become invincible.
        {
            isHit = true;
           
            yield return null;
          
            isHit = false;
        }
    }

    IEnumerator DashMove()
    {
        //movespeed.x

        _rigidbody2D.velocity = Vector2.zero;
        moveSpeed.x += 10;

        _rigidbody2D.gravityScale = 0;
        yield return new WaitForSeconds(0.3f);
        _rigidbody2D.gravityScale = 1;

        moveSpeed.x -= 10;
    }
    

    private void UpdatePlayerNumbering() //multiplayer
    {
        //If the PlayerNumber has not yet been updated, don't do anything
        if (photonView.Owner.GetPlayerNumber() == -1)
            return;

        //Assign the player sprite to the renderer
        NetworkManager.Instance.Log(string.Format("Nickname: {0} Actor Number: {1} PlayerNumber: {2}", photonView.Owner.NickName, photonView.Owner.ActorNumber, photonView.Owner.GetPlayerNumber()));
        photonView.RPC("UpdatePlayerSprite", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void UpdatePlayerSprite()
    {
        mySprite.sprite = NetworkManager.Instance.GetPlayerSprite(photonView.Owner.GetPlayerNumber());
    }


}
