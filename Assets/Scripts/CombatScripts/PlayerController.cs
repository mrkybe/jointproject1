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
    private Text healthBarText;
    private Text weaponUIText;

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

    private enum Weapon { M2_MG, LASER, ROCKET }
    private Weapon currentWeapon = Weapon.M2_MG;


    void Awake()
    {

        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        overseerObject = GameObject.Find("Overseer");
        overseer = overseerObject.GetComponent<Overseer>();
        fire = GetComponent<Fire>();
        lf = GetComponent<LaserFire>();
        rk = GetComponent<Rocket>();
        combatController = GameObject.Find("Overseer").GetComponent<CombatController>();
        weaponUIText = GameObject.Find("WeaponUI").GetComponent<Text>();
        healthBarText = GameObject.Find("HealthBarText").GetComponent<Text>();
        //Debug.Log ("Health is:" + health);
        particleSystem = GetComponent<ParticleSystem>();
        SwitchWeapon(currentWeapon);
    }

    private void SwitchWeapon(Weapon switchTo)
    {
        fire.enabled = lf.enabled = rk.enabled = false;

        switch (switchTo)
        {
            case Weapon.M2_MG:
                weaponUIText.text = "Machine Gun";
                fire.enabled = true;
                break;
            case Weapon.LASER:
                weaponUIText.text = "Laser";
                lf.enabled = true;
                break;
            case Weapon.ROCKET:
                weaponUIText.text = "Rockets";
                rk.enabled = true;
                break;
        }
    }

    private void Update()
    {
        //&& fire.enabled == true)
        if (Input.GetButtonDown("LB") && fire.enabled == true)
        {
            SwitchWeapon(Weapon.LASER);

        }
        else if (Input.GetButtonDown("LB") && lf.enabled == true)
        {
            SwitchWeapon(Weapon.ROCKET);
        }
        else if (Input.GetButtonDown("LB") && rk.enabled == true)
        {
            SwitchWeapon(Weapon.M2_MG);
        }
        Dead();
    }

    public void Depletion(int damage)
    {
        health -= damage;

        currentHealthbar.rectTransform.localScale = new Vector3(health / 100f, 1, 1);
        healthBarText.text = (health).ToString() + '%';
    }

    public void Dead()
    {
        if (health <= 0)
        {
            health = 0;
            combatController.CombatEnd(CombatController.COMBAT_RESULT.PLAYER_DEATH);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("EnemyBullet"))
        {
            overseer.DoExplosion(transform.position, 12, .1f);
            Depletion(1);
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("CombatAsteroid") || other.gameObject.CompareTag("Enemy"))
        {
            Depletion(1);
            overseer.DoExplosion(transform.position, 12, .1f);
        }
    }
}