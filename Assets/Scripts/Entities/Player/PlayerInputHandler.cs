using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _playerInput;
    // private InputAction _moveAction;
    // private InputAction _lightPunchAction;

    private void Awake()
    {
        // _player = GetComponent<Player>();
        // _playerInput = GetComponent<PlayerInput>();


        // _moveAction = _playerInput.currentActionMap.FindAction("Move");
        // _lightPunchAction = _playerInput.currentActionMap.FindAction("LightPunch");
        //
        // _moveAction.performed += OnMove;
        // _lightPunchAction.performed += OnLightPunch;
    }
    
    // public void OnTeleport()
    // {
    //     transform.position = new Vector3(Random.Range(-75, 75), 0.5f, Random.Range(-75, 75));
    // }

    public void OnLightPunch(InputAction.CallbackContext context)
    {
        _player.Punch();
    }

    // public void OnMove()
    // {
    //     Debug.Log("lol");
    // }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _player.SetInputVector(context.ReadValue<Vector2>());    
    }
}
