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

/// <summary>
/// Press Space to open list of nearby moons and ships.
/// Click on one to open inventory.
/// Click on items in inventory to place on center panel.
/// Click buttons in center panel to increase or decrease amount to be traded.
/// Click submit button to confirm trade.
/// Press Space to return to list of nearby agents.
/// Press Space again to close menu.
/// </summary>
public class TradeMenuMk2 : MonoBehaviour
{
    public Button buttonPrefab;
    public Button submitButton;
    public Button elementButton;
    public Button selectorButton;
    public Text textPrefab;

    private RectTransform leftPanel;
    private RectTransform rightPanel;
    private RectTransform centerPanel;
    private RectTransform myElementPanel;
    private RectTransform theirElementPanel;
    private RectTransform theirSelectorUp;
    private RectTransform theirSelectorDown;
    private RectTransform mySelectorUp;
    private RectTransform mySelectorDown;
    private RectTransform myInventoryPanel;
    private RectTransform theirInventoryPanel;
    private RectTransform leftButtonPanel;
    private RectTransform rightButtonPanel;
    private RectTransform leftValuesPanel;
    private RectTransform rightValuesPanel;

    private RectTransform leftTitleBar;
    private RectTransform rightTitleBar;

    private RectTransform myPricesPanel;
    private RectTransform theirPricesPanel;
    private RectTransform myAmountPanel;
    private RectTransform theirAmountPanel;

    private Vector2 on;
    private Vector2 leftOffPosition;
    private Vector2 rightOffPosition;

    private int menuLevel;

    private List<Button> buttonListLeft;
    private List<Button> buttonListRight;
    private List<Button> buttonElementListFrom;
    private List<Button> buttonElementListTo;
    private List<Button> mySelectorListUp;
    private List<Button> mySelectorListDown;
    private List<Button> theirSelectorListUp;
    private List<Button> theirSelectorListDown;

    private List<Text> myAmountSelect;
    private List<Text> theirAmountSelect;
    private List<Text> myInventoryAmounts;
    private List<Text> theirInventoryAmounts;
    private List<Text> myValues;
    private List<Text> theirValues;
    private List<Text> myPrices;
    private List<Text> theirPrices;

    private Spaceship otherShip;
    private Spaceship ship;
    private Planet planet;
    private CargoHold myHold;
    private CargoHold otherHold;

    private Overseer o;

