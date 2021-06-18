using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ExitGames.Client.Photon;
using Photon.Realtime;

public class ChangeGun : MonoBehaviour
{
    private PhotonView photonView;
    private bool PowerisDestroyed;


    public int changewep;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Pickup(collision);
        }
    }
    private void Update()
    {
        StartCoroutine(noPick());
    }
    void Pickup(Collider2D player)
    {
        // make fx

        //apply power effect
        FindObjectOfType<AudioManager>().Play("gunPickUp");
        WeaponHolder wep = player.GetComponentInChildren<WeaponHolder>();
        wep.selectedWep = changewep;
        Debug.Log("Changed wep");
        Destroy(gameObject);
        Debug.Log("Power Up Picked");
    }

    IEnumerator noPick() //if no one picks the powerup destroy gameobj
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
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
        PowerisDestroyed = true;
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void DestroyLocally()
    {
        PowerisDestroyed = true;
    }


}
