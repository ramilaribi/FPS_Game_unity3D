using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst 
    public int bulletPerBurst = 3;
    public int burstBulletsLeft ;

    //spread 
    public float spreadIntensity;


    // Bullet
    // Prefab of the bullet to be instantiated
    public GameObject bulletPrefab;
    // Transform representing the spawn point of the bullet
    public Transform bulletSpawn;
    // Velocity at which the bullet will travel
    public float bulletVelocity = 30f;
    // Time duration (in seconds) for which the bullet prefab will exist before being destroyed
    public float bulletPrefabLifeTime = 3f;



    public enum ShootingMode 
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
    }
    void Update()
    {
       if (currentShootingMode == ShootingMode.Auto)
        {
            // holding down left mouse for auto shoot 
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
       else  if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst) 
        {
            // clicking left mouse once to shoot 
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        }
       if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletPerBurst;
            FireWeapon();
        }
    }



    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
       
        //  Instantiate a new bullet GameObject at the position of the bulletSpawn with no rotation (Quaternion.identity)
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        // Poiting the bullet to face to shooting direction
        bullet.transform.forward = shootingDirection;
        // Shoot the Bullet 
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity , ForceMode.Impulse) ;
       
        // Destroy the bullet after time so that s why we use a couroutine because the code still runing until the delay end 
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        // checking if the shot is done
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        //burst mode 
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // We are ready to shot once before this check 
        {
            burstBulletsLeft--;
            Invoke("FireWeapon",shootingDelay);
        }
     }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        // shooting from the middle where we are pointing at 
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray , out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        // returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);
    }




    // IEnumerator i used for type couroutine methode
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);

    }
}