    // Use this for initialization
    void Awake()
    {
        leftPanel = GameObject.Find("Left Panel").GetComponent<RectTransform>();
        rightPanel = GameObject.Find("Right Panel").GetComponent<RectTransform>();
        leftButtonPanel = GameObject.Find("Left Panel Buttons").GetComponent<RectTransform>();
        rightButtonPanel = GameObject.Find("Right Panel Buttons").GetComponent<RectTransform>();
        centerPanel = GameObject.Find("Center Panel").GetComponent<RectTransform>();
        myElementPanel = GameObject.Find("My Element List").GetComponent<RectTransform>();
        theirElementPanel = GameObject.Find("Their Element List").GetComponent<RectTransform>();
        mySelectorUp = GameObject.Find("My Selector Up").GetComponent<RectTransform>();
        mySelectorDown = GameObject.Find("My Selector Down").GetComponent<RectTransform>();
        theirSelectorUp = GameObject.Find("Their Selector Up").GetComponent<RectTransform>();
        theirSelectorDown = GameObject.Find("Their Selector Down").GetComponent<RectTransform>();
        theirAmountPanel = GameObject.Find("Their Numbers").GetComponent<RectTransform>();
        myAmountPanel = GameObject.Find("My Numbers").GetComponent<RectTransform>();
        myInventoryPanel = GameObject.Find("Left Panel Numbers").GetComponent<RectTransform>();
        theirInventoryPanel = GameObject.Find("Right Panel Numbers").GetComponent<RectTransform>();
        leftValuesPanel = GameObject.Find("Left Panel Values").GetComponent<RectTransform>();
        rightValuesPanel = GameObject.Find("Right Panel Values").GetComponent<RectTransform>();
        myPricesPanel = GameObject.Find("My Price").GetComponent<RectTransform>();
        theirPricesPanel = GameObject.Find("Their Price").GetComponent<RectTransform>();
        leftTitleBar = GameObject.Find("Left Title Bar").GetComponent<RectTransform>();
        rightTitleBar = GameObject.Find("Right Title Bar").GetComponent<RectTransform>();

        leftTitleBar.gameObject.SetActive(false);
        rightTitleBar.gameObject.SetActive(false);

        ship = GameObject.Find("PlayerShip").GetComponent<Spaceship>();

        o = GameObject.Find("Overseer").GetComponent<Overseer>();

        centerPanel.gameObject.SetActive(false);

        on = new Vector2(0, 0);
        leftOffPosition = new Vector2(-675, 0);
        rightOffPosition = new Vector2(675, 0);

        menuLevel = 0;

        buttonListLeft = new List<Button>();
        buttonListRight = new List<Button>();
        buttonElementListFrom = new List<Button>();
        buttonElementListTo = new List<Button>();
        mySelectorListUp = new List<Button>();
        mySelectorListDown = new List<Button>();
        theirSelectorListDown = new List<Button>();
        theirSelectorListUp = new List<Button>();

        myAmountSelect = new List<Text>();
        theirAmountSelect = new List<Text>();
        myInventoryAmounts = new List<Text>();
        theirInventoryAmounts = new List<Text>();
        myValues = new List<Text>();
        theirValues = new List <Text>();
        myPrices = new List<Text>();
        theirPrices = new List<Text>();

        otherShip = new Spaceship();
        planet = new Planet();
        otherHold = new CargoHold(GetComponent<MonoBehaviour>(), 0);
        myHold = ship.GetCargoHold;


        PopulatePanel(buttonListLeft, buttonPrefab, leftButtonPanel, 15);
        PopulatePanel(buttonListRight, buttonPrefab, rightButtonPanel, 15);
        PopulatePanel(buttonElementListFrom, elementButton, myElementPanel, 10);
        PopulatePanel(buttonElementListTo, elementButton, theirElementPanel, 10);
        PopulatePanel(mySelectorListDown, selectorButton, mySelectorDown, 10);
        PopulatePanel(mySelectorListUp, selectorButton, mySelectorUp, 10);
        PopulatePanel(theirSelectorListDown, selectorButton, theirSelectorDown, 10);
        PopulatePanel(theirSelectorListUp, selectorButton, theirSelectorUp, 10);

        PopulateNumberPanel(myAmountSelect, textPrefab, myAmountPanel, 10);
        PopulateNumberPanel(theirAmountSelect, textPrefab, theirAmountPanel, 10);
        PopulateNumberPanel(myInventoryAmounts, textPrefab, myInventoryPanel, 15);
        PopulateNumberPanel(theirInventoryAmounts, textPrefab, theirInventoryPanel, 15);
        PopulateNumberPanel(myValues, textPrefab, leftValuesPanel, 15);
        PopulateNumberPanel(theirValues, textPrefab, rightValuesPanel, 15);
        PopulateNumberPanel(myPrices, textPrefab, myPricesPanel, 10);
        PopulateNumberPanel(theirPrices, textPrefab, theirPricesPanel, 10);

        submitButton.onClick.AddListener(MakeTrade);

        MassClear();

        //<FOR TESTING:> 
        myHold.AddHoldType("Dirt");
        myHold.AddHoldType("Water");
        myHold.AddToHold("Dirt", 100);
        //</FOR TESTING>
    }


