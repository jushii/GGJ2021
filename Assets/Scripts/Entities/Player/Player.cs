using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float currentMaxSpeed;
    public float moveSpeed = 10.0f;
    private Vector2 _inputVector = Vector2.zero;
    private Vector2 _velocity = Vector2.zero;
    private Rigidbody2D _rb2d;

    public float punchHitboxHorizontal = 1f;
    public float punchHitboxVertical = 5f;
    public float xOffset = 0.5f;

    private Collider2D[] punchedNPCs;
    private Collider2D[] nearbyNPCs = new Collider2D[5];
    private LayerMask mask;
    private Vector3 point;
    private Vector2 size;

    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(this);
        _rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetInputVector(Vector2 dir)
    {
        _inputVector = dir;
    }

    private void FixedUpdate()
    {
        UpdateMovementSpeed();
        
        _velocity = _inputVector * moveSpeed;
        _rb2d.AddForce(_inputVector * moveSpeed, ForceMode2D.Force);
        
        if (_rb2d.velocity.magnitude > currentMaxSpeed)
        {
            _rb2d.velocity = Vector2.ClampMagnitude(_rb2d.velocity, currentMaxSpeed);
        }
    }

    public void Punch()
    {
        mask = LayerMask.GetMask("NPC");
        point = transform.position + transform.right * (punchHitboxHorizontal/2 + xOffset);
        size = new Vector2(punchHitboxHorizontal, punchHitboxVertical);
        punchedNPCs = Physics2D.OverlapCapsuleAll(point, size, CapsuleDirection2D.Horizontal, 0f, mask);
        
        foreach (Collider2D npcCollider in punchedNPCs)
        {
            if (npcCollider.TryGetComponent(out NPC npc))
            {
                npc.OnPunch();
            }
        }
    }

    private void UpdateMovementSpeed()
    {
        int layerMask = 1 << LayerMask.NameToLayer("NPC");
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, 1.0f, nearbyNPCs, layerMask);
        currentMaxSpeed = count > 0 ? 1.0f : maxSpeed;
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(transform.position + new Vector3(_inputVector.x, _inputVector.y, 0.0f) * xOffset, Vector3.one * 1.0f);
    }
}
