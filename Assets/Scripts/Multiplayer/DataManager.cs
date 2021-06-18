using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DataManager : SingletonPUN<DataManager>
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float playerScore;
    
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private float currentHealth;

    public delegate void HealthUpdate(float current, float max);
    public static event HealthUpdate OnHealthChanged;

    public bool IsGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }

    public float Health
    {
        get
        {
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                GameOver();
            }
            return currentHealth; 
        }
        set
        {
            SetHealth(value);
        }
    }

  

    void SetHealth(float value)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        currentHealth = value;
        //OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
            currentHealth = 0;

        object[] data = new object[] { currentHealth, maxHealth };
        Debug.Log("Raising SetHealth Event");
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(NetworkManager.SET_HEALTH_EVENT_CODE, data, raiseEventOptions, SendOptions.SendUnreliable);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Health = maxHealth;
    }

    private void GameOver()
    {
        IsGameOver = true;
    }
}