    void Update()
    {
        if (o.gameState == GameState.InOverMap)
        {
            if (Input.GetKeyDown(KeyCode.Space))                            //  Remember to change to Gamepad controls.
            {
                if (menuLevel == 0)
                {
                    MassClear();
                    ShowAgentsInRange();
                    centerPanel.gameObject.SetActive(false);
                    leftTitleBar.gameObject.SetActive(false);
                    rightTitleBar.gameObject.SetActive(false);
                }
                else if (menuLevel == 1)
                {
                    Overseer.Main.UnpauseOvermap();
                    o.gameState = GameState.InOverMap;
                    leftPanel.anchoredPosition = leftOffPosition;
                    menuLevel = 0;
                    ClearPanel(buttonListLeft);
                    ClearNumberPanel(myInventoryAmounts);
                    centerPanel.gameObject.SetActive(false);
                    leftTitleBar.gameObject.SetActive(false);
                    rightTitleBar.gameObject.SetActive(false);
                }
                else if (menuLevel > 1)
                {
                    rightPanel.anchoredPosition = rightOffPosition;
                    menuLevel = menuLevel - 1;
                    ShowAgentsInRange();
                    centerPanel.gameObject.SetActive(false);
                    leftTitleBar.gameObject.SetActive(false);
                    rightTitleBar.gameObject.SetActive(false);

                    MassClear();

                }
            }
        }
    }

    private void ClearPanel(List<Button> buttonList)
    {
        int i = 0;
        for(i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponentInChildren<Text>().text = "";
            buttonList[i].gameObject.SetActive(false);
            buttonList[i].onClick.RemoveAllListeners();
        }
    }

