using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float animSpeed = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 2f;
    public float attackCooldown = 2f; // Add a cooldown between attacks
    public float hp = 10f;
    public GameObject[] bloodPrefab;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canAttack = true; // Flag to check if the enemy can attack

    private void GetTarget()
    {
        GameObject[] ivies = GameObject.FindGameObjectsWithTag("Ivy");

        if (ivies.Length > 0)
        {
            GameObject nearestIvy = FindNearestIvy(ivies);
            target = nearestIvy.transform;
        }
        else
        {
            Debug.Log("No Ivy found!");
        }
    }

    private GameObject FindNearestIvy(GameObject[] ivies)
    {
        GameObject nearestIvy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject ivy in ivies)
        {
            float distance = Vector2.Distance(transform.position, ivy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIvy = ivy;
            }
        }

        return nearestIvy;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject!");
        }
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }
        else
        {
            if (canAttack && AttackTarget() != null)
            {
                StartCoroutine(AttackCooldown());
            }
            else
            {
                string animationDirection = GetAnimationDirection();
                animator.SetTrigger(animationDirection);
                MoveTowardsTarget(moveSpeed);
            }
        }
    }

    private string AttackTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            // Trigger attack animation
            animator.speed = attackSpeed;
            return "Attack";
        }
        else return null;
    }

    private void MoveTowardsTarget(float speed)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private string GetAnimationDirection()
    {
        if (target == null)
        {
            return "Idle";
        }

        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Adjust the angle to be between 0 and 360 degrees
        angle = (angle + 360) % 360;
        // Check for attack range
        if (angle >= 0 && angle < 180)
        {
            animator.speed = animSpeed;
            return "RunUp";
        }
        else if (angle >= 181 && angle < 359)
        {
            animator.speed = animSpeed;
            return "RunDown";
        }
        else
        {
            return "";
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Get a random index within the array bounds
            int randomIndex = Random.Range(0, bloodPrefab.Length);

            // Spawn blood splatter at the hit position using the randomly selected blood prefab
            GameObject bloodInstance = Instantiate(bloodPrefab[randomIndex], collision.contacts[0].point, Quaternion.identity);

            // Trigger the "Splatter" animation if the blood prefab has an Animator component
            Animator bloodAnimator = bloodInstance.GetComponent<Animator>();
            if (bloodAnimator != null)
            {
                bloodAnimator.SetTrigger("Splatter");
            }

            // Handle damage or other actions as needed
            HandleDamage(10);

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }

    private void HandleDamage(float damage)
    {
        // Implement actions to handle damage
        hp -= damage;
        if (hp <= 0)
        {
            animator.SetTrigger("Dead");

            // Destroy the enemy gameObject after the "Dead" animation is played
            Destroy(gameObject, 2f); // Assuming "Dead" animation duration is 2 seconds
        }
        // For example, play hurt animations, reduce health, etc.
    }
}