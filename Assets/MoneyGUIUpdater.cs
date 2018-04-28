using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Helper.Pilot;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGUIUpdater : MonoBehaviour {

    private Text text;
    private GameObject player;
    private PlayerPilot playerPilot;

    //private DateTime time;
    // Use this for initialization
    void Start()
    {
        text = this.GetComponent<Text>();
        player = GameObject.Find("PlayerShip");
        playerPilot = player.GetComponent<PlayerPilot>();
        InvokeRepeating("UpdateMoney", 1, 0.75f);
    }

    void UpdateMoney()
    {
        text.text = "$" + playerPilot.Money;
    }
}
