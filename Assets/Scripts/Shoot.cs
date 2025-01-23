using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float _maximumForce;

    [SerializeField]
    private float MaxChargeTime;

    private float _timeMouseButtonDown;

    private Camera _camera;

    public int playerWeaponDamage;
    public int playerAmmoInGun;
    public int playerMagazineCapacity;
    public int playerSpareAmmo;
    public bool isReloading;
    public float reloadTime;
    public float shootDelay;
    public bool isShooting;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false && playerSpareAmmo > 0)
        {
            isReloading = true;
            Invoke("ReloadGun", reloadTime);
        }

        // If reloading can't shoot
        if (isReloading == true || isShooting == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _timeMouseButtonDown = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && playerAmmoInGun > 0)
        {
            isShooting = true;
            Invoke ("SetIsShootingToFalse", shootDelay);
            playerAmmoInGun = playerAmmoInGun - 1;
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            LayerMask zombieLayer = LayerMask.GetMask("Default");
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 9999f, zombieLayer, QueryTriggerInteraction.Ignore))
            {
                Zombie zombie = hitInfo.collider.transform.root.GetComponent<Zombie>();


                if (zombie != null)
                {
                    float mouseButtonDownDuration = Time.time - _timeMouseButtonDown;
                    float forcePercentage;
                    float forceMagnitude;  //Mathf.Lerp(1, _maximumForce, forcePercentage);

                    if (mouseButtonDownDuration > MaxChargeTime)
                    {
                        forcePercentage = 1;
                    }
                    else
                    {
                        forcePercentage = mouseButtonDownDuration / MaxChargeTime;
                    }

                    forceMagnitude = forcePercentage * _maximumForce;


                    /*Vector3 forceDirection = zombie.transform.position - _camera.transform.position;
                    //forceDirection.y = 1;
                    forceDirection.Normalize();
                    */
                    Vector3 force = Vector3.forward;

                    zombie.TriggerRagdoll(force, hitInfo.point);
                    ScoreTracker myScoreTracker = FindObjectOfType<ScoreTracker>();
                    if (zombie.zombieHealth > playerWeaponDamage)
                    {
                        myScoreTracker.damageScore += playerWeaponDamage;
                    }
                    else if (zombie.zombieHealth > 0)
                    {
                        myScoreTracker.damageScore += zombie.zombieHealth;
                        if (zombie.zombieHealth <= 0)
                        {
                            myScoreTracker.killScore += 1;
                        }
                    }
                    
                    zombie.zombieHealth = zombie.zombieHealth - playerWeaponDamage;
                    
                    hitInfo.transform.GetComponent<Rigidbody>().AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);
                }


            }

        }
    }

    public void SetIsShootingToFalse()
    {
        isShooting = false;
    }

    public void ReloadGun()
    {
        if (playerAmmoInGun == playerMagazineCapacity) // If ammo full return
        {
            return;
        }
        int ammoToRemove = playerMagazineCapacity - playerAmmoInGun;
        if (ammoToRemove <= playerSpareAmmo)
        {
            playerSpareAmmo = playerSpareAmmo - ammoToRemove;
            playerAmmoInGun = playerMagazineCapacity;
        }
        else
        {
            playerAmmoInGun = playerAmmoInGun + playerSpareAmmo;
            playerSpareAmmo = 0;
        }
        isReloading = false;
    }
}
