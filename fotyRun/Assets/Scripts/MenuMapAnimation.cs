using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMapAnimation : MonoBehaviour
{
    private float speed;
    private Vector3 destPos;
    private bool isMoving;

    void FixedUpdate()
    {
        if (isMoving == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destPos, step);
            if (transform.position == destPos) isMoving = false;
        }
        else
        {
            speed = 0.5f;
            destPos = new Vector3(Random.Range(-3f, 5f), Random.Range(-3f, 5f), Random.Range(-3f, 5f));
            isMoving = true;
        }
    }
}
