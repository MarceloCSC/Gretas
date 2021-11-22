using Gretas.Artworks.Images;
using Gretas.User.Artworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserExamine : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Camera _secondaryCamera;
        [SerializeField] private float _visualizationMargin = 0.5f;
        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private LayerMask _layerToClick;

        private Vector3 _position;
        private Quaternion _rotation;
        private bool _isPositioning;
        private bool _isExamining;
        private bool _isResetting;

        private UserInputActions _inputActions;
        private UserMovement _movement;
        private UserVision _vision;
        private UserImageViewer _imageViewer;

        private void Awake()
        {
            _inputActions = new UserInputActions();
            _secondaryCamera.enabled = false;
            _movement = GetComponent<UserMovement>();
            _vision = GetComponent<UserVision>();
            _imageViewer = GetComponent<UserImageViewer>();
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
            _inputActions.User.Interact.canceled += ExamineArtwork;
            _inputActions.User.Movement.performed += _ => ExitVisualization();
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
            _inputActions.User.Interact.canceled -= ExamineArtwork;
            _inputActions.User.Movement.performed -= _ => ExitVisualization();
        }

        private void LateUpdate()
        {
            if (_isPositioning)
            {
                PositionCamera();
            }

            if (_isResetting)
            {
                ResettingCamera();
            }
        }

        public void ExitVisualization()
        {
            if (_isExamining)
            {
                _isPositioning = false;
                _isResetting = true;
                _movement.MoveToTarget(transform, true);
                _imageViewer.ActivatePanels(false);
            }
        }

        private void ExamineArtwork(InputAction.CallbackContext context)
        {
            if (!_isExamining && context.interaction is PressInteraction && !EventSystem.current.currentSelectedGameObject)
            {
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 100.0f, _layerToClick))
                {
                    float minDistance = CalculateDistance(hit.transform.GetComponent<MeshRenderer>().bounds);
                    _position = hit.transform.position - hit.transform.forward * minDistance;
                    _rotation = Quaternion.LookRotation(hit.transform.forward);

                    ChangeCameras();

                    // Possibly rotate main camera to face painting
                    _movement.CanMove = false;
                    _movement.MoveToTarget(hit.transform);
                    _vision.CanLook = false;
                    _isPositioning = true;
                    _isExamining = true;

                    if (hit.transform.TryGetComponent(out ImageDisplay imageDisplay))
                    {
                        _imageViewer.ActivatePanels(true);
                        _imageViewer.GetImageData(imageDisplay);
                    }
                }
            }
        }

        private void PositionCamera()
        {
            float gradualSpeed = Vector3.Distance(_secondaryCamera.transform.position, _position) * _speed; // Temporary
            Vector3 position = Vector3.MoveTowards(_secondaryCamera.transform.position, _position, gradualSpeed * Time.deltaTime);
            Quaternion rotation = Quaternion.Slerp(_secondaryCamera.transform.rotation, _rotation, _speed * Time.deltaTime);

            _secondaryCamera.transform.SetPositionAndRotation(position, rotation);

            if (Vector3.Distance(_secondaryCamera.transform.position, _position) <= 0.1f &&
                Quaternion.Angle(_secondaryCamera.transform.rotation, _rotation) <= 0.1f)
            {
                _isPositioning = false;
            }
        }

        private void ResettingCamera()
        {
            float gradualSpeed = Vector3.Distance(_secondaryCamera.transform.position, _mainCamera.transform.position) * _speed; // Temporary
            Vector3 position = Vector3.MoveTowards(_secondaryCamera.transform.position, _mainCamera.transform.position, gradualSpeed * Time.deltaTime);
            Quaternion rotation = Quaternion.Slerp(_secondaryCamera.transform.rotation, _mainCamera.transform.rotation, _speed * 2 * Time.deltaTime);

            _secondaryCamera.transform.SetPositionAndRotation(position, rotation);

            if (Vector3.Distance(_secondaryCamera.transform.position, _mainCamera.transform.position) <= 0.1f &&
                Quaternion.Angle(_secondaryCamera.transform.rotation, _mainCamera.transform.rotation) <= 0.1f)
            {
                _secondaryCamera.enabled = false;
                _mainCamera.enabled = true;
                _movement.CanMove = true;
                _vision.CanLook = true;
                _isResetting = false;
                _isExamining = false;
            }
        }

        private void ChangeCameras()
        {
            _secondaryCamera.transform.SetPositionAndRotation(_mainCamera.transform.position, _mainCamera.transform.rotation);
            _mainCamera.enabled = false;
            _secondaryCamera.enabled = true;
        }

        private float CalculateDistance(Bounds bounds)
        {
            return bounds.extents.magnitude * _visualizationMargin /
                Mathf.Sin(Mathf.Deg2Rad * _secondaryCamera.fieldOfView / 2.0f);
        }
    }
}