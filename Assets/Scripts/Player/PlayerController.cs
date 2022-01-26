using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    public float baseSpeed;
    public float walkAcceleration;
    public float dashTime;
    public float dashSpeedModifier;
    public float dashCooldownTime;

    private Vector2 currentInput;
    private Vector2 currentVelocity;
    private float currentDashCoolDown;
    private float currentDashTime;

    [Header("Collision Detection")]
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleCollisions();

        
        if (currentDashTime > 0)
        {
            currentDashTime -= Time.deltaTime;
            
        }
        else if (currentDashCoolDown > 0)
        {
            currentDashCoolDown -= Time.deltaTime;
        }
    }

    void HandleMovement()
    {
        if (currentDashTime <= 0)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, baseSpeed * currentInput, walkAcceleration * Time.deltaTime);
        }
        transform.Translate(currentVelocity * Time.deltaTime);
    }

    void HandleCollisions()
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

    private void OnMove(InputValue value)
    {
        if (currentDashTime <= 0)
        {
            currentInput = value.Get<Vector2>();
        }
    }

    private void OnDash(InputValue value)
    {
        if (currentDashCoolDown <= 0)
        {
            currentVelocity = currentInput * baseSpeed * dashSpeedModifier;
            currentDashTime = dashTime;
            currentDashCoolDown = dashCooldownTime;
        }
    }
}
