using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class NetworkManager : SingletonPUN<NetworkManager>
{
    [SerializeField]
    private Text logger;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private List<Sprite> playerSprites, playerIcon;

    [SerializeField]
    private List<int> playerlives;

    [SerializeField]
    private int ownerId;

    public delegate void PlayerNumberingDelegate();
    public static event PlayerNumberingDelegate OnNumberingUpdated;

    public const byte SET_HEALTH_EVENT_CODE = 1;

    public override void OnEnable()
    {
        base.OnEnable();
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    #region MonoBehaviour
    private void Start()
    {
        //load lobby scene in case not connected to photon services
        if (!PhotonNetwork.IsConnected)
        {
            //Use Unity's Scene Manager to load the Lobby scene
            SceneManager.LoadScene(0);
            return;
        }

        //Make sure there is a gameobject assigned in the inspector
        if(playerPrefab == null)
        {
            Log("<color=Red>No Player Prefab reference.</color>");
        }
        else
        {
            //Instantiate a player networked object for the localplayer
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            Log("<color=Green>Successfully created a player gameobject for " + PhotonNetwork.LocalPlayer.NickName + ".</color>");
            InitializeID();
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks

    public override void OnJoinedRoom()
    {
        Log("<color=Green>" + PhotonNetwork.LocalPlayer.NickName + "</color> has joined the room.");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log("<color=Green>NEW PLAYER:</color>" + newPlayer.NickName + " has joined the room.");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Log("<color=Green>PLAYER:</color>" + otherPlayer.NickName + " has left the room.");
        Log("Remaining players in the room: " + PhotonNetwork.PlayerList.Length);
    }

    //This will be called when changes in playernumbering happens
    private void OnPlayerNumberingChanged()
    {
        //call the functions that will happen when numbering changes if the actorid is same as ownerid
        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            if(p.ActorNumber == ownerId)
            {
                OnNumberingUpdated?.Invoke();
            }
        }
    }

    #endregion

    private void InitializeID()
    {
        ownerId = PhotonNetwork.LocalPlayer.ActorNumber;
        Log("<color=Green>PLAYER: </color>" + PhotonNetwork.LocalPlayer.NickName + "'s id has been initialized to: " + ownerId);
    }

    public void Log(string message)
    {
        //don't do anything when the logger Text is not assigned
        if (logger == null)
            return;
        logger.text += System.Environment.NewLine + message;
    }

    public Sprite GetPlayerSprite(int id)
    {
        return playerSprites[id];
    }

    public Sprite GetPlayerIcon(int id)
    {
        return playerIcon[id];
    }

    //public int GetPlayerLives(int id)
    //{
    //    return playerlives[id];
    //}
}
