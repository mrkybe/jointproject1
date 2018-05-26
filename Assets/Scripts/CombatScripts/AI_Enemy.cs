using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime;
using BehaviorDesigner;


public class AI_Enemy : MonoBehaviour {

	public float velocidadMax;

	public float xMax;
	public float zMax;
	public float xMin;
	public float zMin;
	public int health = 3;
	public int kineticDMG = 2;
	public int laserDMG = 3;
	public int rocketDMG = 4;
	public int radius = 20;

	public int speed = 10;


	private float x;
	private float z;
	private float tiempo;
	private float angulo;
	private float dist;
	private Fire f;
	private int count = 0 ;
	public AudioClip ExplosionSound;

	private AudioSource source;
	private Rigidbody rigidBody;
	private ParticleSystem ps;
	private GameObject overseerObject;
	private Overseer overseer;
	private BehaviorTree tree; 
	private CombatController combatController;
	private BehaviorTree behaviorTree;
	private GameObject player;

	void Awake(){
		player = GameObject.Find("Combat_ship_player");
		source = GetComponent <AudioSource> ();
		tree = GetComponent<BehaviorTree> ();
		combatController = GameObject.Find ("Overseer").GetComponent<CombatController> ();
		overseer = GameObject.Find ("Overseer").GetComponent<Overseer> ();
		behaviorTree = transform.GetComponent<BehaviorTree>();
		rigidBody = GetComponent<Rigidbody> ();
		if (behaviorTree)
		{
			behaviorTree.StartWhenEnabled = true;
			behaviorTree.GetVariable("PlayerTransform").SetValue(player.transform);
			behaviorTree.GetVariable ("enemy_AudioSource").SetValue (source);

		}
	}
		

	// Update is called once per frame
	void Update () {
		//KillYourself ();
	}

	void OnCollisionEnter(Collision other)
	{
		//Vector3 push = other.impulse * -1;
		//rigidBody.AddForce(push);

		if (other.gameObject.CompareTag ("CombatAsteroid") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) {
			DepleteHealth(1);
			overseer.DoExplosion (transform.position, 12, .1f);
		}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Bullet"))
		{
			//ps.Play ();
			//overseer.DoExplosion(transform.position, 12, 2);
			DepleteHealth (kineticDMG);
			Destroy (other.gameObject);
		}

		if (other.gameObject.CompareTag("Laser"))
		{
			//ps.Play ();
			//overseer.DoExplosion(transform.position, 12, 2);
			DepleteHealth (laserDMG);
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag("Rocket"))
		{
			//ps.Play ();
			//overseer.DoExplosion(transform.position, 12, 6);
			DepleteHealth (rocketDMG);
			Destroy (other.gameObject);
		}
	}

	private void KillYourself()
	{

	}
	public void DepleteHealth(int dmg)
	{
		health -= dmg;

		if (health <= 0) {
			overseer.DoExplosion(transform.position, 12, 12);
			source.PlayOneShot (ExplosionSound);
			tree.enabled = false;
			//gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;

			//gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

			//gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, -10, 0));

			if (gameObject == combatController.GetLeader ()) 
			{
				combatController.CombatEnd (CombatController.COMBAT_RESULT.ENEMY_DEATH);
			}
			else 
				Destroy (gameObject);
			//gameObject.SetActive(false);

		}
	}
		
}
