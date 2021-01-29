using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public enum LookDirection
{
    Right,
    TopRight,
    Top,
    TopLeft,
    Left,
    BottomLeft,
    Bottom,
    BottomRight,
    None,
}

public class Player : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float currentMaxSpeed;
    public float moveSpeed = 10.0f;
    private Vector2 _inputVector = Vector2.zero;
    public Vector2 lookVector = Vector2.zero;
    private Vector2 _velocity = Vector2.zero;
    public Rigidbody2D _rb2d;
    public Camera camera;
    public GameObject cameraHolder;
    private Tweener _cameraShakeTween;
    
    public float punchHitboxHorizontal = 1f;
    public float punchHitboxVertical = 5f;
    public float punchHitBoxOffset = 1.255f;

    private Collider2D[] punchedNPCs = new Collider2D[30];
    private Collider2D[] nearbyNPCs = new Collider2D[5];
    private LayerMask mask;
    private int _punchFrameTime = 1;
    private int _punchFrameTimer;
    
    private Animator _animator;
    private AnimatorControllerParameter _tmpParameter;
    private List<int> _cachedParameterIds = new List<int>();

    private float _lookAngle = 0.0f;        // player's looking direction angle in radian ranged from 0 to 2*Pi
    private float _angleDivider = 45.0f;
    private LookDirection _lookDirection = LookDirection.Right;
    public List<Kid> followers = new List<Kid>();
    public List<Transform> followerPositions;
    private Dictionary<Kid, Transform> _reservedFollowerPositions = new Dictionary<Kid, Transform>();
    public Transform followerPositionsPivot;
    
    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(this);

        _animator = GetComponent<Animator>();
        CacheAnimatorParameterIds();
    }

    public void SetInputVector(Vector2 dir)
    {
        _inputVector = dir;

        if (dir.magnitude != 0.0f)
        {
            lookVector = _inputVector;
        }
        else
        {
            lookVector = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        UpdateMovementSpeed();
        UpdatePunchFrameTimer();

        _velocity = _inputVector * moveSpeed;
        _rb2d.AddForce(_inputVector * moveSpeed, ForceMode2D.Force);
        
        if (_rb2d.velocity.magnitude > currentMaxSpeed)
        {
            _rb2d.velocity = Vector2.ClampMagnitude(_rb2d.velocity, currentMaxSpeed);
        }
    }

    public void Punch()
    {
        _punchFrameTimer = _punchFrameTime;
        
        mask = 1 << LayerMask.NameToLayer("NPC");
        mask |= 1 << LayerMask.NameToLayer("PromotionGuy");
        Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(playerPos2D, playerPos2D + lookVector);
        
        int punchedNpcCount = Physics2D.OverlapBoxNonAlloc(playerPos2D + new Vector2(lookVector.x, lookVector.y) * punchHitBoxOffset, Vector2.one * 2.5f, angle, punchedNPCs, mask);

        if (punchedNpcCount > 0)
        {
            for (int i = 0; i < punchedNpcCount; i++)
            {
                if (punchedNPCs[i].TryGetComponent(out NPC npc))
                {
                    npc.OnPunch();
                }
            }
        }
    }

    private void UpdateMovementSpeed()
    {
        int layerMask = 1 << LayerMask.NameToLayer("NPC");
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, nearbyNPCs, layerMask);

        // if (_punchFrameTimer > 0)
        // {
        //     currentMaxSpeed = 0.0f;
        //     return;
        // }
        
        if (count > 0)
        {
            currentMaxSpeed = 1.0f;
            return;
        }

        currentMaxSpeed = maxSpeed;
    }

    private void UpdatePunchFrameTimer()
    {
        _punchFrameTimer = Mathf.Clamp(_punchFrameTimer - 1, 0, _punchFrameTime);
    }

    public void RunAnimation()
    {
        _animator = GetComponent<Animator>();
        if(lookVector != Vector2.zero)
        {
            _lookAngle = Mathf.Acos(Vector2.Dot(Vector2.right, lookVector) / lookVector.magnitude);
            if(lookVector.y < 0)
            {
                _lookAngle = 2 * Mathf.PI - _lookAngle;
            }

            _lookDirection = (LookDirection) Mathf.RoundToInt(_lookAngle * Mathf.Rad2Deg / _angleDivider);
        }
        else
        {
            _lookDirection = LookDirection.None;
        }

        //Debug.Log(_lookDirection);
        switch (_lookDirection)
        {
            case LookDirection.Left:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunLeft");
                break;
            case LookDirection.Right:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunRight");
                break;
            case LookDirection.None:
                ResetAnimatorTriggers();
                _animator.SetTrigger("Idle");
                break;
        }
    }

    private void CacheAnimatorParameterIds()
    {
        for (int i = 0; i < _animator.parameters.Length; i++)
        {
            _tmpParameter = _animator.parameters[i];
            if (_tmpParameter.type == AnimatorControllerParameterType.Trigger)
            {
                _cachedParameterIds.Add(Animator.StringToHash(_tmpParameter.name));
            }
        }
    }

    private void ResetAnimatorTriggers()
    {
        foreach (int id in _cachedParameterIds)
        {
            _animator.ResetTrigger(id);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(transform.position + new Vector3(_lookVector.x, _lookVector.y, 0.0f) * punchHitBoxOffset, Vector3.one * 2.0f);
    }

    public void AddFollower(Kid kid)
    {
        followers.Add(kid);
    }

    public void RemoveFollower(Kid kid)
    {
        followers.Remove(kid);
    }
    
    public Transform GetKidFollowTarget(Kid kid)
    {
        if (_reservedFollowerPositions.TryGetValue(kid, out Transform reservedPosition))
        {
            return reservedPosition;
        }

        Transform fPos = followerPositions[Random.Range(0, followerPositions.Count)];
        followerPositions.Remove(fPos);
        _reservedFollowerPositions[kid] = fPos;
        return fPos;

        // return transform;
        
        int kidIndex = followers.IndexOf(kid);

        if (kidIndex == 0)
        {
            return transform;
        }

        return followers[kidIndex - 1].transform;
    }

    public Kid GetPromotionGuyFollowTargetKid()
    {
        return followers[followers.Count - 1];
    }
}
