using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using Assets.Scripts.Classes.Helper;
using Assets.Behavior_Designer.Runtime;
using BehaviorDesigner.Runtime;
using System.Reflection;
using Assets.Scripts.Classes.Helper.ShipInternals;

public class CombatController : MonoBehaviour {
	public GameObject [] enemySpawners;
    public GameObject AsteriodsSpawner;
    public GameObject mainCam;
	public GameObject combatCam;
    public GameObject ai_player;
    public GameObject combat_player;
	public GameObject miniMap;
	public enum COMBAT_RESULT {PLAYER_DEATH,ENEMY_DEATH,PLAYER_ESCAPE,ENEMY_ESCAPE,TESTING};
	public bool showEnemy = false;
	public bool playerCanMove = false;

    private bool flag = false;
	private int spawnerCount = 0;
	private int player_depletion = 0;
	private int enemy_depletion = 0;
    private Move move;
    private Fire fire;
    private CameraController cc;
	private PlayerController pc;
    private LaserFire lf;
	private Rocket rk;
	private LineRenderer lr;
	private Spaceship playerSpaceship;
	private Spaceship enemySpaceship;
	private Spaceship playerTester;
	private Spaceship[] enemyTesters;
	private Overseer o;
	private GameObject[] enemies;
	private GameObject leader;

	public static CombatController instance;
    // Use this for initialization
    void Start()
    {
		instance = this;
		o = GetComponent<Overseer> ();
		cc = combatCam.GetComponent<CameraController> ();
        move = combat_player.GetComponent<Move>();
        fire = combat_player.GetComponent<Fire>();
		rk = combat_player.GetComponent<Rocket> ();
        lf = combat_player.GetComponent<LaserFire>();
		lr = combat_player.GetComponent<LineRenderer> ();
		pc = combat_player.GetComponent<PlayerController> ();

    }
    ///<summary>
    /// Checks every frame if player has pressed the corresponding button that switches between fire and laser fire scripts.
    /// This is how we will handle "switching" between weapons.
    ///</summary>
	/// 
    private void Update()
    {
		playerTester = ai_player.GetComponent<Spaceship> ();
		enemyTesters = GameObject.FindObjectsOfType<Spaceship> ();
		int rando = Random.Range (0, enemyTesters.Length);
		Spaceship tempEnemy = enemyTesters [rando];
		if (Input.GetButtonDown ("Y") && flag == false) {
			CombatStart (playerTester, tempEnemy);
			flag = true;
		} else if (Input.GetButtonDown ("Y") && flag == true) {
			CombatEnd (COMBAT_RESULT.TESTING);
			flag = false;
		}
		// testing
		if (Input.GetKeyDown (KeyCode.P)) 
		{
			PauseCombat ();
		}
		//
		//CowardsWay ();
		//Victory ();
		ShowEnemy ();
    }
    ///<summary>
    /// After Every fixed amount of frames we will check if combat has initiated. For testing purposes combat can be initiated by
    /// pressing the "Y" button.
    ///</summary>
	public void CombatStart(Spaceship player, Spaceship enemy)
	{
		o.PauseOvermap ();
		o.gameState = GameState.InCombat;
		//
//		enum GameStates in overseer
//		{
//			GameOver = 0,
//			InOverMap = 1,
//			InCombat = 2,
//			UI = 3
//		}
		///
		flag = true;
		mainCam.SetActive (false);
		combatCam.SetActive (true);
		combat_player.GetComponent<Rigidbody> ().isKinematic = false;
		//ai_player.SetActive(false);
		combat_player.SetActive(true);
		lr.enabled = false;
		// move.enabled = true;
		// fire.enabled = true;
		//cc.enabled = true;
		//cf.enabled = false;
		lf.enabled = false;
		rk.enabled = false;
		//combatField.SetActive(true);
		//combat_player.transform.position = new Vector3(combatField.transform.position.x, combatField.transform.position.y + 2f, combatField.transform.position.z);
		combatCam.transform.position = new Vector3(combatCam.transform.position.x, combatCam.transform.position.y + 20, combatCam.transform.position.z);
		spawnerCount = enemySpawners.Length;
		for (int i = 0; i < spawnerCount; i++) {
			EnemySpawner spawn = enemySpawners [i].GetComponent<EnemySpawner> ();
			spawn.enabled = true;
			spawn.StartSpawn ();
		}
	    AsteroidsGeneration asteroidManager = AsteriodsSpawner.GetComponent<AsteroidsGeneration>();
	    asteroidManager.enabled = true;
        asteroidManager.SetupCombatAsteroids();
        playerSpaceship = player;
		enemySpaceship = enemy;
		SpawnLeader ();
		showEnemy = true;

		pc.health = playerSpaceship.HullHealth;
		miniMap.SetActive (true);
        Time.timeScale = 1.0f;
        o.SetBehaviorManagerTickrate(o.gameState);
    }

