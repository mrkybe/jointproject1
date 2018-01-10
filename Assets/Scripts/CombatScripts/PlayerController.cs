using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject enemySpawner;
	public GameObject cameraObject;
	public GameObject combatField;

	private bool flag = false;
    private Move move;
    private Spaceship sp;
    private Fire fire;
    private CameraController cc;
    private CameraFollow cf;
    private LaserFire lf;
	// Use this for initialization
	void Start () {
        move = GetComponent<Move>();
        sp = GetComponent<Spaceship>();
        fire = GetComponent<Fire>();
        lf = GetComponent<LaserFire>();
        cc = cameraObject.GetComponent<CameraController>();
        cf = cameraObject.GetComponent<CameraFollow>();
	}

    private void Update()
    {
        if (Input.GetButtonDown("LB") && fire.enabled == false)
        {
            fire.enabled = true;
            lf.enabled = false;
        }
        else if (Input.GetButtonDown("LB") && lf.enabled == false)
        {
            lf.enabled = true;
            fire.enabled = false;
        }
    }
    void FixedUpdate()  // called each physics steps
	{
		if (Input.GetButtonDown("Y") && flag == false)
        {
			flag = true;
            move.enabled = true;
            sp.enabled = false;
            fire.enabled = true;
            cc.enabled = true;
            cf.enabled = false;
            lf.enabled = false;
			transform.position = new Vector3 (combatField.transform.position.x, combatField.transform.position.y, combatField.transform.position.z);

			cameraObject.transform.position = new Vector3 (cameraObject.transform.position.x, cameraObject.transform.position.y+20, cameraObject.transform.position.z);
			combatField.SetActive (true);
			enemySpawner.GetComponent<EnemySpawner>().enabled = true;
		}
    }
}
