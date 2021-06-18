using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ShootMinigun : MonoBehaviour
{
    private PhotonView photonView;

    private Bullet bullet;
    public Transform firePoint;
    public GameObject BulletPrefab;
    public GameObject UpgradedMinigunPrefab;

    public float bulletForce = 20f;
    public float fireRate = 1f;
    public float nextFire = 0;

    public bool upgraded;

    //public GameObject gun1;
    //public GameObject gun2;
    // Start is called before the first frame update
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        bullet = GetComponent<Bullet>();
        upgraded = false;
    }

    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;
    }

    private void Disable()
    {
        if (!photonView.IsMine)
            return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FireRate();
    }

    void FireRate()
    {
        if (TokhangManager.starting == true) //startgame
        {
            if (!Health.isDead)
            {
                if (Input.GetMouseButton(0) && Time.time > nextFire && !upgraded)
                {
                    nextFire = Time.time + fireRate;
                    if (!photonView.IsMine)
                        return;
                    photonView.RPC("shoot", RpcTarget.AllViaServer);
                }
                if (Input.GetMouseButton(0) && Time.time > nextFire && upgraded)
                {
                    nextFire = Time.time + fireRate;
                    if (!photonView.IsMine)
                        return;
                    photonView.RPC("Upgradedshoot", RpcTarget.AllViaServer);
                }
            }

        }
    }

    [PunRPC]
    private void shoot()
    {
        FindObjectOfType<AudioManager>().Play("minigun");
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
    [PunRPC]
    private void Upgradedshoot()
    {
        FindObjectOfType<AudioManager>().Play("minigun");
        GameObject bullet = Instantiate(UpgradedMinigunPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
