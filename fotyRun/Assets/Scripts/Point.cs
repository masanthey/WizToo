using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Point : MonoBehaviour
{
    public static Action PointsAdd;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PointsAdd?.Invoke();
        Destroy(gameObject);
    }
}
