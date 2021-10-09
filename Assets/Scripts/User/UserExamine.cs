using Gretas.Artworks;
using Gretas.User.Artwork.Info;
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
        private UserArtworkInfoViewer _artworkInfoViewer;

        private void Awake()
        {
            _inputActions = new UserInputActions();
            _secondaryCamera.enabled = false;
            _movement = GetComponent<UserMovement>();
            _vision = GetComponent<UserVision>();
            _artworkInfoViewer = GetComponent<UserArtworkInfoViewer>();
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();

            _inputActions.User.Interact.canceled += _ =>
            {
                if (!_isExamining && _.interaction is PressInteraction && !EventSystem.current.IsPointerOverGameObject())
                {
                    ExamineArtwork();
                }
            };

            _inputActions.User.Movement.performed += _ =>
            {
                if (_isExamining)
                {
                    ExitVisualization();
                }
            };
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();

            _inputActions.User.Interact.canceled -= _ =>
            {
                if (!_isExamining && _.interaction is PressInteraction && !EventSystem.current.IsPointerOverGameObject())
                {
                    ExamineArtwork();
                }
            };

            _inputActions.User.Movement.performed -= _ =>
            {
                if (_isExamining)
                {
                    ExitVisualization();
                }
            };
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
            _isPositioning = false;
            _isResetting = true;
            _artworkInfoViewer.ActivatePanel(false);
        }

        private void ExamineArtwork()
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, 100.0f, _layerToClick))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Artwork"))
                {
                    float minDistance = CalculateDistance(hit.transform.GetComponent<MeshRenderer>().bounds);
                    _position = hit.transform.position - hit.transform.forward * minDistance;
                    _rotation = Quaternion.LookRotation(hit.transform.forward);

                    ChangeCameras();

                    // rotate main camera to face painting
                    _movement.CanMove = false;
                    _vision.CanLook = false;
                    _isPositioning = true;
                    _isExamining = true;
                    _artworkInfoViewer.ActivatePanel(true);
                    _artworkInfoViewer.LoadArtworkInfo(hit.transform.GetComponent<ArtworkFrame>().FrameId);
                }
            }
        }

        private void PositionCamera()
        {
            _secondaryCamera.transform.position = Vector3.MoveTowards(_secondaryCamera.transform.position, _position, _speed * Time.deltaTime);
            _secondaryCamera.transform.rotation = Quaternion.Slerp(_secondaryCamera.transform.rotation, _rotation, _speed * Time.deltaTime);

            if (Vector3.Distance(_secondaryCamera.transform.position, _position) <= 0.1f &&
                Quaternion.Angle(_secondaryCamera.transform.rotation, _rotation) <= 0.1f)
            {
                _isPositioning = false;
            }
        }

        private void ResettingCamera()
        {
            _secondaryCamera.transform.position = Vector3.MoveTowards(_secondaryCamera.transform.position, _mainCamera.transform.position, _speed * 2 * Time.deltaTime);
            _secondaryCamera.transform.rotation = Quaternion.Slerp(_secondaryCamera.transform.rotation, _mainCamera.transform.rotation, _speed * 2 * Time.deltaTime);

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
            _secondaryCamera.transform.position = _mainCamera.transform.position;
            _secondaryCamera.transform.rotation = _mainCamera.transform.rotation;
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