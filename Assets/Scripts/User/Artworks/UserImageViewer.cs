using System.Collections;
using Gretas.Artworks.Images;
using Gretas.Artworks.Info;
using Gretas.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Gretas.User.Artworks
{
    public class UserImageViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _galleryManager;
        [SerializeField] private GameObject _imagePanel;
        [SerializeField] private GameObject _imageExpandButton;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _infoExpandButton;
        [SerializeField] private GameObject _navigationPanel;
        [SerializeField] private GameObject _exitButton;

        private ImageLoader _imageLoader;
        private InfoLoader _infoLoader;

        private void Awake()
        {
            _imagePanel.SetActive(false);
            _infoPanel.SetActive(false);
            _imageExpandButton.SetActive(false);
            _infoExpandButton.SetActive(false);
            _exitButton.SetActive(false);
            _imageLoader = _galleryManager.GetComponent<ImageLoader>();
            _infoLoader = _galleryManager.GetComponent<InfoLoader>();
        }

        private void OnEnable()
        {
            _imagePanel.GetComponentInChildren<Button>().onClick.AddListener(CloseImagePanel);
            _infoPanel.GetComponentInChildren<Button>().onClick.AddListener(CloseInfoPanel);
            _imageExpandButton.GetComponent<Button>().onClick.AddListener(ExpandImagePanel);
            _infoExpandButton.GetComponent<Button>().onClick.AddListener(ExpandInfoPanel);
            _exitButton.GetComponent<Button>().onClick.AddListener(GetComponent<UserExamine>().ExitVisualization);
        }

        private void OnDisable()
        {
            if (_imagePanel)
            {
                _imagePanel.GetComponentInChildren<Button>().onClick.RemoveListener(CloseImagePanel);
            }

            if (_infoPanel)
            {
                _infoPanel.GetComponentInChildren<Button>().onClick.RemoveListener(CloseInfoPanel);
            }

            if (_imageExpandButton)
            {
                _imageExpandButton.GetComponent<Button>().onClick.RemoveListener(ExpandImagePanel);
            }

            if (_infoExpandButton)
            {
                _infoExpandButton.GetComponent<Button>().onClick.RemoveListener(ExpandInfoPanel);
            }

            if (_exitButton)
            {
                _exitButton.GetComponent<Button>().onClick.RemoveListener(GetComponent<UserExamine>().ExitVisualization);
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
            _infoExpandButton.SetActive(isActive);
            _navigationPanel.SetActive(!isActive);
            _exitButton.SetActive(isActive);

            if (!isActive)
            {
                StopAllCoroutines();
                _imagePanel.SetActive(isActive);
                _infoPanel.SetActive(isActive);
            }
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
            _infoExpandButton.SetActive(false);
            _exitButton.SetActive(false);
        }

        private void CloseImagePanel()
        {
            _imagePanel.SetActive(false);
            _imageExpandButton.SetActive(true);
            _infoExpandButton.SetActive(true);
            _exitButton.SetActive(true);
        }

        private void ExpandInfoPanel()
        {
            _infoPanel.SetActive(true);
            _imageExpandButton.SetActive(false);
            _infoExpandButton.SetActive(false);
        }

        private void CloseInfoPanel()
        {
            _infoPanel.SetActive(false);
            _imageExpandButton.SetActive(true);
            _infoExpandButton.SetActive(true);
        }
    }
}