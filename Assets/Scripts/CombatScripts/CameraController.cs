using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.WorldSingleton;

///<summary>
/// CameraController is put on the camera object in order to follow the player as they move.
///</summary>

public class CameraController : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object
    public float y_value = 100f;//Private variable to store the offset distance between the player and camera
    private GameObject enemy;
    private CombatController controller;
    private Transform cameraChildTransform;
    private new Camera camera;

    private GameObject overseer;
    [SerializeField]
    private float maxShake = 10;
    [SerializeField]
    private float shakeIntensity = 1;
    [SerializeField]
    bool isMinimap = false;

    private float shakeEnergy = 0;

    private float y_distance;
    // Use this for initialization
    ///<summary>
    /// From the start get the player's location and position the camera a given y value above the player.
    ///</summary>
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        y_distance = player.transform.position.y + y_value;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        overseer = GameObject.Find("Overseer");
        controller = overseer.GetComponent<CombatController>();
        camera = GetComponentInChildren<Camera>();
        camera.enabled = true;

        camera.gameObject.SetActive(true);
        if (!isMinimap)
        {
            cameraChildTransform = transform.GetChild(0).transform;
        }

        //enemy = controller.leader;
    }
    ///<summary>
    /// Check the player's distance and move the camera accordingly.
    ///</summary>
    // LateUpdate is called after Update each frame
    void FixedUpdate()
    {
        if (controller.showEnemy.Equals(false))
            FollowPlayer();

        //FollowPlayer ();
    }

    private void LateUpdate()
    {
        if (!isMinimap)
            ShakeUpdate();
    }

    void FollowPlayer()
    {
        y_distance = player.transform.position.y + y_value;
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = new Vector3(player.transform.position.x, y_distance, player.transform.position.z);
    }

    public void HitShake(float intensity)
    {
        shakeEnergy += intensity;
    }

    public void ShakeUpdate()
    {
        float rx = (Random.value - 0.5f) * shakeEnergy;
        float ry = (Random.value - 0.5f) * shakeEnergy;
        float rz = (Random.value - 0.5f) * shakeEnergy;

        Vector3 shake = new Vector3(rx, ry, rz);

        cameraChildTransform.transform.localPosition = shake;
        if (shakeEnergy < float.Epsilon)
        {
            shakeEnergy = 0;
        }
        else
        {
            shakeEnergy /= 1.05f;
        }
    }

    public void ShowEnemy(GameObject leader)
    {
        //Pause
        Time.timeScale = 0;
        y_distance = player.transform.position.y + y_value;
        float x = transform.position.x;
        x = Mathf.Lerp(player.transform.position.x, leader.transform.position.x, 5f * Time.deltaTime);
        //transform.position = new Vector3 (transform.position.x, y_distance, transform.position.z);
        //Vector3 dist = Vector3.Distance (enemy.transform.position, player.transform.position);


        //UnPause
        //wInvoke("UnPause", 5f);
    }
    void Lerp()
    {

    }

}

