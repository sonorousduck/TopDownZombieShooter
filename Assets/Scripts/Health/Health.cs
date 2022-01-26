using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Values")]
    public float maxHealth;
    private float currentHealth;

    [Header("Immunity")]
    public float immunityTime;
    private bool isImmune;

    [Header("Animator")]
    // This won't get used till we have animations
    public Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        isImmune = false;
    }

    public void Damage(float damage)
    {
        if (!isImmune)
        {
            currentHealth -= damage;
            Debug.Log(currentHealth);
            StartCoroutine(Immunity());
        }
        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            // Do damage animation
        }
    }

    private void Death()
    {
        // Do animation with animator here
        Destroy(gameObject);
    }

    IEnumerator Immunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityTime);
        isImmune = false;
    }
}
