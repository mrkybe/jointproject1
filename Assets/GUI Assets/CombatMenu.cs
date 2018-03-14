using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;


public class CombatMenu : MonoBehaviour {

    public Button buttonPrefab;

    private RectTransform leftPanel;

    private List<Button> ships;

    private Spaceship ship;
    private Spaceship otherShip;

    private bool isLeftOff;

    private Vector2 on;
    private Vector2 leftOffPosition;

    // Use this for initialization
    void Awake () {
        leftPanel = GameObject.Find("Left Panel").GetComponent<RectTransform>();

        ships = new List<Button>();

        isLeftOff = true;

        on = new Vector2(0, 0);
        leftOffPosition = new Vector2(-675, 0);

        ship = GameObject.Find("AI_ship_player").GetComponent<Spaceship>();
        otherShip = new Spaceship();

        PopulatePanel(ships, buttonPrefab, leftPanel, 15);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isLeftOff)
            {
                ShowShipsInRange();
            }
            else
            {
                Overseer.Main.UnpauseOvermap();
                leftPanel.anchoredPosition = leftOffPosition;
                isLeftOff = true;
                clearPanel(ships);
            }
        }
    }

    private void clearPanel(List<Button> buttonList)
    {
        int i = 0;
        for (i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponentInChildren<Text>().text = "";
            buttonList[i].gameObject.SetActive(false);
            buttonList[i].onClick.RemoveAllListeners();
        }
    }

    private void PopulatePanel(List<Button> list, Button butt, RectTransform panel, int x)
    {
        int i = 0;

        for (i = 0; i < x; i++)
        {
            butt = Instantiate(butt);
            list.Add(butt);
        }

        foreach (var b in list)                                         // List of buttons so they can be edited and set active or inactive.
        {
            b.transform.SetParent(panel, true);
            b.transform.localScale = butt.transform.localScale;
            b.gameObject.SetActive(false);
        }
    }

    public void ShowShipsInRange()
    {
        Overseer.Main.PauseOvermap();
        leftPanel.anchoredPosition = on;
        isLeftOff = false;

        List<Spaceship> shipsInRange = ship.GetInSensorRange<Spaceship>();
        Faction myFaction = ship.Pilot.Faction;

        int i = 0;

        if (shipsInRange.Count <= ships.Count)       // THIS WILL PROBABLY NEVER CAUSE A PROBLEM, BUT IF IT DOES, SO HELP ME, GOD...
        {
            for (i = 0; i < shipsInRange.Count; i++)
            {
                ships[i].GetComponentInChildren<Text>().text = shipsInRange[i].name;
                ships[i].gameObject.SetActive(true);
                String t = ships[i].GetComponentInChildren<Text>().text;
                ships[i].interactable = true;
                ships[i].onClick.AddListener(() => { StartCombat(t); });
                
                ColorBlock cb = ships[i].colors;
                cb.normalColor = shipsInRange[i].Pilot.Faction.ColorPrimary;
                cb.disabledColor = cb.normalColor * 0.5f;
                ships[i].colors = cb;
            }
        }
        else
        {
            Debug.Log("TOO MANY DAMN SHIPS");
        }
    }

    private void StartCombat(string t)
    {
        otherShip = GameObject.Find(t).GetComponent<Spaceship>();

        Overseer.Main.ResolveShipCombat(ship, otherShip);

        leftPanel.anchoredPosition = leftOffPosition;
        isLeftOff = true;

        clearPanel(ships);
    }
}
