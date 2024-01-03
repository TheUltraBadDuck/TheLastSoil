using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public int currentLevel = 0;
    public string[] treeLevel = { "Basic", "Evolution", "Legendary" };

    public Transform target;
    public float damage = 1f;
    public float moveSpeed = 0.1f;
    private float animSpeed = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 2f;
    public float hp = 10f;
    public AudioSource hurtSound;
    public AudioSource deadSound;
    public GameObject[] bloodPrefab;
    public GameObject bloodParticle;
    //public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MapManager mapManager;
    private float distanceToHoffen = 0f;
    private IvyInterface targetTree; // Change the type to IvyInterface
    private bool canAttack = true;
    private float attackCooldown = 0f;
    private void GetTarget()
    {
        IvyInterface[] ivies = GameObject.FindObjectsOfType<IvyInterface>();

        if (ivies.Length > 0)
        {
            IvyInterface nearestIvy = FindNearestIvy(ivies);
            targetTree = nearestIvy;
            target = targetTree.transform;
        }
        else
        {
            // If no ivies found, target to the "Hoffen" object
            GameObject hoffen = GameObject.FindGameObjectWithTag("Hoffen");
            target = hoffen != null ? hoffen.transform : null;
        }
    }

    private IvyInterface FindNearestIvy(IvyInterface[] ivies)
    {
        IvyInterface nearestIvy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (IvyInterface ivy in ivies)
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
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
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
        if (gameObject == null)
            return;

        if (hp <= 0)
        {
            MoveTowardsTarget(0);
        }
        else
        {
            GetTarget();

            if (target != null)
            {
                if (canAttack && AttackTarget() != null)
                {
                    // Only initiate attack if the cooldown is over
                    if (attackCooldown <= 0f)
                    {
                        MoveTowardsTarget(0);
                        animator.SetTrigger(AttackTarget());
                        targetTree.BeAttacked(damage);
                        if(targetTree is Cactus)
                        {
                            HandleDamage(damage);
                        }

                        // Apply cooldown
                        canAttack = false;
                        attackCooldown = 1f / attackSpeed; // Set cooldown duration based on attack speed
                    }
                }
                else
                {
                    // Check if not already in attack range before moving
                    if (Vector2.Distance(transform.position, target.position) > attackRange)
                    {
                        string animationDirection = GetAnimationDirection();
                        animator.SetTrigger(animationDirection);
                        MoveTowardsTarget(moveSpeed);
                    }
                }

                // Update cooldown timer
                if (!canAttack)
                {
                    attackCooldown -= Time.deltaTime;
                    if (attackCooldown <= 0f)
                    {
                        canAttack = true;
                    }
                }
            }
        }
    }


    private string AttackTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            return "Attack";
        }
        else return null;
    }

    public void GetDamageRaw(float damage)
    {
        GameObject blood = bloodParticle;
        Instantiate(blood, transform.position, Quaternion.identity);
        HandleDamage(damage);
    }

    private IEnumerator DealDamageAfterAnimation()
    {
        float animationLength = GetAttackAnimationLength();
        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(animationLength);

        // Deal damage to the target after the animation is played
        if (targetTree != null && Vector2.Distance(transform.position, targetTree.transform.position) <= attackRange)
        {
            targetTree.BeAttacked(damage);
        }

        // Reset the canAttack flag for the next attack
        canAttack = true;
    }

    private float GetAttackAnimationLength()
    {
        // Replace "Attack" with the actual name of your attack animation state
        int attackAnimationHash = Animator.StringToHash("Attack");

        // Get the length of the current attack animation
        float animationLength = 0f;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.shortNameHash == attackAnimationHash)
        {
            animationLength = stateInfo.length;
        }

        return animationLength;
    }

    private void MoveTowardsTarget(float speed)
    {
        if (hp > 0) // Only move if not dead
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += speed * direction *Time.deltaTime;
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
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Hit");
            if (bloodPrefab.Length > 0)
            {
                // Get a random index within the array bounds
                int randomIndex = Random.Range(0, bloodPrefab.Length);

                // Spawn blood splatter at the hit position using the randomly selected blood prefab
                GameObject bloodInstance = Instantiate(bloodPrefab[randomIndex], collision.transform.position, Quaternion.identity);

                // Trigger the "Splatter" animation if the blood prefab has an Animator component
                Animator bloodAnimator = bloodInstance.GetComponent<Animator>();
                if (bloodAnimator != null)
                {
                    bloodAnimator.SetTrigger("Splatter");
                }
            }

            GameObject blood = bloodParticle;
            Instantiate(blood, collision.transform.position, Quaternion.identity);
            // Handle damage or other actions as needed
            HandleDamage(collision.gameObject.GetComponent<BuffectExplosion>().getDamage());

        }
    }

    private void HandleDamage(float damage)
    {
        // Implement actions to handle damage
        Debug.Log(damage);
        hp -= damage;

        //turn the sprite to red for a moment
        StartCoroutine(FlashRed());
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
        else
        {
            hurtSound.Play();
        }
    }

    private IEnumerator FlashAndDestroy()
    {
        //mapManager.RemoveEnemyDetection(this);

        float flashDuration = 0.25f; // Adjust the duration of each flash
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
    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;

        // Set the sprite color to red
        spriteRenderer.color = Color.red;

        // Wait for a short duration
        yield return new WaitForSeconds(0.25f); // Adjust the duration as needed

        // Restore the original color
        spriteRenderer.color = originalColor;
    }


    public float GetDistanceToHoffen()
    {
        return distanceToHoffen;
    }


    public float GetDistance(IvyInterface tree)
    {
        return Vector3.Distance(transform.position, tree.transform.position);
    }
}