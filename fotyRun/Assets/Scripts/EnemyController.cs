using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed = 5.0f;
    public float CurrentStep;
    public float DefaultStep;
    public float RageStep;

    private bool _isMoving = false;
    public bool Ragemode = false;

    public LayerMask LayersDetect;
    public LayerMask EnemyLayer;

    private Vector3 lastStepDirection;
    private Vector3 direction;
    private Vector3 destPos;

    public Transform Target;
    public PlayerMovement _Target;


    void Update()
    {
        if (_isMoving == true)
        {
            float step = speed * CurrentStep * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destPos, step);

            if (transform.position == destPos) 
                _isMoving = false;
        }
        if (_Target.isMoving == true && _isMoving == false)
        {
            StepDirection();
            _isMoving = true;
        }
    }

    private RageModeInfo IsPlayerDetected() // определяет находится ли игрок в прямой видимости, возврящает информацию иб этом
    {
        var _RageModeInfo = new RageModeInfo();

        RaycastHit2D hitR = Physics2D.Raycast(transform.position, Vector3.right, 20f, LayersDetect);
        RaycastHit2D hitU = Physics2D.Raycast(transform.position, Vector3.up, 20f, LayersDetect);
        RaycastHit2D hitD = Physics2D.Raycast(transform.position, Vector3.down, 20f, LayersDetect);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, Vector3.left, 20f, LayersDetect);

        if (hitR.collider != null)
        {
            if (hitR.collider.CompareTag("Player"))
            {
                _RageModeInfo.Direction = Vector3.right;
                _RageModeInfo.Distace = Vector3.Distance(transform.position, Target.position);
                _RageModeInfo.Trigger = true;

                return _RageModeInfo;
            }
        }
        if (hitL.collider != null)
        {
            if (hitL.collider.CompareTag("Player"))
            {
                _RageModeInfo.Direction = Vector3.left;
                _RageModeInfo.Distace = Vector3.Distance(transform.position, Target.position);
                _RageModeInfo.Trigger = true;

                return _RageModeInfo;
            }
        }
        if (hitD.collider != null)
        {
            if (hitD.collider.CompareTag("Player"))
            {
                _RageModeInfo.Direction = Vector3.down;
                _RageModeInfo.Distace = Vector3.Distance(transform.position, Target.position);
                _RageModeInfo.Trigger = true;

                return _RageModeInfo;
            }
        }
        if (hitU.collider != null)
        {
            if (hitU.collider.CompareTag("Player"))
            {
                _RageModeInfo.Direction = Vector3.up;
                _RageModeInfo.Distace = Vector3.Distance(transform.position, Target.position);
                _RageModeInfo.Trigger = true;

                return _RageModeInfo;
            }
        }
            return _RageModeInfo;      
    }


    public void StepDirection() // просчитывает какой и куда нужно сделать шаг
    {
        RageModeInfo _RageModeInfo = IsPlayerDetected();

        if (_RageModeInfo.Trigger) // во время агрессивного мода
        {
            if (_RageModeInfo.Distace >= RageStep) // если игрок далеко
            {
                lastStepDirection =  direction;
                CurrentStep = RageStep;
                destPos = transform.position + _RageModeInfo.Direction * RageStep;
            }
            else // если игрок близко
            {
               var hit = Physics2D.Raycast( _Target.destPos, -1*_RageModeInfo.Direction, _RageModeInfo.Distace + 1f, EnemyLayer);

                if (hit.collider != null) // атака 
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        lastStepDirection = -1 * direction;
                        CurrentStep = _RageModeInfo.Distace + 1f;
                        destPos = _Target.destPos;

                    }
                    else // следуем за играком
                    {
                        lastStepDirection = -1 * direction;
                        CurrentStep = _RageModeInfo.Distace;
                        destPos = _Target.lastDestPos;
                    }
                }
                else // исключение
                {
                    lastStepDirection = -1 * direction;
                    CurrentStep = _RageModeInfo.Distace;
                    destPos = _Target.lastDestPos;
                }
            }
        }
        else // обычный шаг
        {
            List<Vector3> availableDirection = new List<Vector3>(); // содержит возможные направления движения

            if (DistanceToWall(Vector3.up) > 1)
                availableDirection.Add(Vector3.up);

            if (DistanceToWall(Vector3.down) > 1)
                availableDirection.Add(Vector3.down);

            if (DistanceToWall(Vector3.right) > 1)
                availableDirection.Add(Vector3.right);

            if (DistanceToWall(Vector3.left) > 1)
                availableDirection.Add(Vector3.left);

            if (lastStepDirection != null)
            {
                if (availableDirection.Count > 1)
                    availableDirection.Remove(lastStepDirection);
            }

            int random = Random.Range(0, availableDirection.Count);

            direction = availableDirection[random];
            lastStepDirection = -1 * direction;
            CurrentStep = DefaultStep;
            destPos = transform.position + direction * DefaultStep;
        }
    }

    public float DistanceToWall(Vector3 Direction) // возвращает дистанцию до стены в направлении Vector3 Direction 
    {
        RaycastHit2D hit;
        float Distance = 1f;

        for (int i = 0; i < 20; i++)
        {
            hit = Physics2D.Raycast(transform.position, Direction, Distance, LayersDetect);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Finish"))
                    return Distance;
            }
            Distance = Distance + 1f;

        }
        return Distance;
    }
    
}
public class RageModeInfo //вспомогательный класс
{
    public Vector3 Direction;
    public float Distace;
    public bool Trigger;
}


