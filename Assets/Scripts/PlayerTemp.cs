using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerTemp : MonoBehaviour
{
    public float velocity = 2.5f;
    private float distance = 0.0f;

    public float hp = 10;

    private void Update()
    {
        distance = MapManager.GetDistanceToHoffen(this);

        // Vector3 direction = Vector3.zero;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, y, 0).normalized;
        transform.position += velocity * Time.deltaTime * direction;
    }

    public float GetDistance()
    {
        return distance;
    }


    public void BeAttacked(int damage)
    {
        hp -= damage;
        Debug.Log("Damage " + damage + " times. Remains HP: " + hp);
    }
}
