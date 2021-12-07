using System.Collections;
using Gretas.Artworks.Images;
using Gretas.Artworks.Images.Loaders;
using Gretas.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Gretas.User.UI
{
    public class UserUI : MonoBehaviour
    {
        [SerializeField] private GameObject _galleryManager;
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private GameObject _imageExpandButton;
        [SerializeField] private GameObject _navigationPanel;
        [SerializeField] private GameObject _exitButton;
        [SerializeField] private GameObject _sceneLoadPanel;
        [SerializeField] private GameObject _refuseButton;

        private Button[] _enabledButtons;
        private ImageLoader _imageLoader;

        private void Awake()
        {
            _imagePanel.SetActive(false);
            _imageExpandButton.SetActive(false);
            _exitButton.SetActive(false);
            _sceneLoadPanel.SetActive(false);

            if (_galleryManager.TryGetComponent(out ImageLoader imageLoader))
            {
                _imageLoader = imageLoader;
            }
        }

        private void OnEnable()
        {
            _imagePanel.GetComponentInChildren<Button>().onClick.AddListener(CloseImagePanel);
            _imageExpandButton.GetComponent<Button>().onClick.AddListener(ExpandImagePanel);
            _exitButton.GetComponent<Button>().onClick.AddListener(GetComponent<UserExamine>().ExitVisualization);
            _refuseButton.GetComponent<Button>().onClick.AddListener(GetComponent<UserExamine>().ExitVisualization);

            _enabledButtons = new Button[] { _imagePanel.GetComponentInChildren<Button>(),
                                             _imageExpandButton.GetComponent<Button>(),
                                             _exitButton.GetComponent<Button>(),
                                             _refuseButton.GetComponent<Button>()
                                           };
        }

        private void OnDisable()
        {
            foreach (var button in _enabledButtons)
            {
                if (button)
                {
                    button.onClick.RemoveAllListeners();
                }
            }
        }

        public void GetImageData(ImageDisplay image)
        {
            if (DataCache.Instance.Textures.TryGetValue($"{image.Id}-high", out Texture2D texture))
            {
                _imagePanel.GetComponentInChildren<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
                _imagePanel.GetComponentInChildren<RawImage>().texture = texture;
                _imageExpandButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                _imageExpandButton.GetComponent<Button>().interactable = false;
                _imageLoader.GetHighResTexture(image);

                StartCoroutine(LoadTexture(image.Id));
            }
        }

        public void ActivatePanels(bool isActive)
        {
            _imageExpandButton.SetActive(isActive);

            if (!isActive)
            {
                StopAllCoroutines();

                _imagePanel.SetActive(false);
                _sceneLoadPanel.SetActive(false);
            }
        }

        public void HideNavigation(bool isHidden)
        {
            _navigationPanel.SetActive(!isHidden);
            _exitButton.SetActive(isHidden);
        }

        public void ActivateSceneLoadPanel()
        {
            _sceneLoadPanel.SetActive(true);
            _navigationPanel.SetActive(false);
        }

        private IEnumerator LoadTexture(string imageId)
        {
            Texture2D texture;

            while (!DataCache.Instance.Textures.TryGetValue($"{imageId}-high", out texture))
            {
                yield return null;
            }

            _imagePanel.GetComponentInChildren<AspectRatioFitter>().aspectRatio = (float)texture.width / texture.height;
            _imagePanel.GetComponentInChildren<RawImage>().texture = texture;
            _imageExpandButton.GetComponent<Button>().interactable = true;
        }

        private void ExpandImagePanel()
        {
            _imagePanel.SetActive(true);
            _imageExpandButton.SetActive(false);
            _exitButton.SetActive(false);
        }

        private void CloseImagePanel()
        {
            _imagePanel.SetActive(false);
            _imageExpandButton.SetActive(true);
            _exitButton.SetActive(true);
        }
    }
}