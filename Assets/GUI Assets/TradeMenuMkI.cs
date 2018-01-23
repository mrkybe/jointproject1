﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pressing the spacebar should bring up a list of nearby ships.
/// Pressing space again closes the list.
/// 
/// Next up:
/// Click on ships in the list to open the list of their cargo.
/// 
///
/// 
/// NOTE:  There is quite a bit of just plain-old bad stuff in here that I don't like.
/// I intend to fix it in later versions.
/// </summary>
/// 

public class TradeMenuMkI : MonoBehaviour {


    public GameObject buttonPrefab;

    private Spaceship myShip;

    private RectTransform leftPanel;
    private RectTransform rightPanel;

    private Vector2 on;
    private Vector2 leftOffPosition;
    private Vector2 rightOffPosition;
    private bool isRightOff;
    private bool isLeftOff;
    private List<GameObject> myButtonList;

    private void Awake()
    {
        leftPanel = GameObject.Find("Left Panel").GetComponent<RectTransform>();
        rightPanel = GameObject.Find("Right Panel").GetComponent<RectTransform>();
        on = new Vector2(0, 0);
        leftOffPosition = new Vector2(-675, 0);                         // Don't love that this is just hard-coded
        rightOffPosition = new Vector2(675, 0);
        isRightOff = true;
        isLeftOff = true;
        myShip = GameObject.Find("AI_ship_player").GetComponent<Spaceship>();
        myButtonList = new List<GameObject>();

        int i = 0;

        for (i = 0; i < 15; i++)                                        // Only 15 buttons.  Look into later.
        {
            GameObject myButton = (GameObject)Instantiate(buttonPrefab);
            myButtonList.Add(myButton);
        }

        foreach(var b in myButtonList)                                  // List of buttons so they can be edited and set active or inactive.
        {
            b.transform.SetParent(leftPanel, false);
            b.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))                            //  Remember to change to Gamepad controls.
        {
            if (isLeftOff)
            {
                Time.timeScale = 0;                                     // Not sure this is the best way to pause the game.
                leftPanel.anchoredPosition = on;
                isLeftOff = false;

                List<Spaceship> shipsInRange = myShip.GetShipsInInteractionRange();

                int i = 0;

                if (shipsInRange.Count <=15)                            // I only made 15 buttons, for now.  Might add scrolling, later.
                {
                    for (i = 0; i < shipsInRange.Count; i++)
                    {
                        myButtonList[i].GetComponentInChildren<Text>().text = shipsInRange[i].name;
                        myButtonList[i].SetActive(true);
                        if (shipsInRange.Count < 15)
                        {
                            int j = 0;
                            for (j=shipsInRange.Count; j<15; j++)
                            {
                                myButtonList[j].SetActive(false);
                            }
                        }
                    }
                }
                else if (shipsInRange.Count == 0)
                {
                    for (i = 0; i < 15; i++)
                    {
                        string nullstring = "";
                        myButtonList[i].GetComponentInChildren<Text>().text = nullstring;
                        myButtonList[i].SetActive(false);
                    }
                }
            }
            else
            {
                int i = 0;
                for (i=0;i<15;i++)
                {
                    myButtonList[i].GetComponentInChildren<Text>().text = "";
                    myButtonList[i].SetActive(false);
                }
                Time.timeScale = 1;                                     // Again, might not be the best way to pause/unpause.
                leftPanel.anchoredPosition = leftOffPosition;
                isLeftOff = true;
            }
        }
    }
}