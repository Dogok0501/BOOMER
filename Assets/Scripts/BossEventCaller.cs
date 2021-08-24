using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEventCaller : MonoBehaviour
{
    public Camera mainCam;
    public Camera weaponCam;
    public Transform eventPosition;
    public GameObject player;
    public GameObject duck;
    public GameObject ui;
    public Image blackFade;
    public GameObject cinematicLine;
    public Text DUCK;

    bool playerEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerEnter = true;
            blackFade.gameObject.SetActive(true);
            DUCK.gameObject.SetActive(true);
            cinematicLine.gameObject.SetActive(true);

            Managers.Sound.Clear();
            Managers.Sound.Play("BGMs/BossRoom", SoundManager.SoundType.BGM);
        }
    }

    private void Update()
    {
        if(playerEnter)
        {            
            StartCoroutine(CameraMove());
            StartCoroutine(Util.FadeOut<Image>("BlackFade", 3f));
        }
    }

    IEnumerator CameraMove()
    {      
        float elapsedTime = 0;
        float waitTime = 10f;
        Vector3 currentPos = mainCam.transform.position;
                
        ComponentController(false);   

        mainCam.transform.LookAt(duck.transform.position + new Vector3(0f, 5f, 0f));

        while (elapsedTime < waitTime)
        {
            mainCam.transform.position = Vector3.Lerp(currentPos, eventPosition.position, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        mainCam.transform.position = eventPosition.position;
        StartCoroutine(Util.FadeIn<Text>("DUCK", 3f));
        yield return new WaitForSeconds(3f);

        mainCam.transform.position = weaponCam.transform.position;
        ComponentController(true);

        blackFade.gameObject.SetActive(false);
        DUCK.gameObject.SetActive(false);
        cinematicLine.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    void ComponentController(bool onoff)
    {
        player.GetComponent<PlayerMovementController>().enabled = onoff;
        player.GetComponent<Weapon>().enabled = onoff;
        player.GetComponent<PlayerSkillController>().enabled = onoff;
        player.GetComponent<PlayerLook>().enabled = onoff;
        player.transform.Find("Weapon").gameObject.SetActive(onoff);

        ui.transform.Find("HUD").gameObject.SetActive(onoff);
        ui.transform.Find("Quest Canvas").gameObject.SetActive(onoff);
    }
}
