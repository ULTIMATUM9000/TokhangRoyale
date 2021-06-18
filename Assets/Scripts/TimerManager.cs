using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class TimerManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public int matchLength = 180;
    private int currentMatchTime;
    private Coroutine timerCoroutine;

    public bool perpetual = false;

    [SerializeField] private Text ui_timer;
    TokhangManager tokhangManager;

    public enum EventCodes : byte
	{
        RefreshTimer
	}

	private void Start()
	{
        InitializeTimer();
        tokhangManager = FindObjectOfType<TokhangManager>();
	}

    public void OnEvent(EventData photonEvent)
	{
        if (photonEvent.Code >= 200)
            return;

        EventCodes e = (EventCodes)photonEvent.Code;
        object[] o = (object[])photonEvent.CustomData;

        switch(e)
		{
            case EventCodes.RefreshTimer:
                RefreshTimer_R(o);
                break;
		}
	}

	public void NewMatch_R()
    {
        InitializeTimer();
    }

    private void InitializeTimer()
	{
        currentMatchTime = matchLength;
        RefreshTimerUI();

        if(PhotonNetwork.IsMasterClient)
		{
            timerCoroutine = StartCoroutine(Timer());
		}
	}

    private void RefreshTimerUI()
	{
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        ui_timer.text = $"{minutes}:{seconds}";
	}

    private void EndGame()
	{
        //End the game
        // set game state to ending
        //state = GameState.Ending;

        // set timer to 0
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentMatchTime = 0;
        RefreshTimerUI();

        TokhangManager.starting = false;
        tokhangManager.CheckEndOfGame(); //Calls the TokhangManager End Game

        // disable room
        //if (PhotonNetwork.IsMasterClient)
        //{
        //   PhotonNetwork.DestroyAll();

        //   if (!perpetual)
        //    {
        //        PhotonNetwork.CurrentRoom.IsVisible = false;
        //        PhotonNetwork.CurrentRoom.IsOpen = false;
        //    }
        //}

        // activate map camera
        //mapcam.SetActive(true);

        // show end game ui
        //ui_endgame.gameObject.SetActive(true);
        //Leaderboard(ui_endgame.Find("Leaderboard"));

        // wait X seconds and then return to main menu
        //StartCoroutine(End(5f));
    }

    public void RefreshTimer_S()
    {
        object[] package = new object[] { currentMatchTime };

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.RefreshTimer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
            );
    }

    public void RefreshTimer_R(object[] data)
    {
        currentMatchTime = (int)data[0];
        RefreshTimerUI();
    }

    private IEnumerator Timer ()
	{
        yield return new WaitForSeconds(1f);

        currentMatchTime -= 1;

        if(currentMatchTime <= 0)
		{
            timerCoroutine = null;
            EndGame();
		}
        else
		{
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
		}
	}

    private IEnumerator End(float p_wait)
    {
        yield return new WaitForSeconds(p_wait);

        // disconnect
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }
}
