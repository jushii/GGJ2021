using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    private Vector2 _inputVector = Vector2.zero;
    private Rigidbody2D _rb2d;

    public float punchHitboxHorizontal = 1f;
    public float punchHitboxVertical = 5f;
    public float xOffset = 0.5f;

    private Collider2D[] punchedNPCs;
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

    private void Update()
    {
        _rb2d.AddForce(_inputVector * moveSpeed, ForceMode2D.Force);
    }

    public void Punch()
    {
        mask = LayerMask.GetMask("NPC");
        point = transform.position + transform.right * (punchHitboxHorizontal/2 + xOffset);
        size = new Vector2(punchHitboxHorizontal, punchHitboxVertical);
        punchedNPCs = Physics2D.OverlapCapsuleAll(point, size, CapsuleDirection2D.Horizontal, 0f, mask);
        foreach (Collider2D collider in punchedNPCs)
        {
            //Debug.Log("punch!");
            ServiceLocator.Current.Get<AIManager>().ChangeState(collider.GetComponent<NPC>(), typeof(Entities.NPC.States.Consumer_Fly));
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(transform.position + new Vector3(_inputVector.x, _inputVector.y, 0.0f) * xOffset, Vector3.one * 1.0f);
    }
}
