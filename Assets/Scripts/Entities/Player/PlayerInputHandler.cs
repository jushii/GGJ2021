using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void OnLightPunch(InputAction.CallbackContext context)
    {
        if (_player != null)
        {
            _player.Punch();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_player != null)
        {
            _player.SetInputVector(context.ReadValue<Vector2>());
            _player.RunAnimation();
        }
    }
}
