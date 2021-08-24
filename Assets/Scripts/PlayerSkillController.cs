using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSkillController : MonoBehaviour
{
    UI_Skill uiSkill;
    Camera cam;
    Weapon weapon;
    Transform weaponObject;
        
    Transform spawnPoint;
    public GameObject grenade;
    float grenadeRange = 20f;
    public float grenadeCooltime = 3f;
    public bool isGrenadeCooldown = false;

    public GameObject handPosition;
    public GameObject shockwave;
    public ParticleSystem shockwaveEffect;
    float shockwaveRange = 60f;
    float shockwaveDistance = 12f;
    float shockwaveDamage = 20f;
    float shockwaveTick = 0.4f;
    float shockwaveDuration = 2f;
    public float shockwaveCooltime = 5f;
    public bool isShockwaveCooldown = false;
    public bool isCollision = false;

    UI_Upgrade upgrade;

    public float ShockwaveDamageSetter
    {
        get { return shockwaveDamage; }
        set { shockwaveDamage = value; }
    }

    private void Awake()
    {
        upgrade = GameObject.Find("Upgrade Panel").GetComponent<UI_Upgrade>();
        upgrade.SetShockwaveSlider(shockwaveDamage);
    }

    void Start()
    {        

        weapon = GameObject.Find("Player").transform.GetComponent<Weapon>();
        weaponObject = GameObject.Find("Weapon").transform;

        spawnPoint = transform.Find("Grenade Launcher");
        cam = Util.FindChild<Camera>(transform.gameObject, "MainCamera", true);
        uiSkill = GameObject.Find("Skill Canvas").GetComponent<UI_Skill>();

        handPosition = GameObject.Find("Skill Hand Position");
        shockwave = handPosition.transform.Find("Skill Hand").gameObject;
        shockwaveEffect = Util.FindChild<ParticleSystem>(shockwave, "FlameThrower", true);
    }

    void Update()
    {        
        LaunchGrenade();
        Shockwave();
    }

    private void LateUpdate()
    {
        handPosition.transform.rotation = cam.transform.rotation;
    }

    void LaunchGrenade()
    {
        if(Input.GetKeyDown(KeyCode.Q) && isGrenadeCooldown == false)
        {
            isGrenadeCooldown = true;
            GameObject _grenade = Instantiate(grenade, spawnPoint.position, spawnPoint.rotation);
            _grenade.GetComponent<Rigidbody>().AddForce(cam.transform.forward * grenadeRange, ForceMode.Impulse);
            uiSkill.skillGrenade.fillAmount = 1;
        }        
    }

    void Shockwave()
    {
        if (Input.GetKeyDown(KeyCode.F) && isShockwaveCooldown == false)
        {
            isShockwaveCooldown = true;
            shockwave.SetActive(true);
            shockwaveEffect.Play();
            StartCoroutine(SkillHandEquipAnimation());
            StartCoroutine(ShockwaveDamage());

            uiSkill.skillShockwave.fillAmount = 1;
        }
    }

    IEnumerator ShockwaveDamage()
    {
        for (int i = 0; i < shockwaveDuration / shockwaveTick; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, shockwaveDistance);
            Vector3 direction;
            float dotValue = 0f;

            foreach (Collider near in colliders)
            {
                dotValue = Mathf.Cos(Mathf.Deg2Rad * (shockwaveRange / 2));
                direction = near.transform.position - transform.position;

                if(direction.magnitude < shockwaveDistance)
                {
                    if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                        isCollision = true;
                    else
                        isCollision = false;
                }
                else
                {
                    isCollision = false;
                }

                if(isCollision)
                {
                    if (near.CompareTag("Enemy"))
                    {
                        near.SendMessage("HitByPlayer", shockwaveDamage, SendMessageOptions.DontRequireReceiver);
                        if (near.gameObject.GetComponent<EnemyStates>().currentState == near.gameObject.GetComponent<EnemyStates>().patrolState || near.gameObject.GetComponent<EnemyStates>().currentState == near.gameObject.GetComponent<EnemyStates>().alertState)
                        {
                            near.gameObject.SendMessage("HiddenShot", GameObject.Find("Player").transform.position, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else if(near.CompareTag("Boss"))
                    {
                        near.SendMessage("HitByPlayer", shockwaveDamage, SendMessageOptions.DontRequireReceiver);
                    }
                    else if (near.CompareTag("Missile"))
                    {
                        near.SendMessage("MissileHitByPlayer", shockwaveDamage, SendMessageOptions.DontRequireReceiver);
                    }
                }                
            }
            yield return new WaitForSeconds(shockwaveTick);
        }

        yield return null;
    }

    IEnumerator SkillHandEquipAnimation()
    {
        weapon.enabled = false;
        weaponObject.gameObject.SetActive(false);

        if (shockwave.GetComponent<Animator>())
        {
            shockwave.GetComponent<Animator>().Play("SkillStart", 0, 0);
        }
        else
        {
            Debug.Log("Can't find Component!");
        }

        yield return new WaitForSeconds(shockwaveDuration);

        shockwaveEffect.Stop();
        shockwave.GetComponent<Animator>().Play("SkillEnd", 0, 0);

        yield return new WaitForSeconds(1);

        shockwave.SetActive(false);
        weapon.enabled = true;
        weaponObject.gameObject.SetActive(true);
    }
}
