using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;

    [SerializeField]
    float moveSpeed = 1;
    [SerializeField]
    float aimSpeed = 1;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float shootPower = 1;

    [SerializeField]
    float projectileRadius, projectileForce, projectileTime;
    [SerializeField]
    float recoil = 1;

    Transform gunPivot;
    Transform shootPoint;

    Transform projectileContainer;

    Vector2 gunDirection;

    private void Awake()
    {
        player = this;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gunPivot = GetChildComponent<Transform>("GunPivot");
        shootPoint = FindGameObjectComponent<Transform>("ShootPoint");

        if (projectileContainer == null)
            projectileContainer = new GameObject("#ProjectileContainer").transform;

    }

    Vector3 goTo;
    Rigidbody2D rb;

    void Update()
    {
        Movement();
        Aim();

        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    void Movement()
    {
        goTo.x = Input.GetAxis("Horizontal");
        goTo.y = Input.GetAxis("Vertical");

        rb.position = transform.position + goTo * moveSpeed * Time.deltaTime;
    }

    void Aim()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;
        direction = Vector3.Lerp(transform.up, direction, Time.deltaTime * aimSpeed);
        transform.up = direction;

        gunDirection = (mouseScreenPosition - (Vector2)gunPivot.position).normalized;
        gunDirection = Vector3.Lerp(gunPivot.up, gunDirection, Time.deltaTime * aimSpeed);
        gunPivot.up = gunDirection;
    }

    void Shoot()
    {
        GameObject tempProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation, projectileContainer);
        tempProjectile.GetComponent<Rigidbody2D>().AddForce(shootPoint.up * shootPower, ForceMode2D.Impulse);
        tempProjectile.GetComponent<explosiveProjectile>().setStats(projectileRadius, projectileForce, projectileTime);

        rb.AddForce(-gunDirection * recoil, ForceMode2D.Impulse);
    }



    T GetChildComponent<T>(string name)
    {
        if (transform.Find("#" + name) != null)
            return transform.Find("#" + name).GetComponent<T>();

        Debug.LogError("\"#" + name + "\" child missing");
        return default(T);
    }

    T FindGameObjectComponent<T>(string name)
    {
        if (GameObject.Find("#" + name) != null)
            return GameObject.Find("#" + name).GetComponent<T>();

        Debug.LogError("\"#" + name + "\" game object missing");
        return default(T);
    }


}
