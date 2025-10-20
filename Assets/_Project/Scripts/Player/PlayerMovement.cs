using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        private Rigidbody _rb;
        private InputSystem_Actions _input;
        private Vector2 _moveInput;

        private void Awake()
        {
            _input = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Move.performed += On_Move;
            _input.Player.Move.canceled += On_Move;
        }

        private void OnDisable()
        {
            _input.Player.Move.performed -= On_Move;
            _input.Player.Move.canceled -= On_Move;
            _input.Disable();
        }
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void On_Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Vector3 right = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

            Vector3 moveDir = (right * _moveInput.x + forward * _moveInput.y).normalized;

            Vector3 newPos = _rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);
        }
    }
}
