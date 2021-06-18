using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Photon.Pun.UtilityScripts;

public class PlayerPanelController : MonoBehaviour
{
    public List<PlayerInfoUI> playerInfos;

    private void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        //Debug.Log(PhotonNetwork.PlayerList.Length);
        //update each player information ui based on the number of available players in the network
        for (int i = 0; i < playerInfos.Count; i++)
        {
            //check if there is an available player
            if (i <= PhotonNetwork.PlayerList.Length - 1)
            {
                if (PhotonNetwork.PlayerList[i].GetPlayerNumber() == -1)
                    continue;
                //Set the information of the player
                playerInfos[i].UpdatePlayerInformation(
                    PhotonNetwork.PlayerList[i].NickName,
                    NetworkManager.Instance.GetPlayerIcon(PhotonNetwork.PlayerList[i].GetPlayerNumber())); // NetworkManager.Instance.GetPlayerLives(PhotonNetwork.PlayerList[i].GetPlayerNumber())
            }
            //there is no available player in the current index
            else
            {
                playerInfos[i].SetAsWaiting();
            }
        }

    }
}
