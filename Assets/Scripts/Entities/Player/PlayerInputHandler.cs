using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void OnLightPunch(InputAction.CallbackContext context)
    {
        _player.Punch();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _player.SetInputVector(context.ReadValue<Vector2>());    
    }
}
