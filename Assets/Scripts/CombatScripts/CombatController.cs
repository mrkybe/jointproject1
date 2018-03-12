using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;

public class CombatController : MonoBehaviour {
    public GameObject enemySpawner;
    public GameObject mainCam;
	public GameObject combatCam;
    public GameObject combatField;
    public GameObject ai_player;
    public GameObject combat_player;

    private bool flag = false;
	private int depletion = 0;
    private Move move;
    private Fire fire;
    private CameraController cc;
	private PlayerController pc;
    private LaserFire lf;
	private Rocket rk;
	private LineRenderer lr;
	private Spaceship playerSpaceship;
	private Spaceship enemySpaceship;
	private Overseer o;
    // Use this for initialization
    void Start()
    {
		o = GetComponent<Overseer> ();
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
    private void Update()
    {
		if (Input.GetButtonDown ("Y") && flag == false) {
			CombatStart ();
		} else if (Input.GetButtonDown ("Y") && flag == true) {
			CombatEnd ();
		}
    }
    ///<summary>
    /// After Every fixed amount of frames we will check if combat has initiated. For testing purposes combat can be initiated by
    /// pressing the "Y" button.
    ///</summary>
	public void CombatStart(Spaceship player, Spaceship enemy)
	{
		o.PauseOvermap ();
		flag = true;
		mainCam.SetActive (false);
		combatCam.SetActive (true);
		ai_player.SetActive(false);
		combat_player.SetActive(true);
		lr.enabled = false;
		// move.enabled = true;
		// fire.enabled = true;
		//cc.enabled = true;
		//cf.enabled = false;
		lf.enabled = false;
		rk.enabled = false;
		combatField.SetActive(true);
		combat_player.transform.position = new Vector3(combatField.transform.position.x, combatField.transform.position.y + 2f, combatField.transform.position.z);
		combatCam.transform.position = new Vector3(combatCam.transform.position.x, combatCam.transform.position.y + 20, combatCam.transform.position.z);
		enemySpawner.GetComponent<EnemySpawner>().enabled = true;
		playerSpaceship = player;
		enemySpaceship = enemy;
		pc.health = playerSpaceship.HullHealth;
	}

	public void CombatEnd()
	{
		o.UnpauseOvermap ();
		flag = false;
		mainCam.SetActive (true);
		combatCam.SetActive (false);
		ai_player.SetActive(true);
		combat_player.SetActive (false);
		combatField.SetActive (false);
		//cameraObject.transform.position = new Vector3 (cameraObject.transform.position.x, cameraObject.transform.position.z - 20, cameraObject.transform.position.z);
		enemySpawner.GetComponent<EnemySpawner> ().enabled = false;
		depletion = 100 - pc.health;
		playerSpaceship.TakeDamage (depletion, enemySpaceship);
		enemySpaceship.TakeDamage (1000, playerSpaceship);
	}
}
