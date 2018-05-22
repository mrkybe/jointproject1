using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.Scripts.Classes.Helper.ShipInternals;
using Assets.Scripts.Classes.Static;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine.SceneManagement;

/// <summary>
/// Press Space to open list of nearby moons and ships.
/// Click on one to open menu.
/// Choose combat, bounties, or trade.
/// If trading, click on items in inventory to place on center panel.
/// Click buttons in center panel to increase or decrease amount to be traded.
/// Click submit button to confirm trade.
/// Press Space again to close menu.
/// Press Space again to close menu.
/// </summary>
public class TradeMenuMk2 : MonoBehaviour
{
    public Button buttonPrefab;
    public Button submitButton;
    public Button elementButton;
    public Button selectorUpButton;
    public Button selectorDownButton;
    public Button bountyButton;
    public Text textPrefab;

    public Button escapeButton;
    public Button resumeCButton;
    public Button exitButton;
    public Button resumeOButton;

    private CombatController cc;

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
    private RectTransform bountyPanel;
    private RectTransform bountyDisplay;
    private RectTransform bountyRewards;
    private RectTransform walletPanel;

    private RectTransform chatPanel;
    private RectTransform chatName;
    private RectTransform chatText;

    private RectTransform leftTitleBar;
    private RectTransform rightTitleBar;
    private RectTransform leftNameBar;
    private RectTransform rightNameBar;

    private RectTransform myPricesPanel;
    private RectTransform theirPricesPanel;
    private RectTransform myAmountPanel;
    private RectTransform theirAmountPanel;

    private RectTransform escapePanel;
    private Text escapeStat;

    private RectTransform menuPanel;

    private Vector2 on;
    private Vector2 leftOffPosition;
    private Vector2 rightOffPosition;

    private bool isRightOff;
    private bool isLeftOff;
    private bool isEscapeOff;
    private bool isMenuOff;

    private List<Button> buttonListLeft;
    private List<Button> buttonListRight;
    private List<Button> buttonElementListFrom;
    private List<Button> buttonElementListTo;
    private List<Button> mySelectorListUp;
    private List<Button> mySelectorListDown;
    private List<Button> theirSelectorListUp;
    private List<Button> theirSelectorListDown;
    private List<Button> bountyList;

    private List<Text> myAmountSelect;
    private List<Text> theirAmountSelect;
    private List<Text> myInventoryAmounts;
    private List<Text> theirInventoryAmounts;
    private List<Text> myValues;
    private List<Text> theirValues;
    private List<Text> myPrices;
    private List<Text> theirPrices;
    private List<Text> bountyRewardsList;
    private List<Text> walletList;

    private List<Text> leftName;
    private List<Text> rightName;
    private List<Text> chatNameText;

    private List<Text> chat;

    private Spaceship otherShip;
    private Spaceship playerShip;
    private Planet planet;
    private CargoHold myHold;
    private CargoHold otherHold;

    private Overseer o;

    // Use this for initialization
    void Start()
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
        //bountyPanel = GameObject.Find("Bounty Panel").GetComponent<RectTransform>();
        //bountyDisplay = GameObject.Find("Bounty Display List").GetComponent<RectTransform>();
        //bountyRewards = GameObject.Find("Bounty Rewards").GetComponent<RectTransform>();
        walletPanel = GameObject.Find("Wallet Panel").GetComponent<RectTransform>();
        leftNameBar = GameObject.Find("Left Name Panel").GetComponent<RectTransform>();
        rightNameBar = GameObject.Find("Right Name Panel").GetComponent<RectTransform>();

        chatPanel = GameObject.Find("Chat Panel").GetComponent<RectTransform>();
        chatName = GameObject.Find("Chat Name Panel").GetComponent<RectTransform>();
        chatText = GameObject.Find("Chat Text Panel").GetComponent<RectTransform>();

        escapePanel = GameObject.Find("Escape Panel").GetComponent<RectTransform>();
        escapeStat = GameObject.Find("Escape Status Text").GetComponent<Text>();

        menuPanel = GameObject.Find("Menu Panel").GetComponent<RectTransform>();


        playerShip = GameObject.Find("PlayerShip").GetComponent<Spaceship>();

        o = GameObject.Find("Overseer").GetComponent<Overseer>();

        cc = GameObject.Find("Overseer").GetComponent<CombatController>();

        centerPanel.gameObject.SetActive(false);
        //bountyPanel.gameObject.SetActive(false);

