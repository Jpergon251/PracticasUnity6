using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public float jumpForce = 3f;
    [SerializeField] public float mouseSensitivity = 0.1f;
    [SerializeField] public float objectDistanceDetection = 1f;
    
    private Rigidbody _rb;
    public CinemachineCamera _camera;
    
    private bool moving;
    public bool interacting;
    private Vector2 _moveInput;
    public GameObject _objectObserved;
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
        // Debug.Log(_moveInput);
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
        
        // Raycast desde la cámara hacia adelante
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, objectDistanceDetection)) // objectDistanceDetection es la distancia máxima para el raycast
        {
            // Si el raycast golpea algo, guardamos el objeto en _objectObserved
            _objectObserved = hit.collider.gameObject;

            // Aquí puedes agregar lógica para interactuar con el objeto si lo deseas
            // Debug.Log("Objeto observado: " + _objectObserved.name);
        }
        else
        {
            // Si no está mirando a nada, se limpia la referencia
            _objectObserved = null;
            // Debug.Log("No hay ningún objeto");
        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        
        // Si la acción está en el estado "Performed" (tecla presionada)
        if (context.started) 
        {
            if (_objectObserved)
            {
                interacting = true;
                if (_objectObserved.CompareTag("Button"))
                {
                    _objectObserved.GetComponent<ButtonInteraction>().PressButton();
                }
            }

            
        }
    
        // Si la acción está en el estado "Canceled" (tecla soltada)
        if (context.canceled) 
        {
            interacting = false;
          
        }
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
