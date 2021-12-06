using UnityEngine;
using UnityEngine.UI;

namespace Gretas.UI
{
    public class UIZoomImage : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Vector3 _initialScale;
        [SerializeField] private float _speedMultiplier = 0.001f;
        [SerializeField] private float _maxZoom = 3.0f;

        private UserInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new UserInputActions();
        }

        private void OnEnable()
        {
            _inputActions.User.Enable();
            _slider.onValueChanged.AddListener(SlideToZoom);
            transform.localScale = _initialScale;
        }

        private void OnDisable()
        {
            _inputActions.User.Disable();
            _slider.onValueChanged.RemoveListener(SlideToZoom);
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                ScrollToZoom();
            }
        }

        private void ScrollToZoom()
        {
            Vector2 inputValues = _inputActions.User.ZoomImage.ReadValue<Vector2>();
            Vector3 newScale = transform.localScale + Vector3.one * (inputValues.y * _speedMultiplier);

            transform.localScale = ClampScale(newScale);
            UpdateSlider(transform.localScale);
        }

        private void SlideToZoom(float sliderValue)
        {
            Vector3 newScale = Vector3.one * sliderValue;
            transform.localScale = ClampScale(newScale);
        }

        private void UpdateSlider(Vector3 newScale)
        {
            _slider.value = newScale.x;
        }

        private Vector3 ClampScale(Vector3 targetScale)
        {
            return Vector3.Max(Vector3.one, Vector3.Min(Vector3.one * _maxZoom, targetScale));
        }
    }
}