        on = new Vector2(0, 0);
        leftOffPosition = new Vector2(-675, 0);
        rightOffPosition = new Vector2(675, 0);

        isLeftOff = true;
        isRightOff = true;
        isEscapeOff = true;
        isMenuOff = true;

        buttonListLeft = new List<Button>();
        buttonListRight = new List<Button>();
        buttonElementListFrom = new List<Button>();
        buttonElementListTo = new List<Button>();
        mySelectorListUp = new List<Button>();
        mySelectorListDown = new List<Button>();
        theirSelectorListDown = new List<Button>();
        theirSelectorListUp = new List<Button>();
        bountyList = new List<Button>();
        walletList = new List<Text>();
        leftName = new List<Text>();
        rightName = new List<Text>();

        myAmountSelect = new List<Text>();
        theirAmountSelect = new List<Text>();
        myInventoryAmounts = new List<Text>();
        theirInventoryAmounts = new List<Text>();
        myValues = new List<Text>();
        theirValues = new List <Text>();
        myPrices = new List<Text>();
        theirPrices = new List<Text>();
        bountyRewardsList = new List<Text>();

        chat = new List<Text>();
        chatNameText = new List<Text>();

        otherShip = new Spaceship();
        planet = new Planet();
        otherHold = new CargoHold(GetComponent<MonoBehaviour>(), 0);
        myHold = playerShip.GetCargoHold;


        PopulatePanel(buttonListLeft, buttonPrefab, leftButtonPanel, 15);
        PopulatePanel(buttonListRight, buttonPrefab, rightButtonPanel, 15);
        PopulatePanel(buttonElementListFrom, elementButton, myElementPanel, 10);
        PopulatePanel(buttonElementListTo, elementButton, theirElementPanel, 10);
        PopulatePanel(mySelectorListDown, selectorUpButton, mySelectorDown, 10);
        PopulatePanel(mySelectorListUp, selectorUpButton, mySelectorUp, 10);
        PopulatePanel(theirSelectorListDown, selectorDownButton, theirSelectorDown, 10);
        PopulatePanel(theirSelectorListUp, selectorDownButton, theirSelectorUp, 10);
        PopulatePanel(bountyList, buttonPrefab, bountyDisplay, 1);

        PopulateNumberPanel(myAmountSelect, textPrefab, myAmountPanel, 10);
        PopulateNumberPanel(theirAmountSelect, textPrefab, theirAmountPanel, 10);
        PopulateNumberPanel(myInventoryAmounts, textPrefab, myInventoryPanel, 15);
        PopulateNumberPanel(theirInventoryAmounts, textPrefab, theirInventoryPanel, 15);
        PopulateNumberPanel(myValues, textPrefab, leftValuesPanel, 15);
        PopulateNumberPanel(theirValues, textPrefab, rightValuesPanel, 15);
        PopulateNumberPanel(myPrices, textPrefab, myPricesPanel, 10);
        PopulateNumberPanel(theirPrices, textPrefab, theirPricesPanel, 10);
        PopulateNumberPanel(bountyRewardsList, textPrefab, bountyRewards, 1);
        PopulateNumberPanel(walletList, textPrefab, walletPanel, 1);
        PopulateNumberPanel(leftName, textPrefab, leftNameBar, 1);
        PopulateNumberPanel(rightName, textPrefab, rightNameBar, 1);

        PopulateNumberPanel(chatNameText, textPrefab, chatName, 1);
        PopulateNumberPanel(chat, textPrefab, chatText, 15);


        leftName[0].text = playerShip.ShipName;

        submitButton.onClick.AddListener(SubmitTrade);
        bountyButton.onClick.AddListener(ConfirmBounty);

        walletList[0].gameObject.SetActive(false);
       
        rightName[0].gameObject.SetActive(false);
        leftName[0].gameObject.SetActive(false);

        escapePanel.gameObject.SetActive(false);
        escapeButton.onClick.AddListener(EscapeCombat);
        resumeCButton.onClick.AddListener(ResumeCombat);
        escapeStat.text = "";

        menuPanel.gameObject.SetActive(false);
        exitButton.onClick.AddListener(ExitGame);
        resumeOButton.onClick.AddListener(ResumeOvermap);

        MassClear();


