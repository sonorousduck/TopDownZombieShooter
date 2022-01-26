using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement Values")]
    public float speed;
    public float movementAcceleration;
    public float closenessTolerance;
    private Vector2 currentVelocity;
    private Vector2 currentWalkDirection;


    [SerializeField] private Vector2 goToPosition;

    [Header("Collision Detection")]
    public bool canGoThroughWalls;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, goToPosition) > closenessTolerance)
        {
            // We should keep moving toward it
            currentWalkDirection = (goToPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
        else
        {
            currentWalkDirection = Vector2.zero;
        }
        HandleMovement();
        HandleCollisions();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, currentWalkDirection * 2);
    }

    public void SetTargetPosition(Vector2 position)
    {
        goToPosition = position;
    }

    void HandleMovement()
    {
        currentVelocity = Vector2.MoveTowards(currentVelocity, speed * currentWalkDirection, movementAcceleration * Time.deltaTime);
        transform.Translate(currentVelocity * Time.deltaTime);
    }

    void HandleCollisions()
    {
        if (!canGoThroughWalls)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius, LayerMask.GetMask("Obstacle"));
            foreach (Collider2D hit in hits)
            {
                if (hit == circleCollider)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(circleCollider);

                if (colliderDistance.isOverlapped)
                {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                }
            }
        }
        
    }
}
