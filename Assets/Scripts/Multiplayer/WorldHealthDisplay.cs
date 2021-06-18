using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class WorldHealthDisplay : MonoBehaviour, IOnEventCallback
{
    public Image healthBar;
    public TextMeshProUGUI healthValue;

    private void Awake()
    {
    }

    private void OnDestroy()
    {
        //DataManager.OnHealthChanged -= UpdateHealth;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if(eventCode == NetworkManager.SET_HEALTH_EVENT_CODE)
        {
            Debug.Log("Received Health Data");
            object[] data = (object[])photonEvent.CustomData;
            float currentHealth = (float)data[0];
            float maxHealth = (float)data[1];
            UpdateHealth(currentHealth, maxHealth);
        }
    }

    void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthValue.text = string.Format("{0} / {1}", currentHealth, maxHealth);
    }

   
}
