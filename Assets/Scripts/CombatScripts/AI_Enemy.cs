using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	public GameObject Player;

	// Use this for initialization
	void Start () {
		x = Random.Range(-velocidadMax, velocidadMax);
		z = Random.Range(-velocidadMax, velocidadMax);
		angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
		transform.localRotation = Quaternion.Euler( 0, angulo, 0);
		dist = Vector3.Distance (Player.transform.position, gameObject.transform.position);
		f = GetComponent<Fire> ();

		Debug.DrawLine (transform.position, transform.forward);

	}

	// Update is called once per frame
	void Update () {
		KillYourself ();
		if (count == 0) {
			
			Player = GameObject.FindGameObjectWithTag("Player");
			count++;
		}
		Debug.Log (Player.transform.position);
		dist = Vector3.Distance (Player.transform.position, gameObject.transform.position);
		float step = speed * Time.deltaTime;
		if (dist < radius) {
			
			gameObject.transform.LookAt (Player.transform);
			f.enemyFire ();
			//gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, Player.transform.position, step);
		} else {

			tiempo += Time.deltaTime;

			if (transform.localPosition.x > xMax) {
				x = Random.Range (-velocidadMax, 0.0f);
				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
				transform.localRotation = Quaternion.Euler (0, angulo, 0);
				tiempo = 0.0f; 
			}
			if (transform.localPosition.x < xMin) {
				x = Random.Range (0.0f, velocidadMax);
				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
				transform.localRotation = Quaternion.Euler (0, angulo, 0); 
				tiempo = 0.0f; 
			}
			if (transform.localPosition.z > zMax) {
				z = Random.Range (-velocidadMax, 0.0f);
				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
				transform.localRotation = Quaternion.Euler (0, angulo, 0); 
				tiempo = 0.0f; 
			}
			if (transform.localPosition.z < zMin) {
				z = Random.Range (0.0f, velocidadMax);
				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
				transform.localRotation = Quaternion.Euler (0, angulo, 0);
				tiempo = 0.0f; 
			}


			if (tiempo > 1.0f) {
				x = Random.Range (-velocidadMax, velocidadMax);
				z = Random.Range (-velocidadMax, velocidadMax);
				angulo = Mathf.Atan2 (x, z) * (180 / 3.141592f) + 90;
				transform.localRotation = Quaternion.Euler (0, angulo, 0);
				tiempo = 0.0f;
			}

			transform.localPosition = new Vector3 (transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);

		}

	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			health = health - kineticDMG;
			Destroy (other.gameObject);
			if (health < 0) {
				Destroy (gameObject);
			}
		}

		if (other.gameObject.CompareTag("Laser"))
		{
			health = health - laserDMG;
			Destroy (other.gameObject);
			if (health < 0) {
				Destroy (gameObject);
			}
		}
		if (other.gameObject.CompareTag("Rocket"))
		{
			health = health - rocketDMG;
			other.gameObject.GetComponent<ParticleSystem> ().Play();
			Destroy (other.gameObject);
			if (health < 0) {
				Destroy (gameObject);
			}

		}
	}

	private void KillYourself()
	{
		if (health <= 0)
			Destroy (gameObject);
	}

	public void DepleteHealth(int dmg)
	{
		health -= dmg;
	}
}
