using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// Take 2:  This time I'm trying to use the newer UI stuff, since it seems to be better suited to Button stuff.
/// ...Though the actual changes seem to have been relatively minor.  Whatever.  Still requires me to learn some new stuff.
/// 
/// 
/// </summary>
public class TradeMenuMk2 : MonoBehaviour
{
    public Button buttonPrefab1;
    public Button buttonPrefab2;

    private Spaceship ship;

    private RectTransform leftPanel;
    private RectTransform rightPanel;

    private Vector2 on;
    private Vector2 leftOffPosition;
    private Vector2 rightOffPosition;

    private bool isRightOff;
    private bool isLeftOff;

    private List<Button> buttonListLeft;
    private List<Button> buttonListRight;

    // Use this for initialization
    void Awake()
    {
        leftPanel = GameObject.Find("Left Panel").GetComponent<RectTransform>();
        rightPanel = GameObject.Find("Right Panel").GetComponent<RectTransform>();
        on = new Vector2(0, 0);
        leftOffPosition = new Vector2(-675, 0);                         // Don't love that this is just hard-coded, but I can't think of another way.
        rightOffPosition = new Vector2(675, 0);
        isRightOff = true;
        isLeftOff = true;
        ship = GameObject.Find("AI_ship_player").GetComponent<Spaceship>();
        buttonListLeft = new List<Button>();
        buttonListRight = new List<Button>();

        PopulatePanel(buttonListLeft, buttonPrefab1, leftPanel);
        PopulatePanel(buttonListRight, buttonPrefab2, rightPanel);
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

                List<Spaceship> shipsInRange = ship.GetInInteractionRange<Spaceship>();

                int i = 0;

                if (shipsInRange.Count <= buttonListLeft.Count)
                {
                    for (i = 0; i < shipsInRange.Count; i++)
                    {
                        buttonListLeft[i].GetComponentInChildren<Text>().text = shipsInRange[i].name;
                        buttonListLeft[i].gameObject.SetActive(true);
                        buttonListLeft[i].onClick.AddListener(() => { OpenInventory(buttonListLeft[i]); });
                        print(buttonListLeft[i].GetComponentInChildren<Text>().text);

                        if (shipsInRange.Count < 15)
                        {
                            int j = 0;
                            for (j = shipsInRange.Count; j < 15; j++)
                            {
                                buttonListLeft[j].gameObject.SetActive(false);
                            }
                        }
                    }
                }
                else if (shipsInRange.Count == 0)
                {
                    for (i = 0; i < 15; i++)
                    {
                        string nullstring = "";
                        buttonListLeft[i].GetComponentInChildren<Text>().text = nullstring;
                        buttonListLeft[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                int i = 0;
                for (i = 0; i < 15; i++)
                {
                    buttonListLeft[i].GetComponentInChildren<Text>().text = "";            // Clear list of buttons on menu closing.
                    buttonListLeft[i].gameObject.SetActive(false);
                }
                Time.timeScale = 1;
                leftPanel.anchoredPosition = leftOffPosition;
                isLeftOff = true;
            }
        }
    }

    private void OpenInventory(Button butt)
    {
        if (isRightOff)
        {
            rightPanel.anchoredPosition = on;
            buttonListRight[0].GetComponentInChildren<Text>().text = butt.GetComponentInChildren<Text>().text;
            buttonListRight[0].gameObject.SetActive(true);
            buttonListRight[0].onClick.AddListener(MakeTrade);
            isRightOff = false;
        }
    }

    private void MakeTrade()
    {
        if (!isRightOff)
        {
            rightPanel.anchoredPosition = rightOffPosition;
            isRightOff = true;
        }
    }

    private void PopulatePanel(List<Button> list, Button butt, RectTransform panel)
    {
        int i = 0;

        for (i = 0; i < 15; i++)                                        // Only 15 buttons per panel.  Look into later.
        {
            butt = Instantiate(buttonPrefab1);
            list.Add(butt);
        }

        foreach (var b in list)                                         // List of buttons so they can be edited and set active or inactive.
        {
            b.transform.SetParent(panel, false);
            b.gameObject.SetActive(false);
        }
    }
}