        //<FOR TESTING:> 
        myHold.AddHoldType("Dirt");
        myHold.AddToHold("Dirt", 100);
        //</FOR TESTING>
    }


    void Update()
    {
        if (o.gameState == GameState.InOverMap || o.gameState == GameState.UI)
        {
            walletList[0].text = playerShip.Pilot.Money.ToString();

            if (Input.GetKeyDown(KeyCode.Space))                            //  Remember to change to Gamepad controls.
            {       
                if (isLeftOff && isRightOff)
                {
                    MassClear();
                    ShowAgentsInRange();
                    centerPanel.gameObject.SetActive(false);
                    //bountyPanel.gameObject.SetActive(false);
                    foreach (RectTransform child in leftTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    foreach (RectTransform child in rightTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    walletList[0].gameObject.SetActive(false);
                    leftName[0].gameObject.SetActive(false);
                    ClearNumberPanel(leftName);
                    rightName[0].gameObject.SetActive(false);
                    ClearNumberPanel(rightName);
                    ClearNumberPanel(chat);
                    chatPanel.anchoredPosition = rightOffPosition;
                    escapePanel.gameObject.SetActive(false);
                    isEscapeOff = true;
                }
                else if (!isLeftOff && isRightOff)
                {
                    Overseer.Main.UnpauseOvermap();
                    o.gameState = GameState.InOverMap;
                    leftPanel.anchoredPosition = leftOffPosition;
                    isLeftOff = true;
                    ClearPanel(buttonListLeft);
                    ClearNumberPanel(myInventoryAmounts);
                    centerPanel.gameObject.SetActive(false);
                    //bountyPanel.gameObject.SetActive(false);
                    foreach (RectTransform child in leftTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    foreach (RectTransform child in rightTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    walletList[0].gameObject.SetActive(false);
                    ClearNumberPanel(chat);
                    chatPanel.anchoredPosition = rightOffPosition;
                    escapePanel.gameObject.SetActive(false);
                    isEscapeOff = true;
                }
                else if (!isLeftOff && !isRightOff)
                {
                    rightPanel.anchoredPosition = rightOffPosition;
                    isRightOff = true;
                    ShowAgentsInRange();
                    centerPanel.gameObject.SetActive(false);
                    //bountyPanel.gameObject.SetActive(false);
                    foreach (RectTransform child in leftTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    foreach (RectTransform child in rightTitleBar.GetComponentInChildren<RectTransform>())
                    {
                        child.gameObject.SetActive(false);
                    }
                    walletList[0].gameObject.SetActive(false);

                    chatPanel.anchoredPosition = rightOffPosition;
                    escapePanel.gameObject.SetActive(false);
                    isEscapeOff = true;
                    MassClear();
                }
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                if (isLeftOff)
                {
                    showMyInventory();
                }
                else
                {
                    leftPanel.anchoredPosition = leftOffPosition;
                    isLeftOff = true;
                    o.UnpauseOvermap();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowEscapeMenu();
            }
        }
        else if (o.gameState == GameState.InCombat)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                
                if(isEscapeOff)
                {
                    cc.PauseCombat();
                    escapePanel.gameObject.SetActive(true);
                    if (cc.getLeader() != null)
                    {
                        float dist = Vector3.Distance(cc.combat_player.transform.position, cc.getLeader().transform.position);
                        //if distance is to great then player escapes.
                        if (dist > 300)
                            escapeStat.text = "Far Enough";
                        else
                            escapeStat.text = "Too Close";
                    }
                }
                else
                {
                    cc.PauseCombat();
                    escapePanel.gameObject.SetActive(false);
                }
                isEscapeOff = !isEscapeOff;
            }
        }
    }

    private void ShowEscapeMenu()
    {
        if (isMenuOff)
        {
            Overseer.Main.PauseOvermap();
            menuPanel.gameObject.SetActive(true);
            isMenuOff = false;
        }
        else
        {
            Overseer.Main.UnpauseOvermap();
            menuPanel.gameObject.SetActive(false);
            isMenuOff = true;
        }
    }

    private void ResumeOvermap()
    {
        ShowEscapeMenu();
    }

    private void ExitGame()
    {
        SceneManager.LoadScene("Menu_Scene");
    }

    private void EscapeCombat()
    {
        if (cc.getLeader() != null)
        {
            float dist = Vector3.Distance(cc.combat_player.transform.position, cc.getLeader().transform.position);
            //if distance is to great then player escapes.
            if (dist > 300)
                cc.CombatEnd(CombatController.COMBAT_RESULT.PLAYER_ESCAPE);
        }
        escapePanel.gameObject.SetActive(false);
        cc.PauseCombat();
        isEscapeOff = true;
    }

    private void ResumeCombat()
    {
        cc.PauseCombat();
        escapePanel.gameObject.SetActive(false);
        isEscapeOff = true;
    }


    private void showMyInventory()
    {
        o.gameState = GameState.UI;
        leftPanel.anchoredPosition = on;
        isLeftOff = false;
        walletPanel.gameObject.SetActive(true);
        walletList[0].gameObject.SetActive(true);
        leftName[0].text = playerShip.ShipName;
        leftName[0].gameObject.SetActive(true);
        foreach (RectTransform child in leftTitleBar.GetComponentInChildren<RectTransform>())
        {
            child.gameObject.SetActive(true);
        }

        if (!o.IsOvermapPaused())
        {
            Overseer.Main.PauseOvermap();
        }

        int i = 0;
        
        for (i=0; i < myHold.GetCargoItems().Count; i++)
        {
            buttonListLeft[i].gameObject.SetActive(true);
            buttonListLeft[i].GetComponentInChildren<Text>().text
                = myHold.GetCargoItems()[i];
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
        isLeftOff = false;

        List<Spaceship> shipsInRange = playerShip.GetInInteractionRange<Spaceship>();
        List<Planet> planetsInRange = playerShip.GetInInteractionRange<Planet>();
        Faction myFaction = playerShip.Pilot.Faction;

        int i = 0;

        if (shipsInRange.Count + planetsInRange.Count <= buttonListLeft.Count)       // THIS WILL PROBABLY NEVER CAUSE A PROBLEM, BUT IF IT DOES, BLAME MARK WAHLBERG.
        {
            for (i = 0; i < planetsInRange.Count; i++)
            {
                String t = planetsInRange[i].MyName;
                if (t.Length > 15)
                {
                    t = t.Remove(15) + "...";
                }
                buttonListLeft[i].GetComponentInChildren<Text>().text = t;
                buttonListLeft[i].gameObject.SetActive(true);
                String s = planetsInRange[i].name;
                buttonListLeft[i].onClick.AddListener(() => {SelectPlanet(s); });

                ColorBlock cb = buttonListLeft[i].colors;
                cb.normalColor = planetsInRange[i].Faction.ColorPrimary;
                cb.disabledColor = cb.normalColor * 0.5f;
                buttonListLeft[i].colors = cb;
            }
            int j = 0;
            for (j = 0; j < shipsInRange.Count; j++)
            {

                String t = shipsInRange[j].ShipName;
                if (t.Length > 15)
                {
                    t = t.Remove(15) + "...";
                }
                buttonListLeft[j + i].GetComponentInChildren<Text>().text = t;

                buttonListLeft[j + i].gameObject.SetActive(true);
                String s = shipsInRange[j].name;
                buttonListLeft[j + i].onClick.AddListener(() => { SelectShip(s); });

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

    private void SelectShip(string s)
    {
        ClearPanel(buttonListLeft);
        ClearPanel(buttonListRight);
        //leftTitleBar.gameObject.SetActive(false);

        buttonListLeft[0].gameObject.SetActive(true);
        buttonListLeft[1].gameObject.SetActive(true);

        buttonListLeft[0].GetComponentInChildren<Text>().text = "Trade";
        buttonListLeft[1].GetComponentInChildren<Text>().text = "Combat";

        buttonListLeft[0].onClick.AddListener(() => { OpenInventoryShip(s); });

        buttonListLeft[1].onClick.AddListener(() => { StartCombat(s); });


        if (GameObject.Find(s).GetComponent<Spaceship>().Pilot.Faction.HostileWith(playerShip.Pilot.Faction))
        {
            buttonListLeft[0].interactable = false;
        }


        int i = 0;

        for (i = 0; i < buttonListLeft.Count; i++)
        {
            ColorBlock cb = buttonListLeft[i].colors;
            cb.normalColor = GameObject.Find(s).GetComponent<Spaceship>().Pilot.Faction.ColorPrimary;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListLeft[i].colors = cb;
        }

        ShowChatMenu(s);

    }

    private void ShowChatMenu(string s)
    {
        chatPanel.anchoredPosition = on;
        otherShip = GameObject.Find(s).GetComponent<Spaceship>();
        BlackBox bb = otherShip.BlackBox;

        leftName[0].text = playerShip.ShipName;
        leftName[0].gameObject.SetActive(true);

        chatNameText[0].gameObject.SetActive(true);
        chatNameText[0].text = otherShip.ShipName;

        int i = 0;

        for (i = 0; i < chat.Count; i++)
        {
            chat[i].GetComponent<RectTransform>().sizeDelta = new Vector2(1200,100);
            chat[i].text = bb.Read().ToString();
            chat[i].gameObject.SetActive(true);

            chat[i].horizontalOverflow = HorizontalWrapMode.Wrap;
            chat[i].alignment = TextAnchor.UpperLeft;
        }
    }

    private void StartCombat(string s)
    {

        otherShip = GameObject.Find(s).GetComponent<Spaceship>();

        Overseer.Main.ResolveShipCombat(playerShip, otherShip);

        leftPanel.anchoredPosition = leftOffPosition;
        chatPanel.anchoredPosition = rightOffPosition;
        rightPanel.anchoredPosition = rightOffPosition;

        centerPanel.gameObject.SetActive(false);
        //bountyPanel.gameObject.SetActive(false);

        isLeftOff = true;
        isRightOff = true;
        
        MassClear();
    }

    private void SelectPlanet(string p)
    {
        ClearPanel(buttonListLeft);
        ClearPanel(buttonListRight);
        //leftTitleBar.gameObject.SetActive(false);

        planet = GameObject.Find(p).GetComponent<Planet>();

        buttonListLeft[0].gameObject.SetActive(true);
        buttonListLeft[1].gameObject.SetActive(true);
        buttonListLeft[2].gameObject.SetActive(true);

        buttonListLeft[0].GetComponentInChildren<Text>().text = "Trade";
        buttonListLeft[1].GetComponentInChildren<Text>().text = "Bounties";
        buttonListLeft[2].GetComponentInChildren<Text>().text = "View Industry";

        buttonListLeft[0].onClick.AddListener(() => { OpenInventoryPlanet(p); });

        buttonListLeft[1].onClick.AddListener(() => { ShowBounties(p); });
        buttonListLeft[2].onClick.AddListener(() => { ShowIndustry(p); });

        if (GameObject.Find(p).GetComponent<Planet>().Faction.HostileWith(playerShip.Pilot.Faction))
        {
            //int k = 0;
            for (int k = 0; k < buttonListLeft.Count; k++)
            {
                buttonListLeft[k].interactable = false;
            }
        }


        int i = 0;

        for (i = 0; i < buttonListLeft.Count; i++)
        {
            ColorBlock cb = buttonListLeft[i].colors;
            cb.normalColor = planet.Faction.ColorPrimary;
            cb.disabledColor = cb.normalColor * 0.5f;
            buttonListLeft[i].colors = cb;
        }

    }

    private void ShowIndustry(string p)
    {
        // Planet has a Faction, Faction has a list of bounties.
        planet = GameObject.Find(p).GetComponent<Planet>();

        List<Building> buildings = planet.Buildings();

        rightName[0].gameObject.SetActive(true);
        rightName[0].text = planet.MyName;

        rightPanel.anchoredPosition = on;
        isRightOff = false;
        chatPanel.anchoredPosition = rightOffPosition;
        //bountyPanel.gameObject.SetActive(true);
        
        int reps = Mathf.Min(buttonListRight.Count, buildings.Count);
        for (int i = 0; i < reps; i++)
        {
            String s = buildings[i].Name;
            buttonListRight[i].GetComponentInChildren<Text>().text = s;
            buttonListRight[i].gameObject.SetActive(true);
        }
    }

    private void ShowBounties(string p)
    {
        // Planet has a Faction, Faction has a list of bounties.
        planet = GameObject.Find(p).GetComponent<Planet>();

        ClearPanel(buttonListRight);
        List<Bounty> bounties = planet.Faction.BountyBoard;

        rightName[0].gameObject.SetActive(true);
        rightName[0].text = planet.MyName;

        rightPanel.anchoredPosition = on;
        isRightOff = false;
        chatPanel.anchoredPosition = rightOffPosition;
        //bountyPanel.gameObject.SetActive(true);


        int i = 0;
        if (bounties.Count > 0 && bounties.Count <= buttonListRight.Count)
        {
            for (i = 0; i < bounties.Count; i++)
            {
                String s = bounties[i].Target.name;
                if (s.Length > 15)
                {
                    s = s.Remove(15) + "...";
                }
                buttonListRight[i].GetComponentInChildren<Text>().text = s;
                buttonListRight[i].gameObject.SetActive(true);
                String r = bounties[i].Target.name;
                Faction f = planet.Faction;
                buttonListRight[i].onClick.AddListener(() => { DisplayBounty(r, f); });

                theirValues[i].text = bounties[i].Value.ToString();
                theirValues[i].gameObject.SetActive(true);

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
                //buttonListRight[i].onClick.AddListener(() => { DisplayBounty(s, f); });

                theirValues[i].text = bounties[i].Value.ToString();
                theirValues[i].gameObject.SetActive(true);

            }
        }
    }

    private void DisplayBounty(string s, Faction f)
    {
        bountyList[0].gameObject.SetActive(true);
        bountyList[0].GetComponentInChildren<Text>().text = s;
        bountyRewardsList[0].gameObject.SetActive(true);
        int i = 0;
        for (i=0; i<f.BountyBoard.Count; i++)
        {
            if (s.Equals(f.BountyBoard[i].Target.name))
            {
                bountyRewardsList[0].text = f.BountyBoard[i].Value.ToString();
            }
        }
    }

    private void OpenInventoryShip(String otherName)
    {
        ClearPanel(buttonListRight);
        centerPanel.gameObject.SetActive(true);
        rightPanel.anchoredPosition = on;
        isRightOff = false;
        chatPanel.anchoredPosition = rightOffPosition;

        otherShip = GameObject.Find(otherName).GetComponent<Spaceship>();

        walletPanel.gameObject.SetActive(true);
        walletList[0].gameObject.SetActive(true);
        leftName[0].text = playerShip.ShipName;
        leftName[0].gameObject.SetActive(true);

        rightName[0].gameObject.SetActive(true);
        rightName[0].text = otherShip.ShipName;

        

        otherHold = otherShip.GetCargoHold;

        int otherCargoSize = otherHold.GetCargoItems().Count;

        int myCargoSize = myHold.GetCargoItems().Count;

        leftTitleBar.gameObject.SetActive(true);
        rightTitleBar.gameObject.SetActive(true);

        walletPanel.gameObject.SetActive(true);

        walletList[0].gameObject.SetActive(true);

        int i = 0;

        for (i = 0; i < otherCargoSize; i++)
        {
            buttonListRight[i].gameObject.SetActive(true);
            buttonListRight[i].GetComponentInChildren<Text>().text
                = otherHold.GetCargoItems()[i];
            theirInventoryAmounts[i].text = otherHold.GetAmountInHold(otherHold.GetCargoItems()[i]).ToString();
            theirInventoryAmounts[i].gameObject.SetActive(true);
            String s = otherHold.GetCargoItems()[i];
            theirValues[i].text = otherHold.GetCargoItemUnitCost(s).ToString();
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
            myValues[i].text = myHold.GetCargoItemUnitCost(s).ToString();
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
        ClearPanel(buttonListRight);
        centerPanel.gameObject.SetActive(true);
        rightPanel.anchoredPosition = on;
        isRightOff = false;
        chatPanel.anchoredPosition = rightOffPosition;

        walletPanel.gameObject.SetActive(true);
        walletList[0].gameObject.SetActive(true);
        leftName[0].text = playerShip.ShipName;
        leftName[0].gameObject.SetActive(true);

        rightName[0].gameObject.SetActive(true);
        rightName[0].text = GameObject.Find(otherName).GetComponent<Planet>().MyName;

        planet = GameObject.Find(otherName).GetComponent<Planet>();

        otherHold = planet.GetCargoHold;

        int otherCargoSize = otherHold.GetCargoItems().Count;

        int myCargoSize = myHold.GetCargoItems().Count;

        leftTitleBar.gameObject.SetActive(true);
        rightTitleBar.gameObject.SetActive(true);

        walletPanel.gameObject.SetActive(true);
        walletList[0].gameObject.SetActive(true);

        int i = 0;

        for (i = 0; i < otherCargoSize; i++)
        {
            buttonListRight[i].gameObject.SetActive(true);
            buttonListRight[i].GetComponentInChildren<Text>().text 
                =  otherHold.GetCargoItems()[i];
            theirInventoryAmounts[i].text = otherHold.GetAmountInHold(otherHold.GetCargoItems()[i]).ToString();
            theirInventoryAmounts[i].gameObject.SetActive(true);
            String s = otherHold.GetCargoItems()[i];
            theirValues[i].text = otherHold.GetCargoItemUnitCost(s).ToString();
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
            myValues[i].text = otherHold.GetCargoItemUnitCost(s).ToString();
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
                myPrices[i].text = (otherHold.GetCargoItemUnitCost(c)*x).ToString(); /// REPLACE
            }
        }
        else
        {
            x = int.Parse(theirAmountSelect[i].text); 
            if (x < otherHold.GetAmountInHold(c))
            {
                x=x+1;
                theirAmountSelect[i].text = x.ToString();
                theirPrices[i].text = (otherHold.GetCargoItemUnitCost(c)*x).ToString();  /// REPLACE
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
                myPrices[i].text = (otherHold.GetCargoItemUnitCost(c) * x).ToString();   /// REPLACE
            }
        }
        else
        {
            x = int.Parse(theirAmountSelect[i].text);
            if (x > 0)
            {
                x = x - 1;
                theirAmountSelect[i].text = x.ToString();
                theirPrices[i].text = (otherHold.GetCargoItemUnitCost(c)* x).ToString();   /// REPLACE
            }
        }
    }

    protected struct TradeTotal
    {
        public int Value;
        public int Volume;
    }

    // TODO: FIX FABLE BUG
    private TradeTotal CalculatePlayerGoodsSellValue()
    {
        TradeTotal trade = new TradeTotal();
        for (int i = 0; i < myAmountSelect.Count; i++)
        {
            if (myAmountSelect[i].gameObject.activeInHierarchy)
            {
                int myAmount = int.Parse(myAmountSelect[i].text);
                String myResource = buttonElementListFrom[i].GetComponentInChildren<Text>().text;
                if (myAmount <= myHold.GetAmountInHold(myResource))
                {
                    trade.Value += otherHold.GetCargoItemUnitCost(myResource) * myAmount;
                    trade.Volume += myHold.CargoItems.First(x => x.Name == myResource).Size * myAmount;
                }
                else // You can't try to buy more than there is
                {
                    return new TradeTotal(){Value = -1, Volume = -1};
                }
            }
        }
        Debug.Log("Player Goods Sell Value: " + trade.Value);
        return trade;
    }

    // TODO: FIX FABLE BUG
    private TradeTotal CalculatePlayerGoodsBuyingCost()
    {
        TradeTotal trade = new TradeTotal();
        for (int j = 0; j < theirAmountSelect.Count; j++)
        {
            if (theirAmountSelect[j].gameObject.activeInHierarchy)
            {
                int theirAmount = int.Parse(theirAmountSelect[j].text);
                String theirResource = buttonElementListTo[j].GetComponentInChildren<Text>().text;
                if (theirAmount <= otherHold.GetAmountInHold(theirResource))
                {
                    trade.Value += otherHold.GetCargoItemUnitCost(theirResource) * theirAmount;
                    trade.Volume += otherHold.CargoItems.First(x => x.Name == theirResource).Size * theirAmount;
                }
                else
                {
                    return new TradeTotal() { Value = -1, Volume = -1 };
                }
            }
        }
        Debug.Log("Player Buying Cost: " + trade.Value);
        return trade;
    }

    private void SubmitTrade()
    {
        Debug.Log("Trying to trade!");
        TryTrade();
    }

    private void ConfirmBounty()
    {
        ClearPanel(bountyList);
        ClearNumberPanel(bountyRewardsList);
    }

    private bool TryTrade()
    {
        TradeTotal sellTrade = CalculatePlayerGoodsSellValue(); // Value of goods being sold by the player
        TradeTotal buyTrade = CalculatePlayerGoodsBuyingCost(); // Value of goods being bought by the player
        if (sellTrade.Value == -1 || buyTrade.Value == -1)
        {
            Debug.Log("Trade Failed! Tried to buy/sell more than there is.");
            return false; // Tried to buy/sell more than there is
        }
        // Check that the player has enough room to carry the goods after removing theirs
        // Check that the reciever has enough room to carry the goods after removing theirs
        int playerSpace = myHold.GetRemainingSpace() - sellTrade.Volume;
        int recieverSpace = otherHold.GetRemainingSpace() - buyTrade.Volume;
        if (playerSpace < buyTrade.Volume || recieverSpace < sellTrade.Volume)
        {
            Debug.Log("One party has insufficient space to complete the trade.");
            return false;
        }

        int costToPlayer = buyTrade.Value - sellTrade.Value;
        if (costToPlayer > 0)
        {
            Debug.Log(costToPlayer + " > 0, trying to charge player's funds.");
            // Check player has enough money to cover the transaction
            if (playerShip.Pilot.TryChargeMoney(costToPlayer))
            {
                Debug.Log("Charged player " + costToPlayer);
                // Give them the goods, take their sold goods
                MakeTrade();
            }
        }
        else // costToPlayer <= 0
        {
            Debug.Log(costToPlayer + " < 0, giving player money.");
            // Credit their $
            playerShip.Pilot.GiveMoney(-costToPlayer);
            // Give them the goods, take their sold goods
            MakeTrade();
            return true;
        }
        return false;
    }

    private void MakeTrade()
    {
        bool traded = false;

        for (int i = 0; i < myAmountSelect.Count; i++)
        {
            if (myAmountSelect[i].gameObject.activeInHierarchy)
            {
                int x = int.Parse(myAmountSelect[i].text);
                String e = buttonElementListFrom[i].GetComponentInChildren<Text>().text;
                if (x <= myHold.GetAmountInHold(e))
                {
                    otherHold.Credit(e, myHold, x, true);
                    traded = true;
                }
            }
        }

        for (int j = 0; j < theirAmountSelect.Count; j++)
        {
            if (theirAmountSelect[j].gameObject.activeInHierarchy)
            {
                int y = int.Parse(theirAmountSelect[j].text);
                String f = buttonElementListTo[j].GetComponentInChildren<Text>().text;
                if (y <= otherHold.GetAmountInHold(f))
                {
                    myHold.Credit(f, otherHold, y, true);
                    traded = true;
                }
            }
        }
        if (traded)
        {
            MassClear();

            walletList[0].gameObject.SetActive(true);

            for (int i = 0; i < myHold.GetCargoItems().Count; i++)
            {
                buttonListLeft[i].gameObject.SetActive(true);
                buttonListLeft[i].GetComponentInChildren<Text>().text
                    = myHold.GetCargoItems()[i];
                myInventoryAmounts[i].text = myHold.GetAmountInHold(myHold.GetCargoItems()[i]).ToString();
                myInventoryAmounts[i].gameObject.SetActive(true);
                String s = myHold.GetCargoItems()[i];
                myValues[i].text = myHold.GetCargoItemUnitCost(s).ToString();
                myValues[i].gameObject.SetActive(true);
                buttonListLeft[i].onClick.AddListener(() => { Trade1(s); });

                ColorBlock cb = buttonListLeft[i].colors;
                cb.normalColor = Color.white;
                cb.disabledColor = cb.normalColor * 0.5f;
                buttonListLeft[i].colors = cb;
            }

            for (int i = 0; i < otherHold.GetCargoItems().Count; i++)
            {
                buttonListRight[i].gameObject.SetActive(true);
                buttonListRight[i].GetComponentInChildren<Text>().text
                    = otherHold.GetCargoItems()[i];
                theirInventoryAmounts[i].text = otherHold.GetAmountInHold(otherHold.GetCargoItems()[i]).ToString();
                theirInventoryAmounts[i].gameObject.SetActive(true);
                String s = otherHold.GetCargoItems()[i];
                theirValues[i].text = otherHold.GetCargoItemUnitCost(s).ToString();
                theirValues[i].gameObject.SetActive(true);
                buttonListRight[i].onClick.AddListener(() => { Trade2(s); });

                ColorBlock cb = buttonListRight[i].colors;
                cb.normalColor = Color.white;
                cb.disabledColor = cb.normalColor * 0.5f;
                buttonListRight[i].colors = cb;
            }

        }

        for (int i = 0; i < myInventoryAmounts.Count; i++)
        {
            String e = buttonListLeft[i].GetComponentInChildren<Text>().text;
            int a = myHold.GetAmountInHold(e);
            myInventoryAmounts[i].text = a.ToString();
        }
        for (int i = 0; i < theirInventoryAmounts.Count; i++)
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
        ClearPanel(bountyList);
        
        ClearNumberPanel(theirAmountSelect);
        ClearNumberPanel(myAmountSelect);
        ClearNumberPanel(theirInventoryAmounts);
        ClearNumberPanel(myInventoryAmounts);
        ClearNumberPanel(myValues);
        ClearNumberPanel(theirValues);
        ClearNumberPanel(myPrices);
        ClearNumberPanel(theirPrices);
        //ClearNumberPanel(walletList);
        ClearNumberPanel(bountyRewardsList);
        ClearNumberPanel(chat);
    }
}





/* TODO:
 *  
 */