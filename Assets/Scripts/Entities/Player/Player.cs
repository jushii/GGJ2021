using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    
    private Vector3 _moveDirection = Vector2.zero;
    private Vector2 _inputVector = Vector2.zero;
    private Rigidbody2D _rb2d;
    
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
        _moveDirection = new Vector3(_inputVector.x, _inputVector.y, 0.0f);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= moveSpeed;
        _rb2d.AddForce(_moveDirection, ForceMode2D.Force);
    }
}
