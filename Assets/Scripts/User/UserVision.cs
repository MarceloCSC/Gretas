using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserVision : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _mouseSensitivity = 15.0f;
        [SerializeField] private float _maxVerticalAngle = 90.0f;
        [SerializeField] private float _minVerticalAngle = -75.0f;

        private float _mouseX;
        private float _mouseY;
        private float _xAxisRotation;
        private bool _canLook;
        private bool _isLooking;

        private UserInputActions _inputActions;

        public bool CanLook { get => _canLook; set => _canLook = value; }

        private void Awake()
        {
            _inputActions = new UserInputActions();
            _canLook = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();

            _inputActions.User.Interact.performed += _ =>
            {
                if (_canLook && _.interaction is HoldInteraction)
                {
                    _isLooking = true;
                }
            };
            _inputActions.User.Interact.canceled += _ =>
            {
                if (_canLook && _.interaction is HoldInteraction)
                {
                    _isLooking = false;
                }
            };
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();

            _inputActions.User.Interact.performed -= _ =>
            {
                if (_canLook && _.interaction is HoldInteraction)
                {
                    _isLooking = true;
                }
            };
            _inputActions.User.Interact.canceled -= _ =>
            {
                if (_canLook && _.interaction is HoldInteraction)
                {
                    _isLooking = false;
                }
            };
        }

        private void Update()
        {
            if (_isLooking)
            {
                LookWithMouse();
            }
        }

        private void LookWithMouse()
        {
            var inputValues = _inputActions.User.Look.ReadValue<Vector2>();

            _mouseX = inputValues.x * _mouseSensitivity * Time.deltaTime;
            _mouseY = inputValues.y * _mouseSensitivity * Time.deltaTime;

            _xAxisRotation += _mouseY;

            if (_xAxisRotation > _maxVerticalAngle)
            {
                _xAxisRotation = _maxVerticalAngle;
                _mouseY = 0.0f;
                ClampRotation(-_maxVerticalAngle);
            }
            else if (_xAxisRotation < _minVerticalAngle)
            {
                _xAxisRotation = _minVerticalAngle;
                _mouseY = 0.0f;
                ClampRotation(-_minVerticalAngle);
            }

            _cameraTransform.Rotate(Vector3.left * _mouseY);
            transform.Rotate(Vector3.up * _mouseX);
        }

        private void ClampRotation(float xValue = 0, float yValue = 0, float zValue = 0)
        {
            Vector3 eulerRotation = _cameraTransform.eulerAngles;

            eulerRotation.x = xValue == 0 ? _cameraTransform.eulerAngles.x : xValue;
            eulerRotation.y = yValue == 0 ? _cameraTransform.eulerAngles.y : yValue;
            eulerRotation.z = zValue == 0 ? _cameraTransform.eulerAngles.z : zValue;

            _cameraTransform.eulerAngles = eulerRotation;
        }
    }
}