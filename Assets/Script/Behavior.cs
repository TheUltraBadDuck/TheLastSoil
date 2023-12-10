using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3f;
    public float animSpeed = 1f;
    //public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    private Animator animator;

    private void GetTarget()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            target = playerObject.transform;
            Debug.Log("Player found!");
        }
        else
        {
            Debug.Log("Player not found!");
        }
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
        rb = GetComponent<Rigidbody2D>();
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
        } else
        {
            string animationDirection = GetAnimationDirection();
            animator.SetTrigger(animationDirection);
            MoveTowardsTarget();
        }
        
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        animator.speed = animSpeed;
        rb.velocity = direction * moveSpeed;
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

        if (angle >= 45 && angle < 135)
        {
            return "RunUp";
        }
        else if (angle >= 225 && angle < 315)
        {
            return "RunDown";
        }
        else
        {
            return "RunSide";
        }
    }
}
