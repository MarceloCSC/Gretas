using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserMovement : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _movementSpeed = 10.0f;
        [SerializeField] private LayerMask _layerMasks;

        private bool _isMovingAfterClicking;
        private bool _canMove;
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
            _inputActions.User.Interact.canceled += ClickToMove;
            _inputActions.User.Movement.performed += _ => _isMovingAfterClicking = false;
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
            _inputActions.User.Interact.canceled -= ClickToMove;
            _inputActions.User.Movement.performed -= _ => _isMovingAfterClicking = false;
        }

        private void Update()
        {
            if (_isMovingAfterClicking)
            {
                MoveByClicking();
            }
            else if (_canMove)
            {
                MoveWithKeyboard();
            }
        }

        public void MoveToTarget(Transform target, bool isSelf = false)
        {
            Vector3 offset = isSelf ? Vector3.zero : target.forward * 5.0f;
            _targetPosition = target.position - offset;
            _targetPosition.y = transform.position.y;
            _isMovingAfterClicking = true;
        }

        public void MoveAfterTeleport(Transform portal)
        {
            Vector3 newPosition = new Vector3(portal.position.x, transform.position.y, portal.position.z) + -portal.forward * 5.0f;
            _targetPosition = newPosition;
        }

        private void MoveWithKeyboard()
        {
            Vector2 inputValues = _inputActions.User.Movement.ReadValue<Vector2>();

            Vector3 forwardMovement = transform.forward * inputValues.y;
            Vector3 rightMovement = transform.right * inputValues.x;

            _characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * _movementSpeed);
        }

        private void ClickToMove(InputAction.CallbackContext context)
        {
            if (_canMove && context.interaction is PressInteraction && !EventSystem.current.currentSelectedGameObject)
            {
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 100.0f, _layerMasks))
                {
                    if (hit.normal == Vector3.up)
                    {
                        _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        _isMovingAfterClicking = true;
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Portal"))
                    {
                        _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        _targetPosition += transform.forward * 1.5f;
                        _isMovingAfterClicking = true;
                    }
                }
            }
        }

        private void MoveByClicking()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) <= 1.0f)
            {
                _isMovingAfterClicking = false;
            }
        }
    }
}