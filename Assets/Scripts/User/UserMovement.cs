using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 10.0f;

        private bool _canMove;
        private bool _hasClicked;
        private Vector3 _targetPosition;

        private CharacterController _characterController;
        private UserInputActions _inputActions;

        public bool CanMove { get => _canMove; set => _canMove = value; }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputActions = new UserInputActions();
            _canMove = true;
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
            _inputActions.User.Interact.canceled += OnClick;
            _inputActions.User.Movement.performed += _ => StopMovement();
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
            _inputActions.User.Interact.canceled -= OnClick;
            _inputActions.User.Movement.performed -= _ => StopMovement();
        }

        private void Update()
        {
            if (_hasClicked)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, _targetPosition) <= 1.0f)
                {
                    StopMovement();
                }
            }
            else if (_canMove)
            {
                Vector2 inputValues = _inputActions.User.Movement.ReadValue<Vector2>();
                Vector3 forwardMovement = transform.forward * inputValues.y;
                Vector3 rightMovement = transform.right * inputValues.x;

                _characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * _movementSpeed);
            }
        }

        public void MoveToTarget(Vector3 target)
        {
            _targetPosition = target;
            _hasClicked = true;
        }

        public void MoveAfterTeleport(Transform portal)
        {
            Vector3 newPosition = new Vector3(portal.position.x, transform.position.y, portal.position.z) + -portal.forward * 5.0f;
            _targetPosition = newPosition;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (_canMove && context.interaction is PressInteraction && !EventSystem.current.currentSelectedGameObject)
            {
                if (_targetPosition != Vector3.zero)
                {
                    _hasClicked = true;
                }
            }
        }

        private void StopMovement()
        {
            _hasClicked = false;
            _targetPosition = Vector3.zero;
        }
    }
}