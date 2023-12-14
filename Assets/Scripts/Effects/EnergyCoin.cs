using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnergyCoin : MonoBehaviour
{
    private Camera _camera;
    private EnergyBar energyBar;
    public AnimationCurve animationCurve;
    private Animator animator;

    private float durationTime = 0.0f;
    private float collectTime = 0.0f;
    private bool collecting = false;
    private bool finishCollecting = false;
    private Vector3 oldPos = Vector3.zero;
    private Vector3 jumpVector = Vector3.zero;

    private void Start()
    {
        _camera = Camera.main;
        energyBar = GameObject.Find("UIGameplay").GetComponent<EnergyBar>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (finishCollecting)
            return;

        if (collecting)
        {
            collectTime += Time.deltaTime;
            if (collectTime <= 1.0f)
            {
                float speedOffset = animationCurve.Evaluate(collectTime / 1.0f);
                transform.position = new Vector3(oldPos.x + speedOffset * jumpVector.x, oldPos.y + speedOffset * jumpVector.y);
            }
            else
            {
                finishCollecting = true;
                energyBar.AddScore(25);
                animator.Play("CoinDisappear");
            }

        }
        else
        {
            durationTime += Time.deltaTime;
            if (durationTime > 10.0f)
            {
                animator.Play("CoinDisappear");
                durationTime = 0.0f;
            }
        }

    }

    //public void OnClick(InputAction.CallbackContext context)
    //{
    //    if (!context.started)
    //        return;

    //    var rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
    //    if (!rayHit.collider)
    //        return;

    //    oldPos = transform.position;
    //    jumpVector = _camera.ScreenToWorldPoint(energyBar.GetImage().transform.position) - oldPos;
    //    collecting = true;

    //    RaycastHit hit;
    //}

    private void OnMouseDown()
    {
        oldPos = transform.position;
        jumpVector = _camera.ScreenToWorldPoint(energyBar.GetImage().transform.position) - oldPos;
        collecting = true;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
