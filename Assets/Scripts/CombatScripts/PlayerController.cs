using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Mobile;
using Assets.Scripts.Classes.WorldSingleton;
using UnityEngine.UI;
using UnityEngine;


///<summary>
/// The PlayerController is repsonsible for controlling which objects are actuve during
/// combat. This class also enables and disables associated scripts with the Overworld
/// player.
///</summary>
public class PlayerController : MonoBehaviour
{
	public int health = 100;
	public Image currentHealthbar;
	public Text ratioText;

    private Fire fire;
	private LaserFire lf;
	private Rocket rk;
    // Use this for initialization
	public AudioClip shootSound;

	private AudioSource source;
	private CombatController combatController;
	private ParticleSystem particleSystem;
	private GameObject overseerObject;
	private Overseer overseer;

	void Awake(){

		source = GetComponent <AudioSource> ();
	}

    void Start()
    {
		overseerObject = GameObject.Find ("Overseer");
		overseer = overseerObject.GetComponent<Overseer> ();
        fire = GetComponent<Fire>();
        lf = GetComponent<LaserFire>();
		rk = GetComponent<Rocket> ();
		combatController = GameObject.Find ("Overseer").GetComponent<CombatController> ();
		//Debug.Log ("Health is:" + health);
		particleSystem = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
		//&& fire.enabled == true)
		if (Input.GetButtonDown ("LB") && fire.enabled == true) {
			//Debug.Log ("Switching");
			source.PlayOneShot (shootSound);
			fire.enabled = false;
			lf.enabled = true;
			rk.enabled = false;
			//fire.speed = 15f;
			//fire.ammo = fire.laser;

		} else if (Input.GetButtonDown ("LB") && lf.enabled == true) {
			//Debug.Log ("Switching");
			fire.enabled = false;
			lf.enabled = false;
			rk.enabled = true;
			source.PlayOneShot (shootSound);
			//fire.speed = 3f;
			//fire.ammo = fire.bullet;
		}
		else if (Input.GetButtonDown ("LB") && rk.enabled == true) 
		{
			//Debug.Log ("Switching");
			fire.enabled = true;
			lf.enabled = false;
			rk.enabled = false;
			source.PlayOneShot (shootSound);
		}
		Dead ();
    }

	public void Depletion(int damage)
	{
		health -= damage;
		float ratio = health/15;
		//Debug.Log ("health: " + health); 
		//Debug.Log ("Ratio: " + ratio); 
		currentHealthbar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ratioText.text = (health).ToString () + '%';
		//Debug.Log ("Health: " + health);
	}

	public void Dead()
	{
		if (health <= 0) {
			health = 0;
			combatController.CombatEnd (CombatController.COMBAT_RESULT.PLAYER_DEATH);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Contains("EnemyBullet")) {
			overseer.DoExplosion (transform.position, 12, 8);
			Depletion (1);
			Destroy (other.gameObject);
		}
	}
}
