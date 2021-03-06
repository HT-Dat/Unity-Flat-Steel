using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Photon.Pun;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public int damage = 10;
    public float maxDistance = 10;
    public UnityEvent OnHit;
    private Vector2 startPosition;
    private float travelledDistance = 0;
    private Rigidbody2D rb2d;
    private PhotonView view;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();

    }

    public void Initialize()
    {
        startPosition = transform.position;
        rb2d.velocity = transform.up * speed;
    }

    //Update calculate distance travelled of the bullet
    private void Update()
    {
        travelledDistance = Vector2.Distance(transform.position, startPosition);
        if (travelledDistance > maxDistance)
        {
            SelfDestruct();
        }
    }

    private void SelfDestruct()
    {
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Damagable damagable = col.GetComponent<Damagable>();
        OnHit?.Invoke();
        if (damagable != null)
        {
            damagable.Hit(damage);
        }
        CameraShake.Instance.ShakeCamera();
        SelfDestruct();
    }
}