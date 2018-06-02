using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.WorldSingleton;
using BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation;

///<summary>
/// LaserFire class is used for firing a laser by utilizing Unity's LineRenderer component.
///</summary>
public class LaserFire : MonoBehaviour
{
    private LineRenderer laser;
    public bool rayhit;

    private Material laserMaterial;
    public AudioClip shootSound;
    public Color LaserColor;
    public float LaserWidth;

    private AudioSource source;
    private CombatController manager;

    void Awake()
    {

        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();
        laser.enabled = false;
        manager = GameObject.Find("Overseer").GetComponent<CombatController>();
        laserMaterial = laser.material;
    }

    ///<summary>
    /// Check every frame if the fire button has been pressed, and starts coroutine FireLaser until the player lets go of the fir button.
    ///</summary>
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && manager.playerCanMove.Equals(true))
        {
            //StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
			GetComponent <AudioSource>().Play();
        }
		if (Input.GetMouseButtonUp (0) == true) {
			GetComponent<AudioSource> ().Stop ();
		}

        /*
		if (!rayhit)
			StopCoroutine("FireLaser");
	*/
    }

    ///<summary>
    /// IEnumerator function FireLaser is used to enable the LineRenderer and use a ray forward to check for a hit.
    /// If the ray hits an object, and if the object is an enemy, then destroy it. IEnumerator is used to create a sustained laser using coroutines. 
    ///</summary>
    IEnumerator FireLaser()
    {
        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            rayhit = true;
            laser.enabled = true;
            bool didHit = Physics.Raycast(ray, out hit);

            if (didHit)
            {
                laser.SetPosition(0, ray.origin);
                laser.SetPosition(1, ray.GetPoint(hit.distance) + transform.forward * 0.5f);
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<AI_Enemy>().DepleteHealth(1);
                }
                else if (hit.transform.gameObject.CompareTag("Bullet"))
                {
                    Destroy(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.CompareTag("Rocket"))
                {
                    Destroy(hit.transform.gameObject);
                }
                float rand_alph = Random.value / 2 + 0.5f;
                Color c = LaserColor;
                c.a = rand_alph;
                laserMaterial.SetColor("_Color", c);
                laser.startWidth = rand_alph;
                laser.endWidth = rand_alph;
            }
            else
            {
                laser.SetPosition(0, ray.origin);
                laser.SetPosition(1, transform.position + transform.forward * 1000f);
                float rand_alph = Random.value / 2 + 0.1f;
                Color c = LaserColor;
                c.a = rand_alph;
                laserMaterial.SetColor("_Color", c);
                laser.startWidth = rand_alph;
                laser.endWidth = rand_alph;
            }

            yield return null;
            laser.enabled = false;
        }
    }
}
