using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject enemySpawner;
	public GameObject cameraObject;
	public GameObject combatField;
	public GameObject ai_player;
	public GameObject player;

	private bool flag = false;
    private Move move;
    private Spaceship sp;
    private Fire fire;
    private CameraController cc;
    private CameraFollow cf;
    private LaserFire lf;
	private Timer tm;
	// Use this for initialization
	void Start () {
        move = player.GetComponent<Move>();
        sp = ai_player.GetComponent<Spaceship>();
        fire = player.GetComponent<Fire>();
        lf = player.GetComponent<LaserFire>();
		tm = GetComponent<Timer> ();
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
			ai_player.SetActive (false);
			player.SetActive (true);
            move.enabled = true;
            fire.enabled = true;
            cc.enabled = true;
            cf.enabled = false;
            lf.enabled = false;
			tm.Pause (false);
			combatField.SetActive (true);
			player.transform.position = new Vector3 (combatField.transform.position.x, combatField.transform.position.y, combatField.transform.position.z);
			cameraObject.transform.position = new Vector3 (cameraObject.transform.position.x, cameraObject.transform.position.y+20, cameraObject.transform.position.z);
			combatField.SetActive (true);
			enemySpawner.GetComponent<EnemySpawner>().enabled = true;
		}
    }
}
