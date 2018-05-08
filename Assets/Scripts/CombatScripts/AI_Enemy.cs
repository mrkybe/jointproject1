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

	void Awake(){
		source = GetComponent <AudioSource> ();
		tree = GetComponent<BehaviorTree> ();
		combatController = GameObject.Find ("Overseer").GetComponent<CombatController> ();
	}



	public GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		rigidBody = GetComponent<Rigidbody> ();
		overseerObject = GameObject.Find ("Overseer");
		overseer = overseerObject.GetComponent<Overseer> ();
		x = Random.Range(-velocidadMax, velocidadMax);
		z = Random.Range(-velocidadMax, velocidadMax);
		angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
		transform.localRotation = Quaternion.Euler( 0, angulo, 0);
		//dist = Vector3.Distance (Player.transform.position, transform.position);
		f = GetComponent<Fire> ();

		//Debug.DrawLine (transform.position, transform.forward);

		//ps = GetComponent<ParticleSystem> ();
		//ps.Stop ();

	}

	// Update is called once per frame
	void Update () {
		KillYourself ();
//		if (count == 0) {
//			
//			Player = GameObject.FindGameObjectWithTag("Player");
//			count++;
//		}
		//Debug.Log (Player.transform.position);
////		dist = Vector3.Distance (Player.transform.position, gameObject.transform.position);
//		float step = speed * Time.deltaTime;
			
			//gameObject.transform.LookAt (Player.transform);
//		  if (f)
//           {
//                f.enemyFire();
//           }
//		source.PlayOneShot (shootSound);
			//gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, Player.transform.position, step);


//			xMax = Player.transform.position.x + xMax;
//
//			zMax = Player.transform.position.z + xMax;
//			xMin = Player.transform.position.x - xMin;
//			zMin = Player.transform.position.x - zMin;
//			tiempo += Time.deltaTime;
//
//			if (transform.localPosition.x > xMax) {
//				x = Random.Range (-velocidadMax, 0.0f);
//				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
//				transform.localRotation = Quaternion.Euler (0, angulo, 0);
//				tiempo = 0.0f; 
//			}
//			if (transform.localPosition.x < xMin) {
//				x = Random.Range (0.0f, velocidadMax);
//				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
//				transform.localRotation = Quaternion.Euler (0, angulo, 0); 
//				tiempo = 0.0f; 
//			}
//			if (transform.localPosition.z > zMax) {
//				z = Random.Range (-velocidadMax, 0.0f);
//				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
//				transform.localRotation = Quaternion.Euler (0, angulo, 0); 
//				tiempo = 0.0f; 
//			}
//			if (transform.localPosition.z < zMin) {
//				z = Random.Range (0.0f, velocidadMax);
//				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
//				transform.localRotation = Quaternion.Euler (0, angulo, 0);
//				tiempo = 0.0f; 
//			}
//
//
//			if (tiempo > 1.0f) {
//				x = Random.Range (-velocidadMax, velocidadMax);
//				z = Random.Range (-velocidadMax, velocidadMax);
//				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
//				transform.localRotation = Quaternion.Euler (0, angulo, 0);
//				tiempo = 0.0f;
//			}
//
//			transform.localPosition = new Vector3 (transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);

	

	}

	void OnCollisionEnter(Collision other)
	{
		Vector3 push = other.impulse * -1;
		rigidBody.AddForce(push);
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
			gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;

			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

			gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, -10, 0));
			if (gameObject == combatController.GetLeader ()) 
			{
				combatController.deadLeader = true;
			}
			//Destroy (gameObject);
			//gameObject.SetActive(false);

		}
	}
		
}
