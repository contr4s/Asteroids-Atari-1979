﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip: Singleton<PlayerShip>
{
    [Header("Set in Inspector")]
    public float shipSpeed = 10f;

    [SerializeField] private int _lifes = 3;
    [SerializeField] private ProjectilePool _projectilePool;
    private Rigidbody _rigid;
    private Camera _mainCam;

    static public Vector3 position
    {
        get
        {
            return S.transform.position;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _rigid = GetComponent<Rigidbody>();
        _mainCam = Camera.main;

        if (_projectilePool == null)
            _projectilePool = GetComponentInChildren<ProjectilePool>();
    }

    void Update()
    {
        float aX = Input.GetAxis("Horizontal");
        float aY = Input.GetAxis("Vertical");

        Vector3 vel = new Vector3(aX, aY);
        if (vel.magnitude > 1)
        {
            // Avoid speed multiplying by sqrt(2) when moving at a diagonal
            vel.Normalize();
        }

        _rigid.velocity = vel * shipSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "asteroid")
        {
            _lifes--;
        }
    }

    private void Fire()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = -_mainCam.transform.position.z;
        Vector3 mPos3D = _mainCam.ScreenToWorldPoint(mPos);

        var projectile = _projectilePool.GetAvailableObject();
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(mPos3D);
        projectile.gameObject.SetActive(true);
    }
}