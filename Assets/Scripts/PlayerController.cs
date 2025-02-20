using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float mouseSensitivity;

    private bool moving;
    private Rigidbody _rb;
    private Vector2 _moveInput;
    public CinemachineCamera _camera;

    // Verifica si el jugador está en el suelo
    private bool IsGrounded() { return Physics.Raycast(transform.position, Vector3.down, 1.1f); }

    // Mueve al jugador
    void Move()
    {
        transform.Translate(new Vector3(_moveInput.x * Time.deltaTime * speed, 0, _moveInput.y * Time.deltaTime * speed));
        if (_moveInput == Vector2.zero)
        {
            moving = false;
        }
    }

    // Recibe entrada de movimiento del jugador
    public void onMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        moving = true;
        _moveInput = value;
        Debug.Log(_moveInput);
    }

    // Aplica el salto si el jugador está en el suelo
    public void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded()) _rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
    }

    // Rotación del jugador y la cámara
    public void Look(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        transform.RotateAround(transform.position, Vector3.up, value.x * mouseSensitivity);

        // Limita la rotación de la cámara vertical
        float newRotationX = _camera.transform.localEulerAngles.x - value.y * mouseSensitivity;
        if (newRotationX > 180) newRotationX -= 360;
        newRotationX = Mathf.Clamp(newRotationX, -80f, 80f);

        _camera.transform.localRotation = Quaternion.Euler(newRotationX, 90f, 0f);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Bloquea el cursor en el centro de la pantalla
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (moving)
        {
            Move();
        }
    }
}
