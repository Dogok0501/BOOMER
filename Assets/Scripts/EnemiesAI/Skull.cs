using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skull : MonoBehaviour
{
    public Monster monster;

    public Sprite deadBody;
    int maxHealth;
    float currentHealth;

    public UI_MonsterHealth healthBar;
    public GameObject floatingText;
    public GameObject playerSpotted;

    EnemyStates es;
    NavMeshAgent nma;
    SpriteRenderer sr;
    BoxCollider bc;
    GameObject vision;
    DynamicBillboardChange dbc;
    Animator anim;
    Rigidbody rb;

    private void Start()
    {
        InfoSet();

        maxHealth = monster.health;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        es = GetComponent<EnemyStates>();
        nma = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
        dbc = GetComponent<DynamicBillboardChange>();
        vision = transform.Find("Vision").gameObject;
        anim = GetComponent<Animator>();

        if(currentHealth == maxHealth)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            EnemyDie();
            GameObject.Find("InventoryThings").SendMessage("HuntCounter", monster.index, SendMessageOptions.DontRequireReceiver);
            enabled = false;
        }
    }

    void InfoSet()
    {
        string string_name = this.GetType().Name;

        for (int i = 3000; i < Managers.Data.monstertDict.Count + 3000; i++)
        {
            Debug.Log(i);
            if (string_name == Managers.Data.monstertDict[i].name)
            {
                monster = Managers.Data.monstertDict[i];
                break;
            }
        }
    }

    void HitByPlayer(float damage)
    {
        currentHealth -= damage;
        if (healthBar.gameObject.activeSelf == false && currentHealth >= 0)
        {
            healthBar.gameObject.SetActive(true);
        }

        if(floatingText)
        {
            ShowFloatingText(damage);
        }
        
        healthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            Managers.Sound.Play("enemyHit", SoundManager.SoundType.Effect);
        }
    }

    void ShowFloatingText(float damage)
    {
        if (currentHealth <= 0)
            return;

        Vector3 Offset = new Vector3(0f, 3f, -1f);
        Vector3 RandomizeIntensity = new Vector3(0.5f, 0.5f, 0f);

        GameObject _floatingText = Managers.Pool.Pop(floatingText, transform).gameObject;
        _floatingText.transform.position = transform.position;
        _floatingText.transform.rotation = Quaternion.identity;
        _floatingText.GetComponent<TextMesh>().characterSize = 1f;

        _floatingText.transform.localPosition += Offset;
        _floatingText.transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x), Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y), Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        _floatingText.GetComponent<TextMesh>().text = damage.ToString();        
    }

    void EnemyDie()
    { 
        es.enabled = false;
        nma.enabled = false;
        dbc.enabled = false;
        anim.enabled = false;
        anim.Rebind();
        sr.sprite = deadBody;
        bc.center = new Vector3(0, -0.8f, 0);
        bc.size = new Vector3(1.05f, 0.43f, 0.2f);
        bc.isTrigger = true;
        vision.SetActive(false);
        healthBar.gameObject.SetActive(false);
        playerSpotted.gameObject.SetActive(false);

        GameObject engram = Resources.Load("Prefabs/Engram") as GameObject;
        for(int i = 0; i < 2; i++)
        {
            Instantiate(engram, transform.position, engram.transform.rotation);
        }
        GameObject ammo = Resources.Load("Prefabs/Items/AmmoBonus") as GameObject;
        Instantiate(ammo, transform.position + new Vector3 (1,0,0), ammo.transform.rotation);        
    }
}
