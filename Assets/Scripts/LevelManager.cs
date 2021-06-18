using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform respawnPoint;
    public GameObject playerPrefab;

    public int Player1Lives = 2;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn()
    {
        //PhotonNetwork.Instantiate(playerPrefab.name, respawnPoint.position, Quaternion.identity);
        //playerPrefab.transform.position = respawnPoint.transform.position;
    }
}
