using Assets.Scripts.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {
    public GameObject enemySpawner;
    public GameObject cameraObject;
    public GameObject combatField;
    public GameObject ai_player;
    public GameObject player;

    private bool flag = false;
    private Move move;
    private Fire fire;
    private CameraController cc;
    private CameraFollow cf;
    private LaserFire lf;
	private LineRenderer lr;
    // Use this for initialization
    void Start()
    {
        move = player.GetComponent<Move>();
        fire = player.GetComponent<Fire>();
        lf = player.GetComponent<LaserFire>();
        cc = cameraObject.GetComponent<CameraController>();
        cf = cameraObject.GetComponent<CameraFollow>();
		lr = player.GetComponent<LineRenderer> ();
    }
    ///<summary>
    /// Checks every frame if player has pressed the corresponding button that switches between fire and laser fire scripts.
    /// This is how we will handle "switching" between weapons.
    ///</summary>
    private void Update()
    {
        if (Input.GetButtonDown("Y") && flag == false)
        {
            flag = true;
            ai_player.SetActive(false);
            player.SetActive(true);
			lr.enabled = false;
            // move.enabled = true;
            // fire.enabled = true;
            cc.enabled = true;
            cf.enabled = false;
            lf.enabled = false;
            combatField.SetActive(true);
            player.transform.position = new Vector3(combatField.transform.position.x, combatField.transform.position.y + 2f, combatField.transform.position.z);
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y + 20, cameraObject.transform.position.z);
            enemySpawner.GetComponent<EnemySpawner>().enabled = true;
        }
    }
    ///<summary>
    /// After Every fixed amount of frames we will check if combat has initiated. For testing purposes combat can be initiated by
    /// pressing the "Y" button.
    ///</summary>
}