    private void ClearNumberPanel(List<Text> textList)
    {
        int i = 0;
        for(i = 0; i < textList.Count; i++)
        {
            textList[i].text = "0";
            textList[i].gameObject.SetActive(false);
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

    private void PopulateNumberPanel(List<Text> list, Text t, RectTransform panel, int x)
    {
        int i = 0;

        for (i = 0; i < x; i++)
        {
            t = Instantiate(t);
            list.Add(t);
        }

        foreach (var v in list)
        {
            v.transform.SetParent(panel, true);
            v.transform.localScale = t.transform.localScale;
            v.gameObject.SetActive(false);
        }
    }

    private void ShowAgentsInRange()
    {
        o.gameState = GameState.UI;
        if (!o.IsOvermapPaused())                   // it was definitely overmap_pause_count getting too high, but I didn't want to fiddle with Overseer.
        {
            Overseer.Main.PauseOvermap();
        }
        leftPanel.anchoredPosition = on;
        menuLevel = 1;

        List<Spaceship> shipsInRange = ship.GetInInteractionRange<Spaceship>();
        List<Planet> planetsInRange = ship.GetInInteractionRange<Planet>();
        Faction myFaction = ship.Pilot.Faction;

        int i = 0;

        if (shipsInRange.Count + planetsInRange.Count <= buttonListLeft.Count)       // THIS WILL PROBABLY NEVER CAUSE A PROBLEM, BUT IF IT DOES, BLAME MARK WAHLBERG.
        {
            for (i = 0; i < planetsInRange.Count; i++)
            {
                buttonListLeft[i].GetComponentInChildren<Text>().text = planetsInRange[i].name;
                buttonListLeft[i].gameObject.SetActive(true);
                String t = buttonListLeft[i].GetComponentInChildren<Text>().text;
                buttonListLeft[i].onClick.AddListener(() => {SelectPlanet(t); });
                if (planetsInRange[i].Faction.HostileWith(myFaction))
                {
                    buttonListLeft[i].interactable = false;

                }
                ColorBlock cb = buttonListLeft[i].colors;
                cb.normalColor = planetsInRange[i].Faction.ColorPrimary;
                cb.disabledColor = cb.normalColor * 0.5f;
                buttonListLeft[i].colors = cb;
            }
            int j = 0;
            for (j = 0; j < shipsInRange.Count; j++)
            {
                buttonListLeft[j + i].GetComponentInChildren<Text>().text = shipsInRange[j].name;
                buttonListLeft[j + i].gameObject.SetActive(true);
                String t = buttonListLeft[i + i].GetComponentInChildren<Text>().text;
                buttonListLeft[j + i].onClick.AddListener(() => { SelectShip(t); });
                if (shipsInRange[j].Pilot.Faction.HostileWith(myFaction))
                {
                    buttonListLeft[i + j].interactable = false;
                    
                }
                ColorBlock cb = buttonListLeft[i + j].colors;
                cb.normalColor = shipsInRange[j].Pilot.Faction.ColorPrimary;
                cb.disabledColor = cb.normalColor * 0.5f;
                buttonListLeft[i + j].colors = cb;
            }
            for (i = planetsInRange.Count + shipsInRange.Count; i < buttonListLeft.Count; i++)
            {
                buttonListLeft[i].gameObject.SetActive(false);
            }
        }
        else if (shipsInRange.Count + planetsInRange.Count == 0)
        {
            for (i = 0; i < 15; i++)
            {
                string nullstring = "";
                buttonListLeft[i].GetComponentInChildren<Text>().text = nullstring;
                buttonListLeft[i].gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("TOO MANY NEARBY AGENTS.  HONESTLY, I DON'T KNOW HOW THAT MANY GOT THERE");
        }
    }

    private void SelectShip(string t)
    {
        ClearPanel(buttonListLeft);
        ClearPanel(buttonListRight);
        leftTitleBar.gameObject.SetActive(false);

        buttonListLeft[0].gameObject.SetActive(true);
        buttonListLeft[1].gameObject.SetActive(true);

        buttonListLeft[0].GetComponentInChildren<Text>().text = "Trade";
        buttonListLeft[1].GetComponentInChildren<Text>().text = "Combat";

        buttonListLeft[0].onClick.AddListener(() => { OpenInventoryShip(t); });

        buttonListLeft[1].onClick.AddListener(() => { StartCombat(t); });
    }

    private void StartCombat(string t)
    {
        otherShip = GameObject.Find(t).GetComponent<Spaceship>();

        Overseer.Main.ResolveShipCombat(ship, otherShip);

        leftPanel.anchoredPosition = leftOffPosition;
        
        rightPanel.anchoredPosition = rightOffPosition;

        centerPanel.gameObject.SetActive(false);

        menuLevel = 0;
        MassClear();
    }

    private void SelectPlanet(string t)
    {
        ClearPanel(buttonListLeft);
        ClearPanel(buttonListRight);
        leftTitleBar.gameObject.SetActive(false);

        buttonListLeft[0].gameObject.SetActive(true);
        buttonListLeft[1].gameObject.SetActive(true);

        buttonListLeft[0].GetComponentInChildren<Text>().text = "Trade";
        buttonListLeft[1].GetComponentInChildren<Text>().text = "Bounties";

        buttonListLeft[0].onClick.AddListener(() => { OpenInventoryPlanet(t); });

        buttonListLeft[1].onClick.AddListener(() => { ShowBounties(t); });

    }

    private void ShowBounties(string t)
    {
        // Planet has a Faction, Faction has a list of bounties.
        planet = GameObject.Find(t).GetComponent<Planet>();

        List<Bounty> bounties = planet.Faction.BountyBoard;

        rightPanel.anchoredPosition = on;
        menuLevel = 2;

        int i = 0;
        if (bounties.Count > 0 && bounties.Count <= buttonListRight.Count)
        {
            for (i = 0; i < bounties.Count; i++)
            {
                buttonListRight[i].GetComponentInChildren<Text>().text = bounties[i].Target.name;
                buttonListRight[i].gameObject.SetActive(true);
                String s = bounties[i].Target.name;
                Faction f = planet.Faction;
                buttonListRight[i].onClick.AddListener(() => { DisplayBounty(s, f); });
            }
        }
        else if( bounties.Count > 0 && bounties.Count > buttonListRight.Count)
        {
            for(i = 0; i< buttonListRight.Count; i++)
            {
                buttonListRight[i].GetComponentInChildren<Text>().text = bounties[i].Target.name;
                buttonListRight[i].gameObject.SetActive(true);
                String s = bounties[i].Target.name;
                Faction f = planet.Faction;
                buttonListRight[i].onClick.AddListener(() => { DisplayBounty(s, f); });
            }
        }
    }

    private void DisplayBounty(string s, Faction f)
    {
        // Bounty information
    }

    private void OpenInventoryShip(String otherName)
    {
        centerPanel.gameObject.SetActive(true);
        rightPanel.anchoredPosition = on;
        menuLevel = 2;

        otherShip = GameObject.Find(otherName).GetComponent<Spaceship>();

        otherHold = otherShip.GetCargoHold;

        int otherCargoSize = otherHold.GetCargoItems().Count;

        int myCargoSize = myHold.GetCargoItems().Count;

        leftTitleBar.gameObject.SetActive(true);
        rightTitleBar.gameObject.SetActive(true);

        int i = 0;

        for (i = 0; i < otherCargoSize; i++)
        {
            buttonListRight[i].gameObject.SetActive(true);
            buttonListRight[i].GetComponentInChildren<Text>().text
                = otherHold.GetCargoItems()[i];
            theirInventoryAmounts[i].text = otherHold.GetAmountInHold(otherHold.GetCargoItems()[i]).ToString();
            theirInventoryAmounts[i].gameObject.SetActive(true);
            String s = otherHold.GetCargoItems()[i];
            theirValues[i].text = otherHold.GetCargoItemValue(s).ToString();
            theirValues[i].gameObject.SetActive(true);
            buttonListRight[i].onClick.AddListener(() => { Trade2(s); });

            ColorBlock cb = buttonListRight[i].colors;
            cb.normalColor = Color.white;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListRight[i].colors = cb;
        }

        ClearPanel(buttonListLeft);

        for (i = 0; i < myCargoSize; i++)
        {
            buttonListLeft[i].gameObject.SetActive(true);
            buttonListLeft[i].GetComponentInChildren<Text>().text
                =  myHold.GetCargoItems()[i];
            myInventoryAmounts[i].text = myHold.GetAmountInHold(myHold.GetCargoItems()[i]).ToString();
            myInventoryAmounts[i].gameObject.SetActive(true);
            String s = myHold.GetCargoItems()[i];
            myValues[i].text = myHold.GetCargoItemValue(s).ToString();
            myValues[i].gameObject.SetActive(true);
            buttonListLeft[i].onClick.AddListener(() => { Trade1(s); });

            ColorBlock cb = buttonListLeft[i].colors;
            cb.normalColor = Color.white;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListLeft[i].colors = cb;
        }
    }

    private void OpenInventoryPlanet(String otherName)
    {
        centerPanel.gameObject.SetActive(true);
        rightPanel.anchoredPosition = on;
        menuLevel = 2;

        planet = GameObject.Find(otherName).GetComponent<Planet>();

        otherHold = planet.GetCargoHold;

        int otherCargoSize = otherHold.GetCargoItems().Count;

        int myCargoSize = myHold.GetCargoItems().Count;

        leftTitleBar.gameObject.SetActive(true);
        rightTitleBar.gameObject.SetActive(true);

        int i = 0;

        for (i = 0; i < otherCargoSize; i++)
        {
            buttonListRight[i].gameObject.SetActive(true);
            buttonListRight[i].GetComponentInChildren<Text>().text 
                =  otherHold.GetCargoItems()[i];
            theirInventoryAmounts[i].text = otherHold.GetAmountInHold(otherHold.GetCargoItems()[i]).ToString();
            theirInventoryAmounts[i].gameObject.SetActive(true);
            String s = otherHold.GetCargoItems()[i];
            theirValues[i].text = otherHold.GetCargoItemValue(s).ToString();
            theirValues[i].gameObject.SetActive(true);
            buttonListRight[i].onClick.AddListener(() => { Trade2(s); });

            ColorBlock cb = buttonListRight[i].colors;
            cb.normalColor = Color.white;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListRight[i].colors = cb;
        }

        ClearPanel(buttonListLeft);

        for (i = 0; i < myCargoSize; i++)
        {
            buttonListLeft[i].gameObject.SetActive(true);
            buttonListLeft[i].GetComponentInChildren<Text>().text
                =  myHold.GetCargoItems()[i];
            myInventoryAmounts[i].text = myHold.GetAmountInHold(myHold.GetCargoItems()[i]).ToString();
            myInventoryAmounts[i].gameObject.SetActive(true);
            String s = myHold.GetCargoItems()[i];
            myValues[i].text = myHold.GetCargoItemValue(s).ToString();
            myValues[i].gameObject.SetActive(true);
            buttonListLeft[i].onClick.AddListener(()=> { Trade1(s); });

            ColorBlock cb = buttonListLeft[i].colors;
            cb.normalColor = Color.white;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListLeft[i].colors = cb;
        }
    }

    private void Trade1(String c)
    {
        int i = 0;
        for (i = 0; i < buttonElementListFrom.Count; i++)
        {
            if (!buttonElementListFrom[i].gameObject.activeInHierarchy)
            {
                buttonElementListFrom[i].gameObject.SetActive(true);
                buttonElementListFrom[i].GetComponentInChildren<Text>().text = c;

                mySelectorListDown[i].gameObject.SetActive(true);
                mySelectorListUp[i].gameObject.SetActive(true);

                myAmountSelect[i].gameObject.SetActive(true);
                myAmountSelect[i].text = "0";

                myPrices[i].gameObject.SetActive(true);
                myPrices[i].text = "0";

                int j = i;

                mySelectorListDown[i].onClick.AddListener(() => { Decrease(j, c, true); });
                mySelectorListUp[i].onClick.AddListener(() => { Increase(j, c, true); });

                break;
            }
            else if (buttonElementListFrom[i].GetComponentInChildren<Text>().text == c)
            {
                break;
            }
        }
    }

    private void Trade2(String c)
    {
        int i = 0;
        for (i = 0; i < buttonElementListTo.Count; i++)
        {
            if (!buttonElementListTo[i].gameObject.activeInHierarchy)
            {
                buttonElementListTo[i].gameObject.SetActive(true);
                buttonElementListTo[i].GetComponentInChildren<Text>().text = c;

                theirSelectorListDown[i].gameObject.SetActive(true);
                theirSelectorListUp[i].gameObject.SetActive(true);
                

                theirAmountSelect[i].gameObject.SetActive(true);
                theirAmountSelect[i].text = "0";

                theirPrices[i].gameObject.SetActive(true);
                theirPrices[i].text = "0";

                int j = i;

                theirSelectorListDown[i].onClick.AddListener(() => { Decrease(j, c, false); });
                theirSelectorListUp[i].onClick.AddListener(() => { Increase(j, c, false); });


                break;
            }
            else if (buttonElementListTo[i].GetComponentInChildren<Text>().text == c)
            {
                break;
            }
        }
    }

    private void Increase(int i, String c, bool mine)
    {
        int x = 0;
        if (mine)
        {
            x = int.Parse(myAmountSelect[i].text);
            if (x < myHold.GetAmountInHold(c))
            {
                x = x + 1;
                myAmountSelect[i].text = x.ToString();
                myPrices[i].text = (myHold.GetCargoItemValue(c)*x).ToString(); /// REPLACE
            }
        }
        else
        {
            x = int.Parse(theirAmountSelect[i].text); 
            if (x < otherHold.GetAmountInHold(c))
            {
                x=x+1;
                theirAmountSelect[i].text = x.ToString();
                theirPrices[i].text = (otherHold.GetCargoItemValue(c)*x).ToString();  /// REPLACE
            }
        }
    }
    
    private void Decrease(int i, String c, bool mine)
    {
        int x = 0;
        if (mine)
        {
            x = int.Parse(myAmountSelect[i].text);
            if (x > 0)
            {
                x=x-1;
                myAmountSelect[i].text = x.ToString();
                myPrices[i].text = (myHold.GetCargoItemValue(c) * x).ToString();   /// REPLACE
            }
        }
        else
        {
            x = int.Parse(theirAmountSelect[i].text);
            if (x > 0)
            {
                x = x - 1;
                theirAmountSelect[i].text = x.ToString();
                theirPrices[i].text = (otherHold.GetCargoItemValue(c)* x).ToString();   /// REPLACE
            }
        }
    }

    private void MakeTrade()
    {
        int i = 0;
        int j = 0;
        bool traded = false;
        int x = 0;
        int y = 0;
        for (i = 0; i < myAmountSelect.Count; i++)
        {
            for (j = 0; j < theirAmountSelect.Count; j++)
            {
                if (myAmountSelect[i].gameObject.activeInHierarchy && theirAmountSelect[j].gameObject.activeInHierarchy)
                {
                    x = int.Parse(myAmountSelect[i].text);
                    y = int.Parse(theirAmountSelect[j].text);
                    String e = buttonElementListFrom[i].GetComponentInChildren<Text>().text;
                    String f = buttonElementListTo[j].GetComponentInChildren<Text>().text;
                    if (x <= myHold.GetAmountInHold(e) && y <= otherHold.GetAmountInHold(f))
                    {
                        if (otherHold.Contains(e) && myHold.Contains(f))
                        {
                            myHold.AddToHold(e, -x);
                            otherHold.AddToHold(e, x);
                            myHold.AddToHold(f, y);
                            otherHold.AddToHold(f, -y);
                            traded = true;
                        }
                    }
                }
            }
        }
        if (traded)
        {
            ClearPanel(buttonElementListFrom);
            ClearPanel(buttonElementListTo);
            ClearPanel(theirSelectorListDown);
            ClearPanel(theirSelectorListUp);
            ClearPanel(mySelectorListDown);
            ClearPanel(mySelectorListUp);
            ClearNumberPanel(myAmountSelect);
            ClearNumberPanel(theirAmountSelect);
            ClearNumberPanel(theirPrices);
            ClearNumberPanel(myPrices);
        }

        for (i = 0; i < myInventoryAmounts.Count; i++)
        {
            String e = buttonListLeft[i].GetComponentInChildren<Text>().text;
            int a = myHold.GetAmountInHold(e);
            myInventoryAmounts[i].text = a.ToString();
        }
        for (i = 0; i < theirInventoryAmounts.Count; i++)
        {
            String e = buttonListRight[i].GetComponentInChildren<Text>().text;
            int a = otherHold.GetAmountInHold(e);
            theirInventoryAmounts[i].text = a.ToString();
        }
    }

    private void MassClear()
    {
        ClearPanel(buttonElementListFrom);
        ClearPanel(buttonElementListTo);
        ClearPanel(theirSelectorListDown);
        ClearPanel(theirSelectorListUp);
        ClearPanel(mySelectorListDown);
        ClearPanel(mySelectorListUp);

        ClearNumberPanel(theirAmountSelect);
        ClearNumberPanel(myAmountSelect);
        ClearNumberPanel(theirInventoryAmounts);
        ClearNumberPanel(myInventoryAmounts);
        ClearNumberPanel(myValues);
        ClearNumberPanel(theirValues);
    }
}





/* TODO:
 *  Create and incorporate bounty menu.
 *  Look into blocking menu during combat.
 */