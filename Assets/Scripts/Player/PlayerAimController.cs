using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimController : MonoBehaviour
{
    [Header("Shooting Options")]
    public GameObject bulletPrefab;
    public float roundsPerMinute;
    private float shotCoolDown;

    private bool isShooting;
    private float currentShotCooldown = 0;

    private Vector2 currentAimDirection;
    private Vector3 mousePos;

    private void Awake()
    {
        shotCoolDown = 1 / (roundsPerMinute / 60f);
        Debug.Log("Current Cooldown" + shotCoolDown);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAim();
        HandleShot();
    }

    void HandleAim()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        currentAimDirection = (new Vector2(mousePos.x, mousePos.y) - new Vector2(transform.position.x, transform.position.y)).normalized;

    }

    void HandleShot()
    {
        if (currentShotCooldown > 0)
        {
            currentShotCooldown -= Time.deltaTime;
        }
        if (isShooting && currentShotCooldown <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;

            Vector2 lookDir = mousePos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            currentShotCooldown = shotCoolDown;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, currentAimDirection);
    }

    // This calls on both press down, as well as press up
    void OnFire(InputValue value)
    {
        // Sets to true if called for button being pressed down, sets false for opposite
        isShooting = value.isPressed;
    }
}
