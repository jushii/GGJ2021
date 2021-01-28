using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10.0f;
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

    private void Update()
    {
        _rb2d.AddForce(_inputVector * moveSpeed, ForceMode2D.Force);
    }
}
