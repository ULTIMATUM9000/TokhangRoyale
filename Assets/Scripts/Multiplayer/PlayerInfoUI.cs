using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    public GameObject waiting, connected;
    public TextMeshProUGUI playerName; //Name
    public Image playerIcon; //Player Icon
    //public float playerHealth; //Health


    private void Start()
    {
        waiting.SetActive(true);
        connected.SetActive(false);
    }

    public void UpdatePlayerInformation(string name, Sprite icon)
    {
        waiting.SetActive(false);
        connected.SetActive(true);
        playerName.text = name;
        playerIcon.sprite = icon;

    }

    public void SetAsWaiting()
    {
        waiting.SetActive(true);
        connected.SetActive(false);
    }
}
