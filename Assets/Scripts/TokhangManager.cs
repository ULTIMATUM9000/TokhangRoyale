using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TokhangManager : MonoBehaviourPunCallbacks
{
    
    public static TokhangManager Instance = null;

    public Text InfoText;
    [SerializeField] GameObject MessagePanel;

    public GameObject spawnPowerUp;
    public GameObject spawnStatsUp;
    public GameObject[] bullet;
    public static bool starting = false;

    public void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    public void Start()
    {
        Hashtable props = new Hashtable
            {
                {TokhangGame.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        foreach (GameObject p in bullet)
        {
            p.SetActive(false);
        }
    }

    private IEnumerator EndOfGame(string winner)
    {
        float timer = 5.0f;

        MessagePanel.SetActive(true);

        while (timer > 0.0f)
        {
            
            InfoText.text = string.Format("Player {0} won.\n\n\nReturning to login screen in {1} seconds.", winner, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
 
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }


        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(TokhangGame.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    FindObjectOfType<AudioManager>().Play("countdown");
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ", this.InfoText);
                InfoText.text = "Waiting for other players...";
            }
        }
    }

    private void StartGame()
    {
        Debug.Log("StartGame!");

        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false; //dont let anyone join
        starting = true;
        spawnPowerUp.SetActive(true);
        spawnStatsUp.SetActive(true);

        foreach (GameObject p in bullet)
        {
            p.SetActive(true);
        }
        // on rejoin, we have to figure out if the spaceship exists or not
        // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate


        //float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        //float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        //float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        //Vector3 position = new Vector3(x, 0.0f, z);
        //Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

        //PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)

    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(TokhangGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    public void CheckEndOfGame() 
    {
        bool allDestroyed = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object lives;
            if (p.CustomProperties.TryGetValue(TokhangGame.PLAYER_LIVES, out lives))
            {
                if ((int)lives > 0)
                {
                    allDestroyed = false;
                    break;
                }
            }
        }

        if (allDestroyed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                }
            }

            StartCoroutine(EndOfGame(winner));
        }
		else
		{
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int highestLifeCount = 0;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                p.CustomProperties.TryGetValue(TokhangGame.PLAYER_LIVES, out lives);
               
                if ((int)lives > highestLifeCount)
                {
                    highestLifeCount = (int)lives;
                    winner = p.NickName;
                }
            }
            StartCoroutine(EndOfGame(winner));
        }
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
}
