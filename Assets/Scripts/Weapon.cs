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



    // Prefab of the bullet to be instantiated
    public GameObject bulletPrefab;
    // Transform representing the spawn point of the bullet
    public Transform bulletSpawn;
    // Velocity at which the bullet will travel
    public float bulletVelocity = 30f;
    // Time duration (in seconds) for which the bullet prefab will exist before being destroyed
    public float bulletPrefabLifeTime = 3f;


    void Update()
    {
        // left mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {   //  Instantiate a new bullet GameObject at the position of the bulletSpawn with no rotation (Quaternion.identity)
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        // Shoot the Bullet 
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity , ForceMode.Impulse) ;
        // Destroy the bullet after time so that s why we use a couroutine because the code still runing until the delay end 
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

     }
    // IEnumerator i used for type couroutine methode
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);

            }
}
