using UnityEngine;
using System;

public class EndPoint : MonoBehaviour
{
    public static Action PlayerLeftTheRoom;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerLeftTheRoom?.Invoke();
    }
}
