using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<Transform> turretBarrels;
    public GameObject bulletPrefab;
    public float reloadDelay = 0.5f;

    private bool canShoot = true;
    private Collider2D[] tankColliders;
    public float currentDelay = 0;

    private void Awake()
    {
        tankColliders = GetComponentsInParent<Collider2D>();
    }

    private void Update()
    {
        if (canShoot == false)
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                canShoot = true;
            }
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            currentDelay = reloadDelay;
            foreach (var barrel in turretBarrels)
            {
                Vector2 bulletStartVector = new Vector2();
                bulletStartVector = barrel.position;
                GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletStartVector, Quaternion.identity);
                // bullet.transform.position = barrel.position;
                bullet.transform.localRotation = barrel.rotation;
                bullet.GetComponent<Bullet>().Initialize();
                foreach (var collider in tankColliders)
                {
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), collider);
                }
            }
        }
    }
}