	public void CombatEnd(COMBAT_RESULT result)
    {
		if (leader != null) {
			AI_Enemy leaderAI = leader.GetComponent<AI_Enemy> ();
			//handle enemy health
			enemy_depletion = enemySpaceship.HullHealth - leaderAI.health;
			enemySpaceship.TakeDamage (enemy_depletion, playerSpaceship);
		}

        if(result == COMBAT_RESULT.ENEMY_DEATH)
        {
            CargoHold loot = enemySpaceship.GetCargoHold;
            CargoHold hold = playerSpaceship.GetCargoHold;

            List<string> lootItems = loot.GetCargoItems();
            foreach(string item in lootItems)
            {
                int amount = loot.GetAmountInHold(item);
                hold.Credit(item, loot, amount, true);
            }
        }

		//handle player health
		player_depletion = playerSpaceship.HullHealth - pc.health;
		playerSpaceship.TakeDamage (player_depletion, enemySpaceship);

		//RESET PLAYER
		playerCanMove = false;
		combat_player.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
		combat_player.GetComponent<Rigidbody> ().isKinematic = true;
		combat_player.transform.Rotate (new Vector3 (0, 0, 0));
		combat_player.transform.position = new Vector3 (0, 100, 0);
		combat_player.GetComponent<Rocket> ().ResetAmmo ();
		pc.SwitchWeapon (PlayerController.Weapon.M2_MG);
		//


        //Time.timeScale = 0.0f;
        enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		miniMap.SetActive (false);

		o.gameState = GameState.InOverMap;
		o.UnpauseOvermap ();
		o.UnpauseOvermap ();

		mainCam.SetActive (true);
		combatCam.transform.position = new Vector3 (combat_player.transform.position.x, combatCam.transform.position.y, combat_player.transform.position.z);
		combatCam.SetActive (false);

		//cleaning up enemies
		for (int i = 0; i < spawnerCount; i++) {
			EnemySpawner spawn = enemySpawners [i].GetComponent<EnemySpawner> ();
			spawn.Stop ();
			spawn.enabled = false;
		}
        //clean up asteroids
        AsteriodsSpawner.GetComponent<AsteroidsGeneration>().HideAsteroids();

		//clean up enemies
		foreach (GameObject enemy in enemies)
			Destroy(enemy);

        o.SetBehaviorManagerTickrate(o.gameState);
    }

	//need to fix enemies rotation
	private void SpawnLeader()
	{
		GameObject baddy = enemySpaceship.gameObject.transform.GetChild(1).gameObject;
		//int x = Random.Range (10, 30);
		//int z = Random.Range (10, 30);
		Vector3 position = new Vector3(combat_player.transform.position.x, combat_player.transform.position.y, combat_player.transform.position.z + 50f); 
		GameObject enemy = Resources.Load("Prefabs/Combat_Leader") as GameObject;
		GameObject parent = Instantiate (enemy, position, Quaternion.identity);

		parent.GetComponent<BoxCollider> ().size = new Vector3(1,1,2.5f);
		AI_Enemy leaderAI = parent.GetComponent<AI_Enemy> ();
		leaderAI.health = enemySpaceship.HullHealth;

		//creating ship model as child
		GameObject child = new GameObject();
		child.name = "ShipModel";
		child.layer = 12;
		child.transform.parent = parent.transform;
		child.transform.position = parent.transform.position;
		child.transform.Rotate (new Vector3 (-90, 180, 0));
		child.transform.localScale = new Vector3 (6, 6, 6);
		child.AddComponent<MeshCollider> ();

		Mesh shipMesh = baddy.GetComponent<MeshFilter> ().mesh;
		Material shipMat = baddy.GetComponent<MeshRenderer> ().material;

		MeshFilter meshFilter = child.AddComponent<MeshFilter> ();
		MeshRenderer meshRenderer = child.AddComponent<MeshRenderer> ();

		meshFilter.mesh = shipMesh;
		meshRenderer.material = shipMat;

		leader = parent;

	}

	public GameObject GetLeader()
	{
		if (leader != null)
		{
			return leader;
		}
		else
		{
			return null;
		}
	}

	private void ShowEnemy()
	{
		if (showEnemy) 
		{
			//Time.timeScale = 0;
			float z = leader.transform.position.z;
			Vector3 position = combatCam.transform.position;
			//combatCam.transform.position = combat_player.transform.position;
			combatCam.transform.position += new Vector3 (0, 0, .5f);
			if (combatCam.transform.position.z >= z) 
			{
				combatCam.transform.position = new Vector3(combatCam.transform.position.x, combatCam.transform.position.y, leader.transform.position.z);
			}
				//Vector3.Lerp(combat_player.transform.position, leader.transform.position, 5f * Time.deltaTime);
			StartCoroutine ("UnPause");
		}
			
	}

	private IEnumerator UnPause()
	{
		Time.timeScale = 0f;
		float pauseEndTime = Time.realtimeSinceStartup + 5;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		showEnemy = false;
		playerCanMove = true;
		Time.timeScale = 1;
	}

	public void CowardsWay()
	{
		//check distance between player and enemy leader.
		if (leader != null) 
		{
			float dist = Vector3.Distance (combat_player.transform.position, leader.transform.position);
			//if distance is to great then player escapes.
			if (dist > 300)
				CombatEnd (COMBAT_RESULT.PLAYER_ESCAPE);
		}
	}


    public GameObject getLeader()
    {
        return leader;
    }

	public void PauseCombat()
	{
		if (Time.timeScale != 0) 
		{
			Time.timeScale = 0;
			playerCanMove = false;
		} 
		else if (Time.timeScale == 0) 
		{
			Time.timeScale = 1;
			playerCanMove = true;
		}
	}
}
