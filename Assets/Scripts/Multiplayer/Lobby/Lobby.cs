using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{
    //client's version number. separate s user from each other by gameVersion
    [SerializeField]
    private string gameVersion = "1";
    [SerializeField]
    private byte maxPlayerPerRoom = 4;
    [SerializeField]
    private GameObject controlPanel, loadingPanel;

    [SerializeField]
    private Text logger;


    #region MonoBehaviour
    private void Start()
    {
        controlPanel.SetActive(true);
        //loadingPanel.SetActive(false);
    }
    #endregion


    #region MonobehaviourPunCallbacks

    public override void OnConnectedToMaster()
    {
        Log("<color=Green>Connected to Master.</color>");
        Debug.Log("Connected to Master!");
        //First we try to join a potential existing room.
        //If there is a room, then we connect to that room
        //Else, we need to handle what happens when we failed to join a random room
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        controlPanel.SetActive(true);
        loadingPanel.SetActive(false);
        Log("Disconnected! " + cause);
        Debug.Log("Disconnected because " + cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log("<color=Red>Failed to join a room:</color> " + message);
        //If no rooms are available or all rooms are at max capacity, simply create a new room
        Log("Creating a room...");
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room", new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Log("<color=Green> Successfully joined " + PhotonNetwork.CurrentRoom.Name + "</color>");
        //Load the scene using Photon Network
        PhotonNetwork.LoadLevel(1);
    }

    #endregion


    /// <summary>
    /// Start the connection process
    /// - If we are already connect to photon's network, attempt to join a random room
    /// - If not yet connected, connect to the photon cloud network
    /// </summary>
    public void Connect()
    {
        controlPanel.SetActive(false);
        loadingPanel.SetActive(true);
        //Check if connected
        if (PhotonNetwork.IsConnected)
        {
            Log("Joining a random room...");
            Debug.Log("We are already connected to photon cloud network..Finding a random room.");
            //Attempt to join a random room
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Log("Connecting to Photon Network...");
            Debug.Log("Connecting to photon cloud network...");
            //Connect to the Photon Online Server
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    private void Log(string message)
    {
        //don't do anything when the logger Text is not assigned
        if (logger == null)
            return;
        logger.text += System.Environment.NewLine + message;
    }
   
}
