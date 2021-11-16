using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserMovement : MonoBehaviour
    {
        [SerializeField] private Camera _firstPersonCamera;
        [SerializeField] private float _movementSpeed = 10.0f;
        [SerializeField] private LayerMask _layerToClick;

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
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
            _inputActions.User.Interact.canceled -= ClickToMove;
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

        public void MoveToArtwork(Transform artwork)
        {
            _targetPosition = artwork.position - artwork.forward * 5.0f;
            _targetPosition.y = transform.position.y;
            //CANCEL IN CASE OF INPUT DURING MOVEMENT
            _isMovingAfterClicking = true;
        }

        private void MoveWithKeyboard()
        {
            var inputValues = _inputActions.User.Movement.ReadValue<Vector2>();

            Vector3 forwardMovement = transform.forward * inputValues.y;
            Vector3 rightMovement = transform.right * inputValues.x;

            _characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * _movementSpeed);
        }

        private void ClickToMove(InputAction.CallbackContext context)
        {
            if (_canMove && context.interaction is PressInteraction && !EventSystem.current.currentSelectedGameObject)
            {
                if (Physics.Raycast(_firstPersonCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 100.0f, _layerToClick))
                {
                    _targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

                    _isMovingAfterClicking = true;
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