using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ExitGames.Client.Photon;
using Photon.Realtime;

public class SpeedPowerUp : MonoBehaviour
{
    private PhotonView photonView;
    private bool PowerisDestroyed;

    public float moveSpeedIncrease = 5f;
    
    public float SpeedDuration = 5f;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Pickup(collision));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        Debug.Log("Speed up");
        FindObjectOfType<AudioManager>().Play("speed");
        PlayerMovement speed = player.GetComponent<PlayerMovement>();
        speed.moveSpeed = new Vector2(moveSpeedIncrease, 0f); //speeds up
        speed.HasHaste();
        //speed.poweredUpH = true; //for effects

        GetComponent<SpriteRenderer>().enabled = false; //to turn off the gameobj
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(5f); //duration of speedup

        speed.HasNotHaste();
        Debug.Log("Speed down");
        speed.moveSpeed = new Vector2(3f, 0f); //speeds down
        Destroy(gameObject);
    }
}
