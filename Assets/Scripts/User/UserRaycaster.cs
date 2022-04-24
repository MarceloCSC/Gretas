using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gretas.User
{
    public class UserRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _rayLength = 50.0f;
        [SerializeField] private LayerMask _layerMasks;

        private bool _canRaycast;
        private RaycastHit _currentHit;
        private RaycastHit[] _hitResults;

        private UserInputActions _inputActions;
        private UserMovement _movement;

        private void Awake()
        {
            _hitResults = new RaycastHit[4];
            _movement = GetComponent<UserMovement>();
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
        }

        private void Update()
        {
            if (_canRaycast)
            {
                Physics.RaycastNonAlloc(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), _hitResults, _rayLength, _layerMasks);

                _currentHit = _hitResults.OrderBy(hit => hit.distance).FirstOrDefault();

                if (_currentHit.collider && _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Surface") && _currentHit.normal == Vector3.up)
                {
                    var targetPosition = new Vector3(_currentHit.point.x, transform.position.y, _currentHit.point.z);
                    _movement.MoveToTarget(targetPosition);
                }
                else if (_currentHit.collider && _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Portal"))
                {
                    var targetPosition = new Vector3(_currentHit.point.x, transform.position.y, _currentHit.point.z);
                    targetPosition += transform.forward * 1.5f;
                    _movement.MoveToTarget(targetPosition);
                }
                else if (_currentHit.collider && _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Viewable"))
                {
                    var targetPosition = new Vector3(_currentHit.transform.position.x, transform.position.y, _currentHit.transform.position.z);
                    targetPosition -= _currentHit.transform.forward * 5.0f;
                    _movement.MoveToTarget(targetPosition);
                }
            }
        }
    }
}