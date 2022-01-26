using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BasicBullet : MonoBehaviour
{
    [Header("General")]
    public float speed;
    public float damage;
    public bool isPlayerBullet;
    public bool penetratesEnemies;
    public bool penetratesWalls;
    public float lifetime;

    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += transform.up * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        CheckCollisions();
    }

    void CheckCollisions()
    {
        // Check walls
        if (!penetratesWalls)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius, LayerMask.GetMask("Obstacle"));
            foreach (Collider2D hit in hits)
            {
                if (hit == circleCollider)
                    continue;

                Destroy(gameObject);
            }
        }

        // Should check if bullet hit player
        if (!isPlayerBullet)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius, LayerMask.GetMask("Player"));
            foreach (Collider2D hit in hits)
            {
                if (hit == circleCollider)
                    continue;

                Health health = hit.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(damage);
                }
                // Do something when player gets hit
                Destroy(gameObject);
                
            }
        }
        else // this is enemy bullet
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D hit in hits)
            {
                if (hit == circleCollider)
                    continue;

                // Do something when enemy gets hit
                Health health = hit.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(damage);
                }

                if (!penetratesEnemies)
                {
                    Destroy(gameObject);
                }
                
                

            }
        }

    }
}
