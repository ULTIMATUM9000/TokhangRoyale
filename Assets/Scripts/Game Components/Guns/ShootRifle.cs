using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ShootRifle : MonoBehaviour
{
    private PhotonView photonView;

    private Bullet bullet;
    public Transform firePoint;
    public GameObject BulletPrefab;
    public GameObject UpgradedRiflePrefab;

    public float bulletForce = 20f;
    public float fireRate = 1f;
    public float nextFire = 0;

    public bool upgraded;

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
    void Update()
    {
        FireRate();
    }

    void FireRate()
    {
        if (!Health.isDead)
        {
            if (Input.GetMouseButton(0) && Time.time > nextFire && !upgraded)
            {
                nextFire = Time.time + fireRate;
                if (!photonView.IsMine)
                    return;
                photonView.RPC("shootR", RpcTarget.AllViaServer);
            }
            if (Input.GetMouseButton(0) && Time.time > nextFire && upgraded)
            {
                nextFire = Time.time + fireRate;
                if (!photonView.IsMine)
                    return;
                photonView.RPC("UpgradedshootR", RpcTarget.AllViaServer);
            }
        }
    }

    [PunRPC]
    private void shootR()
    {
        FindObjectOfType<AudioManager>().Play("rifle");
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    [PunRPC]
    private void UpgradedshootR()
    {
        FindObjectOfType<AudioManager>().Play("rifle");
        GameObject bullet = Instantiate(UpgradedRiflePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
    //   // Start is called before the first frame update
    //   public override void shoot()
    //{
    //	base.shoot();
    //	FindObjectOfType<AudioManager>().Play("rifle");
    //}

}
