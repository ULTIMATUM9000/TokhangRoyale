using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ExitGames.Client.Photon;
using Photon.Realtime;

public class DamagePowerUp : MonoBehaviour
{
    private PhotonView photonView;

    public float duration = 5f;

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
        FindObjectOfType<AudioManager>().Play("damage");
        
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        PlayerMovement powerupEffect = player.GetComponent<PlayerMovement>();

        Shoot pistol = player.GetComponentInChildren<Shoot>();
        ShootRifle rifle = player.GetComponentInChildren<ShootRifle>();
        ShootSniper sniper = player.GetComponentInChildren<ShootSniper>();
        ShootMinigun minigun = player.GetComponent<ShootMinigun>();
        ShootShotgun shotgun = player.GetComponent<ShootShotgun>();

        powerupEffect.HasDD();

        ActivateDoubleDamage(pistol,rifle,sniper,minigun, shotgun);

        yield return new WaitForSeconds(duration); //duration of powerup

        powerupEffect.HasNoDD();
        DeactivateDoubleDamage(pistol, rifle, sniper,minigun, shotgun);

        Debug.Log("Damage down");
        Destroy(gameObject);
    }

    void ActivateDoubleDamage(Shoot pistol, ShootRifle rifle, ShootSniper sniper,ShootMinigun minigun, ShootShotgun shotgun)
	{
        if(pistol != null)
		{
            pistol.upgraded = true;
        }
        if (rifle != null)
        {
            rifle.upgraded = true;
        }
        if (sniper != null)
        {
            sniper.upgraded = true;
        }
        if (minigun != null)
        {
            minigun.upgraded = true;
        }
        if (shotgun != null)
        {
            shotgun.upgraded = true;
        }
    }

    void DeactivateDoubleDamage(Shoot pistol, ShootRifle rifle, ShootSniper sniper,ShootMinigun minigun, ShootShotgun shotgun)
	{
        if (pistol != null)
        {
            pistol.upgraded = false;
        }
        if (rifle != null)
        {
            rifle.upgraded = false;
        }
        if (sniper != null)
        {
            sniper.upgraded = false;
        }
        if(minigun != null)
        {
            minigun.upgraded = false;
        }
        if (shotgun != null)
        {
            shotgun.upgraded = false;
        }
    }
}
