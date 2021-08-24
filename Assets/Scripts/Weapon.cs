using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    PlayerMovementController playerMovementController;

    public Camera cam;

    public Gun[] loadout;
    Text ammoText;
    public Transform weaponParent;
    GameObject currentWeapon;
    public GameObject[] myAllWeapon;
    int currentWeaponIndex;

    public GameObject bullethole;
    public GameObject bloodSplat;
    ParticleSystem muzzleFlash;

    public LayerMask canBeShot;
    float currentCooldown;

    bool isReloading;
    bool isEquiping;

    public Coroutine lastRoutine = null;

    float baseFOV;

    private void Start()
    {        
        playerMovementController = GetComponent<PlayerMovementController>();

        baseFOV = cam.fieldOfView;

        myAllWeapon = new GameObject[loadout.Length];
    }

    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Equip(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Equip(1);

        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if(loadout[currentWeaponIndex].burst == false)
            {
                if (Input.GetMouseButtonDown(0) && currentCooldown <= 0 && isReloading == false && isEquiping == false && myAllWeapon[currentWeaponIndex])
                {
                    if (loadout[currentWeaponIndex].CanFireBullet())
                        Shoot();
                    else if (loadout[currentWeaponIndex].GetCurrentAmmo() != 0 && (loadout[currentWeaponIndex].GetCurrentClip() != loadout[currentWeaponIndex].maxClip))
                        lastRoutine = StartCoroutine(Reload(loadout[currentWeaponIndex].reloadTime));
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && currentCooldown <= 0 && isReloading == false && isEquiping == false && myAllWeapon[currentWeaponIndex])
                {
                    if (loadout[currentWeaponIndex].CanFireBullet())
                        Shoot();
                    else if (loadout[currentWeaponIndex].GetCurrentAmmo() != 0 && (loadout[currentWeaponIndex].GetCurrentClip() != loadout[currentWeaponIndex].maxClip))
                        lastRoutine = StartCoroutine(Reload(loadout[currentWeaponIndex].reloadTime));
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && myAllWeapon[currentWeaponIndex] && loadout[currentWeaponIndex].GetCurrentAmmo() != 0 && (loadout[currentWeaponIndex].GetCurrentClip() != loadout[currentWeaponIndex].maxClip) && isReloading == false && isEquiping == false)
            {
                lastRoutine = StartCoroutine(Reload(loadout[currentWeaponIndex].reloadTime));
            }

            // 총 위치 원상복구
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

            // rpm
            if (currentCooldown > 0)
                currentCooldown -= Time.deltaTime;

            RefreshAmmo();
        }
    }

    void RefreshAmmo()
    {
        ammoText = Util.FindChild<Text>(weaponParent.gameObject, "Ammo", true);

        if (myAllWeapon[currentWeaponIndex])
        {
            int ammo = loadout[currentWeaponIndex].GetCurrentAmmo();
            int clip = loadout[currentWeaponIndex].GetCurrentClip();
            ammoText.text = clip + " / " + ammo;
        }        
    }

    IEnumerator Reload(float wait)
    {
        isReloading = true;
        if (currentWeapon.GetComponent<Animator>())
        {
            currentWeapon.GetComponent<Animator>().Play("Reload", 0, 0);
            Managers.Sound.Play($"Gun/Reload/{loadout[currentWeaponIndex].gunName}");
        }
        else
        {
            Debug.Log("Can't find Component!");
        }

        yield return new WaitForSeconds(wait);

        loadout[currentWeaponIndex].Reload();
        isReloading = false;
    }

    IEnumerator EquipAnimation(GameObject newWeapon, float drawSpeed)
    {
        isEquiping = true;

        if (newWeapon.GetComponent<Animator>())
        {
            newWeapon.GetComponent<Animator>().Play("Equip", 0, 0);
        }
        else
        {
            Debug.Log("Can't find Component!");
        }

        yield return new WaitForSeconds(drawSpeed);

        isEquiping = false;
    }

    //void OldEquip (int index)
    //{        
    //    if (currentWeapon != null && currentWeapon.name != loadout[0].name)
    //    {
    //        if(isReloading == true)
    //        {
    //            StopCoroutine(lastRoutine);
    //            isReloading = false;
    //        }
    //        Destroy(currentWeapon);
    //    }            

    //    cureentWeaponIndex = index;

    //    GameObject newWeapon = Instantiate(loadout[cureentWeaponIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent);
    //    newWeapon.transform.localPosition = Vector3.zero;
    //    newWeapon.transform.localEulerAngles = Vector3.zero;

    //    StartCoroutine(EquipAnimation(newWeapon, loadout[cureentWeaponIndex].drawSpeed));

    //    StopCoroutine("Reload");

    //    currentWeapon = newWeapon;

    //    muzzleFlash = Util.FindChild<ParticleSystem>(currentWeapon, null, true);
    //}

    public void Equip(int index)
    {
        if(isReloading == false && myAllWeapon[index])
        {
            if (currentWeapon != null)// && currentWeapon.name != loadout[index].name)
            {
                if (isReloading == true)
                {
                    StopCoroutine(lastRoutine);
                    isReloading = false;
                }
                currentWeapon.SetActive(false);
            }

            currentWeaponIndex = index;

            myAllWeapon[currentWeaponIndex].SetActive(true);

            currentWeapon = myAllWeapon[currentWeaponIndex];

            StartCoroutine(EquipAnimation(currentWeapon, loadout[currentWeaponIndex].drawSpeed));

            muzzleFlash = Util.FindChild<ParticleSystem>(currentWeapon, null, true);
        }        
    }

    public void PickupWeapon(string name)
    {
        switch(name)
        {
            case "Handgun" :
                currentWeaponIndex = 0;
                break;
            case "Shotgun" :
                currentWeaponIndex = 1;
                break;
            default :
                break;
        }

        if(myAllWeapon[currentWeaponIndex])
        {
            Equip(currentWeaponIndex);
            loadout[currentWeaponIndex].Initialize();
        }
        else if (!myAllWeapon[currentWeaponIndex])
        {
            loadout[currentWeaponIndex].Initialize();
            //무기습득
            myAllWeapon[currentWeaponIndex] = Instantiate(loadout[currentWeaponIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent);
            myAllWeapon[currentWeaponIndex].transform.localPosition = Vector3.zero;
            myAllWeapon[currentWeaponIndex].transform.localEulerAngles = Vector3.zero;
            myAllWeapon[currentWeaponIndex].SetActive(true);
            Equip(currentWeaponIndex);
        }
    }

    void Aim(bool aim)
    {
        Transform anchor = Util.FindChild(currentWeapon, "Anchor", true).transform;
        Transform stateADS = Util.FindChild(currentWeapon, "ADS", true).transform;
        Transform stateHip = Util.FindChild(currentWeapon, "Hip", true).transform;

        if (aim && !playerMovementController.isSprinting)
        {
            anchor.position = Vector3.Lerp(anchor.position, stateADS.position, Time.deltaTime * loadout[currentWeaponIndex].aimSpeed);
            DynamicCrosshair.spread *= (float)DynamicCrosshair.ANIMMING_SPREAD;
            transform.Find("Weapon").GetComponent<Weaponbob>().enabled = false;
            GameObject.Find("Crosshair").GetComponent<DynamicCrosshair>().HideCrosshair();
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, baseFOV * 1 / 2, Time.deltaTime * 1000f);
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, stateHip.position, Time.deltaTime * loadout[currentWeaponIndex].aimSpeed);
            transform.Find("Weapon").GetComponent<Weaponbob>().enabled = true;
            GameObject.Find("Crosshair").GetComponent<DynamicCrosshair>().ShowCrosshair();
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }
    }

    void Shoot()
    {
        for (int i = 0; i < Mathf.Max(1, loadout[currentWeaponIndex].pellets); i++)
        {
            // 탄 퍼짐
            Vector2 bulletOffset = Random.insideUnitCircle * (DynamicCrosshair.spread + loadout[currentWeaponIndex].bulletSpread); // 반지름이 1인 원 내부에서의 랜덤 점 * 크로스헤어의 퍼짐(기본은 0) + 각 총기의 탄 퍼짐을 탄 퍼짐에 저장
            Vector3 randomTarget = new Vector3(Screen.width / 2 / 4 + bulletOffset.x, Screen.height / 2 / 4 + bulletOffset.y, 0); // Vector3에 화면 너비의 1/2 + 탄 퍼짐.x, 화면 높이의 1/2 + 탄 퍼짐.y를 저장
            Ray ray = Camera.main.ScreenPointToRay(randomTarget); // 카메라에서 시작된 화면 지점(randomTarget)을 지나는 ray 반환하여 저장
            RaycastHit hit;

            // 레이캐스트        
            if (Physics.Raycast(ray, out hit, loadout[currentWeaponIndex].range, canBeShot))
            {                
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.SendMessage("HitByPlayer", loadout[currentWeaponIndex].damage, SendMessageOptions.DontRequireReceiver);
                    GameObject _bloodSplat = Managers.Pool.Pop(bloodSplat).gameObject;
                    _bloodSplat.transform.position = hit.point;
                    _bloodSplat.transform.rotation = Quaternion.identity;

                    if (hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().patrolState || hit.collider.gameObject.GetComponent<EnemyStates>().currentState == hit.collider.gameObject.GetComponent<EnemyStates>().alertState)
                    {
                        hit.collider.gameObject.SendMessage("HiddenShot", transform.position, SendMessageOptions.DontRequireReceiver);
                    }
                }
                else if(hit.transform.CompareTag("Boss"))
                {
                    hit.collider.gameObject.SendMessage("HitByPlayer", loadout[currentWeaponIndex].damage, SendMessageOptions.DontRequireReceiver);
                    GameObject _bloodSplat = Managers.Pool.Pop(bloodSplat).gameObject;
                    _bloodSplat.transform.position = hit.point;
                    _bloodSplat.transform.rotation = Quaternion.identity;
                }
                else if(hit.transform.CompareTag("Missile"))
                {
                    Debug.Log("요격");
                    hit.collider.gameObject.SendMessage("MissileHitByPlayer", loadout[currentWeaponIndex].damage, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    GameObject _bullethole = Managers.Pool.Pop(bullethole).gameObject;
                    _bullethole.transform.position = hit.point;
                    _bullethole.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    _bullethole.transform.parent = hit.collider.gameObject.transform;
                }
            }
        }        

        // 반동
        currentWeapon.transform.Rotate(-loadout[currentWeaponIndex].recoil, 0, 0);
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentWeaponIndex].kickback;

        // rpm
        currentCooldown = loadout[currentWeaponIndex].firerate;

        // 격발음, 총염 생성
        Managers.Sound.Play($"Gun/Fire/{loadout[currentWeaponIndex].gunName}");
        muzzleFlash.Play();
    }

    public void ObtainAmmo(int ammo)
    {
        for(int i = 0; i < myAllWeapon.Length; i++)
            loadout[i].ObtainAmmo(ammo);
    }
}
