using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float animSpeed = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 2f;
    public float hp = 10f;
    public GameObject[] bloodPrefab;
    //public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
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
            // If no ivies found, target the "Hoffen" object
            GameObject hoffen = GameObject.FindGameObjectWithTag("Hoffen");
            target = hoffen != null ? hoffen.transform : null;

            if (target == null)
            {
                Debug.Log("No Ivy or Hoffen found!");
            }
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

    //private void RotateTowardsTarget()
    //{
    //    Vector2 targetDirection = target.position - transform.position;
    //    float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
    //    Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
    //    transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);

    //}

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject!");
        }

    }

    private void Update()
    {
         // Update the target to the nearest enemy
        if (hp <= 0)
        {
            MoveTowardsTarget(0);
        }
        else
        {
            GetTarget();
            if (target != null)
            {
                if (AttackTarget() != null)
                {
                    MoveTowardsTarget(0);
                    animator.SetTrigger(AttackTarget());
                }
                else
                {
                    string animationDirection = GetAnimationDirection();
                    animator.SetTrigger(animationDirection);
                    MoveTowardsTarget(moveSpeed);
                }
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
        if (hp > 0) // Only move if not dead
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Hit");
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
            HandleDamage(5);

        }
    }

    private void HandleDamage(float damage)
    {
        // Implement actions to handle damage
        hp -= damage;

        if (hp <= 0)
        {
            StartCoroutine(FlashAndDestroy());

            // Disable the collider to prevent further interactions
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Stop any ongoing animation and play the dead animation
            animator.speed = 0f;
            animator.Play("Dead");
        }
        // For example, play hurt animations, reduce health, etc.
    }

    private IEnumerator FlashAndDestroy()
    {
        float flashDuration = 0.4f; // Adjust the duration of each flash
        float flashInterval = 0.05f; // Adjust the interval between flashes

        while (true)
        {
            yield return new WaitForSeconds(flashInterval);

            // Toggle the visibility of the sprite
            spriteRenderer.enabled = !spriteRenderer.enabled;

            flashDuration -= flashInterval;
            if (flashDuration <= 0)
            {
                break; // Stop flashing after a certain duration
            }
        }

        // Destroy the entire GameObject (including the prefab)
        Destroy(gameObject);
    }
}