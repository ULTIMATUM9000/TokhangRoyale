using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ShootShotgun : MonoBehaviour
{
    private PhotonView photonView;

    //lots of sht cuz its shotgun
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    public GameObject BulletPrefab;
    public GameObject BulletPrefab2;
    public GameObject BulletPrefab3;
    public GameObject UpgradedPistolPrefab;
    public GameObject UpgradedPistolPrefab2;
    public GameObject UpgradedPistolPrefab3;

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
                    photonView.RPC("shootSG", RpcTarget.AllViaServer);
                }
                if (Input.GetMouseButton(0) && Time.time > nextFire && upgraded)
                {
                    nextFire = Time.time + fireRate;
                    if (!photonView.IsMine)
                        return;
                    photonView.RPC("UpgradedshootSG", RpcTarget.AllViaServer);
                }
            }

        }
    }

    [PunRPC]
    private void shootSG()
    {
        FindObjectOfType<AudioManager>().Play("shotgun");
        GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
        GameObject bullet2 = Instantiate(BulletPrefab2, firePoint2.position, firePoint2.rotation);
        GameObject bullet3 = Instantiate(BulletPrefab3, firePoint3.position, firePoint3.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        rb2.AddForce(firePoint2.up * bulletForce, ForceMode2D.Impulse);
        rb3.AddForce(firePoint3.up * bulletForce, ForceMode2D.Impulse);
    }
    [PunRPC]
    private void UpgradedshootSG()
    {
        FindObjectOfType<AudioManager>().Play("shotgun");
        GameObject bullet = Instantiate(UpgradedPistolPrefab, firePoint.position, firePoint.rotation);
        GameObject bullet2 = Instantiate(UpgradedPistolPrefab2, firePoint2.position, firePoint2.rotation);
        GameObject bullet3 = Instantiate(UpgradedPistolPrefab3, firePoint3.position, firePoint3.rotation);
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        rb2.AddForce(firePoint2.up * bulletForce, ForceMode2D.Impulse);
        rb3.AddForce(firePoint3.up * bulletForce, ForceMode2D.Impulse);
    }
}
