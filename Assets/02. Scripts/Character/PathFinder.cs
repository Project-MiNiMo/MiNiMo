using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _rayDistance = 1.5f;
    [SerializeField] private LayerMask _layerMask;

    [Tooltip("Weight for inertia effect")]
    [SerializeField] private float _inertiaWeight = 1.0f;
    [Tooltip("Threshold for oscillation detection")]
    [SerializeField] private float _oscillationThreshold = 0.1f;
    [Tooltip("Time threshold for forced direction change during oscillation")]
    [SerializeField] private float _stuckTimeThreshold = 0.5f;

    private float _objectSize;
    private Vector2 _lastDirection;
    private Vector2 _lastPosition;
    private float _stuckTime = 0f;

    private readonly Vector2[] _directions = {
        RotateVector(Vector2.left, 30).normalized,
        RotateVector(new Vector2(0.5f, Mathf.Sqrt(3) / 2), 30).normalized,
        RotateVector(new Vector2(-0.5f, Mathf.Sqrt(3) / 2), 30).normalized,
        RotateVector(Vector2.right, 30).normalized,
        RotateVector(new Vector2(0.5f, -Mathf.Sqrt(3) / 2), 30).normalized,
        RotateVector(new Vector2(-0.5f, -Mathf.Sqrt(3) / 2), 30).normalized
    };

    private Coroutine _pathfindingCoroutine;

    private void Start()
    {
        _lastPosition = transform.position;

        var collider = GetComponent<CircleCollider2D>();
        _objectSize = collider.radius * 2;

        SetTarget(_target);
    }

    private static Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float x = v.x * cos - v.y * sin;
        float y = v.x * sin + v.y * cos;
        return new Vector2(x, y).normalized;
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;

        if (_pathfindingCoroutine != null)
        {
            StopCoroutine(_pathfindingCoroutine);
        }
        if (_target != null)
        {
            _pathfindingCoroutine = StartCoroutine(UpdatePathfinding());
        }
    }

    private IEnumerator UpdatePathfinding()
    {
        var waitTime = new WaitForSeconds(0.05f);

        while (_target != null)
        {
            var interest = CalculateInterest();

            MoveInBestDirection(interest);
            DetectOscillation(interest);

            // Check if target is reached
            if (Vector2.Distance(transform.position, _target.position) < 0.1f)
            {
                _target = null;
            }

            yield return waitTime;
        }
    }

    /// <summary>
    /// Calculate interest and danger
    /// </summary>
    private float[] CalculateInterest()
    {
        float[] interest = new float[_directions.Length];
        Vector2 targetDirection = (_target.position - transform.position).normalized;

        for (int i = 0; i < _directions.Length; i++)
        {
            Vector2 direction = _directions[i];
            interest[i] = IsObstacleInDirection(direction, _rayDistance + _objectSize)
                          ? float.NegativeInfinity
                          : CalculateInterestValue(targetDirection, direction);
        }
        return interest;
    }

    /// <summary>
    /// Check if there is an obstacle in the specified direction
    /// </summary>
    private bool IsObstacleInDirection(Vector2 direction, float distance)
    {
        Vector2 origin = (Vector2)transform.position + direction * (_objectSize / 2);
        return Physics2D.Raycast(origin, direction, distance, _layerMask).collider != null;
    }

    /// <summary>
    /// Calculate interest based on target direction and current direction
    /// </summary>
    private float CalculateInterestValue(Vector2 targetDirection, Vector2 direction)
    {
        float interest = Vector2.Dot(targetDirection, direction);
        float alignmentWithLastDirection = Vector2.Dot(_lastDirection, direction);
        return interest + _inertiaWeight * (1 + alignmentWithLastDirection);
    }

    /// <summary>
    /// Move in the best direction based on interest and danger
    /// </summary>
    private void MoveInBestDirection(float[] interest)
    {
        int bestDirectionIndex = -1;
        float highestInterest = float.NegativeInfinity;

        for (int i = 0; i < _directions.Length; i++)
        {
            if (interest[i] > highestInterest)
            {
                bestDirectionIndex = i;
                highestInterest = interest[i];
            }
        }

        // Move in the best direction
        if (bestDirectionIndex != -1)
        {
            Vector2 finalDirection = _directions[bestDirectionIndex];
            transform.position += _speed * Time.deltaTime * (Vector3)finalDirection;
            _lastDirection = finalDirection;
        }
    }

    /// <summary>
    /// Detect and compensate for oscillation
    /// </summary>
    private void DetectOscillation(float[] interest)
    {
        Vector2 currentPosition = transform.position;
        float distanceMoved = Vector2.Distance(currentPosition, _lastPosition);

        if (distanceMoved < _oscillationThreshold)
        {
            _stuckTime += Time.deltaTime;

            if (_stuckTime > _stuckTimeThreshold)
            {
                int newDirectionIndex = -1;
                float bestWeight = float.NegativeInfinity;

                for (int i = 0; i < _directions.Length; i++)
                {
                    if (interest[i] > float.NegativeInfinity && Vector2.Dot(_lastDirection, _directions[i]) > -0.5f && interest[i] > bestWeight)
                    {
                        bestWeight = interest[i];
                        newDirectionIndex = i;
                    }
                }

                // If can't choose a new direction, just choose the direction with the highest interest value.
                if (newDirectionIndex == -1)
                {
                    newDirectionIndex = System.Array.IndexOf(interest, interest.Max());
                }

                _lastDirection = _directions[newDirectionIndex];
                _stuckTime = 0;
            }
        }
        else
        {
            _stuckTime = 0;
        }

        _lastPosition = currentPosition;
    }
}