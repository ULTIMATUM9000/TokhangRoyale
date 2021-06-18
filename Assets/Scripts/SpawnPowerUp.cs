using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ExitGames.Client.Photon;
using Photon.Realtime;
public class SpawnPowerUp : SingletonPUN<SpawnPowerUp>
{
    public Transform[] powerUpLocation;

    float minSpawnInterval = 7;
    float maxSpawnInterval = 10;

    float SpawnInterval;

	protected override void Awake()
	{
		base.Awake();
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


    void HandleSpawning()
	{
        StartCoroutine(SpawnCoroutine());
	}

    IEnumerator SpawnCoroutine()
	{
        while(true)
		{
            yield return new WaitForSeconds(SpawnInterval);
            SpawnGuns();
            SpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
	}

    void SpawnGuns()
    {
        int i = Random.Range(0, 4);

        switch(i)
		{
            case 0:
                PhotonNetwork.Instantiate("PowerUpRifle", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;
            case 1:
                PhotonNetwork.Instantiate("PowerUpSniper", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate("PowerUpMinigun", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;
            case 3:
                PhotonNetwork.Instantiate("PowerUpShotgun", powerUpLocation[Random.Range(0, powerUpLocation.Length)].position, Quaternion.identity);
                break;

        }
        Debug.Log("Spawned at " + i);
    }
}
