using UnityEngine;

namespace Gretas.UI
{
    public class UIZoomImage : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = 0.001f;
        [SerializeField] private float _maxZoom = 3.0f;

        private Vector3 _initialScale;

        private UserInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new UserInputActions();
            _initialScale = Vector3.one;
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
            transform.localScale = _initialScale;
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                ScrollToZoom();
            }
        }

        public void ScrollToZoom()
        {
            Vector2 inputValues = _inputActions.User.ZoomImage.ReadValue<Vector2>();
            Vector3 newScale = transform.localScale + Vector3.one * (inputValues.y * _zoomSpeed);

            transform.localScale = ClampScale(newScale);
        }

        private Vector3 ClampScale(Vector3 targetScale)
        {
            return Vector3.Max(_initialScale, Vector3.Min(_initialScale * _maxZoom, targetScale));
        }
    }
}