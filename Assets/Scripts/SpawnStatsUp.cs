using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ExitGames.Client.Photon;
using Photon.Realtime;

public class SpawnStatsUp : SingletonPUN<SpawnStatsUp>
{
    public Transform[] powerUpLocation;

    float minSpawnInterval = 15;
    float maxSpawnInterval = 20;

    float SpawnInterval;

    protected override void Awake()
    {
        base.Awake();
        // photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        HandleSpawning();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            if (newMasterClient.ActorNumber == p.ActorNumber)
            {
                HandleSpawning();
            }
        }

    }

    //public override void OnEnable()
    //{
    //    //if (!photonView.IsMine)
    //    //    return;
    //}

    //public override void OnDisable()
    //{
    //    //if (!photonView.IsMine)
    //      //  return;
    //}

    void Update()
    {
        //Invoke("spawn",Random.Range(2f ,5f));
    }
    void FixedUpdate()
    {
        //Counts up
    }

    void HandleSpawning()
    {
        StartCoroutine(SpawnCoroutine());
        //Debug.Log("Spawning Power Ups");
    }



    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);
            SpawnPowerUp();
            SpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnPowerUp()
    {
        int i = Random.Range(0, 2);

        switch (i)
        {
            case 0:
                PhotonNetwork.Instantiate("PowerUpHaste", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;
            case 1:
                PhotonNetwork.Instantiate("PowerUpDamage", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;
        }
        Debug.Log("Spawned at " + i);
    }
}
