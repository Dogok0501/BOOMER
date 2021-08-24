using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DuckBoss : MonoBehaviour
{
    public Monster monster;

    int maxHealth;
    float currentHealth;

    public UI_MonsterHealth healthBar;
    public GameObject floatingText;
    public ParticleSystem explosionEffect;
    public ParticleSystem deathFire;

    EnemyStates es;
    NavMeshAgent nma;
    BoxCollider bc;
    GameObject vision;
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
        bc = GetComponent<BoxCollider>();
        vision = transform.Find("Vision").gameObject;
        anim = GetComponent<Animator>();

        if (currentHealth == maxHealth)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Managers.Sound.Play("BossDeath", SoundManager.SoundType.Effect);
            EnemyDie();
            GameObject.Find("InventoryThings").SendMessage("HuntCounter", 3002, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
            enabled = false;
        }
    }

    void InfoSet()
    {
        string string_name = this.GetType().Name;

        for (int i = 3000; i < Managers.Data.monstertDict.Count + 3000; i++)
        {
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

        if (floatingText)
        {
            ShowFloatingText(damage);
        }

        healthBar.SetHealth(currentHealth);
    }

    void ShowFloatingText(float damage)
    {
        if (currentHealth <= 0)
            return;

        Vector3 Offset = new Vector3(0f, 0.5f, 0.1f);
        Vector3 RandomizeIntensity = new Vector3(0.1f, 0.1f, 0f);

        GameObject _floatingText = Managers.Pool.Pop(floatingText, transform).gameObject;
        _floatingText.transform.position = transform.position;
        _floatingText.transform.rotation = Quaternion.identity;
        _floatingText.GetComponent<TextMesh>().characterSize = 0.25f;

        _floatingText.transform.localPosition += Offset;
        _floatingText.transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x), Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y), Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));

        _floatingText.GetComponent<TextMesh>().text = damage.ToString();
    }

    void EnemyDie()
    {       
        Managers.Sound.Clear();
        Managers.Sound.Play("BGMs/ToEnding", SoundManager.SoundType.BGM);

        es.enabled = false;
        nma.enabled = false;
        anim.enabled = false;
        anim.Rebind();
        bc.isTrigger = true;
        vision.SetActive(false);
        healthBar.gameObject.SetActive(false);

        GameObject engram = Resources.Load("Prefabs/Exotic Engram") as GameObject;
        Instantiate(engram, transform.position, engram.transform.rotation);
        GameObject bossDeath = Resources.Load("Prefabs/BossDeath") as GameObject;
        Instantiate(bossDeath, transform.position, engram.transform.rotation);
    }
}
