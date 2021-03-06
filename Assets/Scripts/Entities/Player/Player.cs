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

public enum PunchDirection
{
    Left,
    Right,
    Top,
    Bottom,
}

public class Player : MonoBehaviour
{
    public Vector2 lookVector = Vector2.zero;
    public Rigidbody2D _rb2d;
    public Camera camera;
    public GameObject cameraHolder;
    public float punchHitBoxSize = 5.0f;
    public float punchHitBoxSizeBig = 8.0f;
    public float maxSpeed = 10.0f;
    public float currentMaxSpeed;
    public float moveSpeed = 10.0f;
    public float punchHitBoxOffset = 3.0f;
    public List<Kid> followers = new List<Kid>();
    public List<Transform> followerPositions;
    public Transform followerPositionsPivot;
    public Observable<int> kidCount = new Observable<int>();
    
    private Vector2 _velocity = Vector2.zero;
    private Tweener _cameraShakeTween;
    private Collider2D[] punchedNPCs = new Collider2D[30];
    private Collider2D[] nearbyNPCs = new Collider2D[5];
    private LayerMask mask;
    private int _punchFrameTime = 1;
    private int _punchFrameTimer;
    private Vector2 _inputVector = Vector2.zero;
    private Animator _animator;
    private AnimatorControllerParameter _tmpParameter;
    private List<int> _cachedParameterIds = new List<int>();
    private Dictionary<Kid, Transform> _reservedFollowerPositions = new Dictionary<Kid, Transform>();
    private float _lookAngle = 0.0f;        // player's looking direction angle in radian ranged from 0 to 2*Pi
    private float _angleDivider = 45.0f;
    private LookDirection _lookDirection = LookDirection.Right;
    public PunchDirection punchDirection = PunchDirection.Top;
    
    // Punching logic!
    private bool _isPunchFreezeFrameActive = false;
    private bool _automaticPunchQueued = false;
    private int _punchComboCounter;
    private bool _isPunchAnimationPlaying;
    private List<NPC> _punchedNPCs = new List<NPC>();

