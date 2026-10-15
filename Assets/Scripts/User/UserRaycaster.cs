using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Gretas.User
{
    public class UserRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _rayLength = 50.0f;
        [SerializeField] private LayerMask _layerMasks;

        private RaycastHit _currentHit;
        private RaycastHit[] _hitResults;

        private UserInputActions _inputActions;
        private UserMovement _movement;
        private UserExamine _examine;

        private void Awake()
        {
            _inputActions = new UserInputActions();
            _hitResults = new RaycastHit[4];
            _movement = GetComponent<UserMovement>();
            _examine = GetComponent<UserExamine>();
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
            _inputActions.User.Interact.canceled += OnClick;
        }

        private void OnDisable()
        {
            _inputActions.User.Interact.canceled -= OnClick;
            _inputActions.User.Disable();
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (!(context.interaction is PressInteraction))
            {
                return;
            }

            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject)
            {
                return;
            }

            int hitCount = Physics.RaycastNonAlloc(
                _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()),
                _hitResults,
                _rayLength,
                _layerMasks
            );

            if (hitCount == 0)
            {
                return;
            }

            _currentHit = _hitResults
                .Take(hitCount)
                .OrderBy(hit => hit.distance)
                .FirstOrDefault();

            if (_currentHit.collider &&
                _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Surface") &&
                _currentHit.normal == Vector3.up)
            {
                var targetPosition = new Vector3(
                    _currentHit.point.x,
                    transform.position.y,
                    _currentHit.point.z
                );

                _movement.MoveToTarget(targetPosition);
            }
            else if (_currentHit.collider &&
                     _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Portal"))
            {
                var targetPosition = new Vector3(
                    _currentHit.point.x,
                    transform.position.y,
                    _currentHit.point.z
                );

                targetPosition += transform.forward * 1.5f;
                _movement.MoveToTarget(targetPosition);
            }
            else if (_currentHit.collider &&
                     _currentHit.collider.gameObject.layer == LayerMask.NameToLayer("Viewable"))
            {
                var targetPosition = new Vector3(
                    _currentHit.transform.position.x,
                    transform.position.y,
                    _currentHit.transform.position.z
                );

                _examine.ExamineArtwork(_currentHit.transform);

                targetPosition -= _currentHit.transform.forward * 5.0f;

                targetPosition.y = transform.position.y;

                _movement.MoveToTarget(targetPosition);
            }
        }


    }
}