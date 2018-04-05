using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Mobile;
using UnityEngine;


///<summary>
/// The PlayerController is repsonsible for controlling which objects are actuve during
/// combat. This class also enables and disables associated scripts with the Overworld
/// player.
///</summary>
public class PlayerController : MonoBehaviour
{
	public int health;

    private Fire fire;
	private LaserFire lf;
	private Rocket rk;
    // Use this for initialization
	public AudioClip shootSound;

	private AudioSource source;
	private CombatController combatController;
	private ParticleSystem particleSystem;


	void Awake(){

		source = GetComponent <AudioSource> ();
	}

    void Start()
    {
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
    }

	public void Depletion(int damage)
	{
		health -= damage;
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
			particleSystem.Play ();
			Depletion (1);
			Destroy (other.gameObject);
		}
	}
}