    public Action onComboEnd;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        CacheAnimatorParameterIds();
    }

    public void SetInputVector(Vector2 dir)
    {
        if (GameManager.isGameOver) return;

        _inputVector = dir;

        if (dir.magnitude != 0.0f)
        {
            lookVector = _inputVector;
        }
        // else
        // {
        //     lookVector = Vector2.zero;
        // }
    }

    private void FixedUpdate()
    {
        if (GameManager.isGameOver) return;
        
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
        if (_isPunchFreezeFrameActive)
        {
            // If we press punch while we're punching (not finished with the punch) we can queue automatic punch.
            // This is a "feel good" thing.
            // Debug.Log("automatic punch queued");
            if (_punchComboCounter < 3)
            {
                _automaticPunchQueued = true;
            }
            return;
        }
        
        _isPunchAnimationPlaying = true;
        
        // Reset automatic queue.
        
        _isPunchFreezeFrameActive = true;
        
        // ResetAnimatorTriggers();

        switch (_punchComboCounter)
        {
            case 0:
            {
                switch (punchDirection)
                {
                    case PunchDirection.Bottom:
                        _animator.SetTrigger("Punch_0_Bottom");
                        break;
                    case PunchDirection.Top:
                        _animator.SetTrigger("Punch_0_Top");
                        break;
                    case PunchDirection.Left:
                        _animator.SetTrigger("Punch_0_Left");
                        break;
                    case PunchDirection.Right:
                        _animator.SetTrigger("Punch_0_Right");
                        break;
                }
                break;
            }
            case 1:
            {
                switch (punchDirection)
                {
                    case PunchDirection.Bottom:
                        _animator.SetTrigger("Punch_1_Bottom");
                        break;
                    case PunchDirection.Top:
                        _animator.SetTrigger("Punch_1_Top");
                        break;
                    case PunchDirection.Left:
                        _animator.SetTrigger("Punch_1_Left");
                        break;
                    case PunchDirection.Right:
                        _animator.SetTrigger("Punch_1_Right");
                        break;
                }
                break;
            }
            case 2:
            {
                switch (punchDirection)
                {
                    case PunchDirection.Bottom:
                        _animator.SetTrigger("Punch_2_Bottom");
                        break;
                    case PunchDirection.Top:
                        _animator.SetTrigger("Punch_2_Top");
                        break;
                    case PunchDirection.Left:
                        _animator.SetTrigger("Punch_2_Left");
                        break;
                    case PunchDirection.Right:
                        _animator.SetTrigger("Punch_2_Right");
                        break;
                }
                break;
            }
        }
        
        _automaticPunchQueued = false;
    }

    public void StartPunch()
    {
        Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.y);

        mask = 1 << LayerMask.NameToLayer("NPC");
        mask |= 1 << LayerMask.NameToLayer("PromotionGuy");
        float angle = Vector2.Angle(playerPos2D, playerPos2D + lookVector);
        
        int npcCount = Physics2D.OverlapBoxNonAlloc(playerPos2D + new Vector2(lookVector.x, lookVector.y) * punchHitBoxOffset, Vector2.one * punchHitBoxSize, angle, punchedNPCs, mask);

        if (npcCount > 0)
        {
            for (int i = 0; i < npcCount; i++)
            {
                if (punchedNPCs[i].TryGetComponent(out NPC npc))
                {
                    npc.OnStartPunch();
                    _punchedNPCs.Add(npc);
                }
            }
        }


        if (_punchComboCounter < 2)
        {
            ServiceLocator.Current.Get<AudioManager>().PlayLightPunchSFX();
        }
        else
        {
            ServiceLocator.Current.Get<AudioManager>().PlayHardPunchSFX();
        }
        
        _punchComboCounter++;
    }
    
    public void EndPunch()
    {
        _isPunchFreezeFrameActive = false;

        // We can now start second punch if we try to combo
        // (if we're too late to press punch, we will trigger EndPunchAnimation and can't combo anymore)
        foreach (NPC punchedNPC in _punchedNPCs)
        {
            punchedNPC.OnEndPunch();
        }
        
        _punchedNPCs.Clear();
        
        // If we have tapped punch button during the previous punch. Do automatic another punch.
        if (_automaticPunchQueued && _punchComboCounter < 3)
        {
            Punch();
        }
    }

    public void EndPunchAnimation()
    {
        // if (_punchComboCounter == 2)
        // {
        //     _cameraShakeTween.Kill();
        //     camera.transform.localPosition = Vector3.zero;
        //     _cameraShakeTween = camera.transform.DOShakePosition(0.25f, Vector3.one * 1.0f, 10, 90, false, true).OnKill(
        //         () =>
        //         {
        //             camera.transform.localPosition = Vector3.zero;
        //         });
        // }
        
        _isPunchFreezeFrameActive = false;
        _isPunchAnimationPlaying = false;
        
        // _animator.SetTrigger("Idle");
      
        RunAnimation();
        _punchComboCounter = 0;
        
        onComboEnd?.Invoke();
    }

    private void UpdateMovementSpeed()
    {
        int layerMask = 1 << LayerMask.NameToLayer("NPC");
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, nearbyNPCs, layerMask);

        if (_isPunchFreezeFrameActive)
        {
            currentMaxSpeed = 0.0f;
            return;
        }
        
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (!nearbyNPCs[i].GetComponent<NPC>().stunned)
                {
                    currentMaxSpeed = 1.0f;
                    return;
                }
            }
        }
        
        currentMaxSpeed = maxSpeed;
    }

    private void UpdatePunchFrameTimer()
    {
        _punchFrameTimer = Mathf.Clamp(_punchFrameTimer - 1, 0, _punchFrameTime);
    }

    public void RunAnimation()
    {
        if (_isPunchAnimationPlaying)
        {
            return;
        }
        
        _animator = GetComponent<Animator>();
        if(lookVector != Vector2.zero)
        {
            _lookAngle = Mathf.Acos(Vector2.Dot(Vector2.right, _inputVector) / _inputVector.magnitude);
            if(lookVector.y < 0)
            {
                _lookAngle = 2 * Mathf.PI - _lookAngle;
            }

            _lookDirection = (LookDirection) (Mathf.RoundToInt(_lookAngle * Mathf.Rad2Deg / _angleDivider) % 8);
        }
        else
        {
            _lookDirection = LookDirection.None;
        }

        // Debug.Log(_lookDirection);
        // Debug.Log("is freeze frame active " + _isPunchFreezeFrameActive);

        switch (_lookDirection)
        {
            case LookDirection.Left:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunLeft");
                punchDirection = PunchDirection.Left;
                break;
            case LookDirection.Right:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunRight");
                punchDirection = PunchDirection.Right;
                break;
            case LookDirection.Top:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunTop");
                punchDirection = PunchDirection.Top;
                break;
            case LookDirection.Bottom:
                ResetAnimatorTriggers();
                _animator.SetTrigger("RunBottom");
                punchDirection = PunchDirection.Bottom;
                break;
            case LookDirection.None:
                ResetAnimatorTriggers();
                _animator.SetTrigger("Idle");
                break;
            default:
            {
                switch (punchDirection)
                {
                    case PunchDirection.Left:
                        ResetAnimatorTriggers();
                        _animator.SetTrigger("RunLeft");
                        punchDirection = PunchDirection.Left;
                        break;
                    case PunchDirection.Right:
                        ResetAnimatorTriggers();
                        _animator.SetTrigger("RunRight");
                        punchDirection = PunchDirection.Right;
                        break;
                    case PunchDirection.Top:
                        ResetAnimatorTriggers();
                        _animator.SetTrigger("RunTop");
                        punchDirection = PunchDirection.Top;
                        break;
                    case PunchDirection.Bottom:
                        ResetAnimatorTriggers();
                        _animator.SetTrigger("RunBottom");
                        punchDirection = PunchDirection.Bottom;
                        break;
                }
                break;
            }
        }

        if (_inputVector.magnitude == 0.0f)
        {
            ResetAnimatorTriggers();
            _animator.SetTrigger("Idle");
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(lookVector.x, lookVector.y, 0.0f) * punchHitBoxOffset, Vector3.one * punchHitBoxSize);
    }

    public void AddFollower(Kid kid)
    {
        followers.Add(kid);
        kidCount.Value = followers.Count;
    }

    public void RemoveFollower(Kid kid)
    {
        followers.Remove(kid);
        kidCount.Value = followers.Count;
    }
    
    public Transform GetKidFollowTarget(Kid kid)
    {
        return transform;

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