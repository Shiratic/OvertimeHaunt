using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject bulletPrefab;
    public GameObject crossHair;   // assign in inspector
    public Animator anim;          // player animator

    public float shootCooldown = 0.5f;
    private float shootTimer;

    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire") && shootTimer <= 0)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector2 shootDir = (crossHair.transform.position - launchPoint.position).normalized;

        // Spawn slightly forward in shoot direction
        Vector2 spawnPos = (Vector2)launchPoint.position + shootDir * 0.2f;

        Bullet bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity).GetComponent<Bullet>();
        bullet.Init(shootDir);

        // Trigger animation
        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }

        shootTimer = shootCooldown;
    }
}
