using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5.0f;
    public float CellSize = 1.0f;
    public bool isMoving = false;
    Vector3 direction;
    public Vector3 destPos;

    public static Action PlayerTouchEnemy;

    public List<Vector3> DirectionHistory = new List<Vector3>();
    public Vector3 lastDestPos;
    public LayerMask IgnorePlayer;

    Vector2 touchStartPosition;
    Vector2 touchEndPosition;
    float minSwipeDistance = 50f;

    void Update()
    {
        if (isMoving == true)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destPos, step);
            if (transform.position == destPos) isMoving = false;
        }

        else
        {          ///////// For PC teSt
            /*    if (Input.GetKeyDown(KeyCode.W) && DistanceToWall(Vector3.up) > 1)
                {
                    //move up
                    direction = Vector3.up;
                    destPos = transform.position + direction * CellSize;
                    isMoving = true;
                    lastDestPos = transform.position;
                    DirectionHistory.Add(direction);
                }
                else if (Input.GetKeyDown(KeyCode.A) && DistanceToWall(Vector3.left) > 1)
                {
                    //move left
                    direction = Vector3.left;
                    destPos = transform.position + direction * CellSize;
                    isMoving = true;
                    lastDestPos = transform.position;
                    DirectionHistory.Add(direction);
                }
                else if (Input.GetKeyDown(KeyCode.S) && DistanceToWall(Vector3.down) > 1)
                {
                    //move down
                    direction = Vector3.down;
                    destPos = transform.position + direction * CellSize;
                    isMoving = true;
                    lastDestPos = transform.position;
                    DirectionHistory.Add(direction);
                }
                else if (Input.GetKeyDown(KeyCode.D) && DistanceToWall(Vector3.right) > 1)
                {
                    //move right
                    direction = Vector3.right;
                    destPos = transform.position + direction * CellSize;
                    isMoving = true;
                    lastDestPos = transform.position;
                    DirectionHistory.Add(direction);
                }
            */

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = touch.position;
                    CheckSwipeDirection();
                }
            }
        }
    }

    public float DistanceToWall(Vector3 Direction)
    {
        RaycastHit2D hit;
        float Distance = 1f;
        for (int i = 0; i < 20; i++)
        {
            hit = Physics2D.Raycast(transform.position, Direction, Distance, IgnorePlayer);
            if (hit.collider != null)
            {
                //  Debug.Log(hit.collider);
                if (hit.collider.CompareTag("Wall"))
                {
                    return Distance;

                }
            }
            Distance++;

        }
        // Debug.Log(Distance);
        return Distance;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            PlayerTouchEnemy?.Invoke();
    }
    private void CheckSwipeDirection()
    {
        float swipeDistance = Vector2.Distance(touchStartPosition, touchEndPosition);
        if (swipeDistance > minSwipeDistance)
        {
            Vector2 swipeDirection = touchEndPosition - touchStartPosition;
            swipeDirection.Normalize();
            float dotProductLeft = Vector2.Dot(swipeDirection, Vector2.left);
            float dotProductRight = Vector2.Dot(swipeDirection, Vector2.right);
            float dotProductUp = Vector2.Dot(swipeDirection, Vector2.up);
            float dotProductDown = Vector2.Dot(swipeDirection, Vector2.down);
            if (dotProductLeft > 0.5f && DistanceToWall(Vector3.left) > 1)
            {
                // Left swipe detected
                direction = Vector3.left;
                destPos = transform.position + direction * CellSize;
                isMoving = true;
                lastDestPos = transform.position;
                DirectionHistory.Add(direction);
            }
            else if (dotProductRight > 0.5f && DistanceToWall(Vector3.right) > 1)
            {
                // Right swipe detected
                direction = Vector3.right;
                destPos = transform.position + direction * CellSize;
                isMoving = true;
                lastDestPos = transform.position;
                DirectionHistory.Add(direction);
            }
            else if (dotProductUp > 0.5f && DistanceToWall(Vector3.up) > 1)
            {
                // Up swipe detected
                direction = Vector3.up;
                destPos = transform.position + direction * CellSize;
                isMoving = true;
                lastDestPos = transform.position;
                DirectionHistory.Add(direction);
            }
            else if (dotProductDown > 0.5f && DistanceToWall(Vector3.down) > 1)
            {
                // Down swipe detected
                direction = Vector3.down;
                destPos = transform.position + direction * CellSize;
                isMoving = true;
                lastDestPos = transform.position;
                DirectionHistory.Add(direction);
            }
        }
    }
}
