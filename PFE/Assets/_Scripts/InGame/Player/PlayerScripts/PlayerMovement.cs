using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMain _main;

    [Header("References")]
    [SerializeField] CharacterController _charController;

    [Header("Parameters")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _mouseSensitivity = 1f;

    Vector3 _moveVector;
    Vector2 _lookVector;

    float xRotation;

    private void Awake()
    {
        TryGetComponent(out _charController);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(_moveVector != Vector3.zero)
        {
            _charController.Move(_moveVector * _moveSpeed * Time.deltaTime);
        }
            
        if(_lookVector != Vector2.zero)
        {
            float xLook = _lookVector.x * _mouseSensitivity * Time.deltaTime;
            float yLook = _lookVector.y * _mouseSensitivity * Time.deltaTime;

            xRotation -= yLook;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            _main.playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * xLook);

        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 ctxValue = ctx.ReadValue<Vector2>();

        _moveVector = transform.right * ctxValue.x + transform.forward * ctxValue.y;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        _lookVector = ctx.ReadValue<Vector2>();
    }
}
