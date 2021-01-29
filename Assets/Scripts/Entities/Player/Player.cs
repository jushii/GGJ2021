using System;
using DG.Tweening;
using UnityEngine;

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
    public float punchHitBoxOffset = 1.0f;

    private Collider2D[] punchedNPCs = new Collider2D[10];
    private Collider2D[] nearbyNPCs = new Collider2D[5];
    private LayerMask mask;
    private Vector3 point;
    private Vector2 size;
    private int _punchFrameTime = 2;
    private int _punchFrameTimer;

    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(this);
    }

    public void SetInputVector(Vector2 dir)
    {
        _inputVector = dir;

        if (dir.magnitude != 0.0f)
        {
            lookVector = _inputVector;
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
        // point = transform.position + transform.right * (punchHitboxHorizontal/2 + punchHitBoxOffset);
        // size = new Vector2(punchHitboxHorizontal, punchHitboxVertical);
        // punchedNPCs = Physics2D.OverlapCapsuleAll(point, size, CapsuleDirection2D.Horizontal, 0f, mask);
        Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(playerPos2D, playerPos2D + lookVector);
        
        int punchedNpcCount = Physics2D.OverlapBoxNonAlloc(playerPos2D + new Vector2(lookVector.x, lookVector.y) * punchHitBoxOffset, Vector2.one * 2.5f, angle, punchedNPCs, mask);

        if (punchedNpcCount > 0)
        {
            // _rb2d.MovePosition(playerPos2D + _lookVector * 0.1f);

            for (int i = 0; i < punchedNpcCount; i++)
            {
                if (punchedNPCs[i].TryGetComponent(out NPC npc))
                {
                    npc.OnPunch();
                }
            }

            // _cameraShakeTween?.Kill();
            // _cameraShakeTween = camera.transform.DOShakePosition(0.25f, Vector3.one * 0.025f, 1, 50.0f, false, true).OnKill(() =>
            // {
            //     camera.transform.localPosition = Vector3.zero;
            // });
        }
    }

    private void UpdateMovementSpeed()
    {
        int layerMask = 1 << LayerMask.NameToLayer("NPC");
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, nearbyNPCs, layerMask);

        if (_punchFrameTimer > 0)
        {
            currentMaxSpeed = 0.0f;
            return;
        }
        
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

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;

        // Gizmos.DrawWireCube(transform.position + new Vector3(_lookVector.x, _lookVector.y, 0.0f) * punchHitBoxOffset, Vector3.one * 2.0f);
    }
}
