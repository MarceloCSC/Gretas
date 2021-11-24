using System.Collections;
using Gretas.Artworks.Images;
using Gretas.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Gretas.User.Artworks
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
        //[SerializeField] private GameObject _infoPanel;
        //[SerializeField] private GameObject _infoExpandButton;

        private ImageLoader _imageLoader;
        //private InfoLoader _infoLoader;

        private void Awake()
        {
            _imagePanel.SetActive(false);
            _imageExpandButton.SetActive(false);
            _exitButton.SetActive(false);
            _sceneLoadPanel.SetActive(false);
            //_infoPanel.SetActive(false);
            //_infoExpandButton.SetActive(false);
            _imageLoader = _galleryManager.GetComponent<ImageLoader>();
            //_infoLoader = _galleryManager.GetComponent<InfoLoader>();
        }

        private void OnEnable()
        {
            _imagePanel.GetComponentInChildren<Button>().onClick.AddListener(CloseImagePanel);
            _imageExpandButton.GetComponent<Button>().onClick.AddListener(ExpandImagePanel);
            _exitButton.GetComponent<Button>().onClick.AddListener(GetComponent<UserExamine>().ExitVisualization);
            _refuseButton.GetComponent<Button>().onClick.AddListener(GetComponent<UserExamine>().ExitVisualization);
            //_infoPanel.GetComponentInChildren<Button>().onClick.AddListener(CloseInfoPanel);
            //_infoExpandButton.GetComponent<Button>().onClick.AddListener(ExpandInfoPanel);
        }

        private void OnDisable()
        {
            if (_imagePanel)
            {
                _imagePanel.GetComponentInChildren<Button>().onClick.RemoveListener(CloseImagePanel);
            }

            if (_imageExpandButton)
            {
                _imageExpandButton.GetComponent<Button>().onClick.RemoveListener(ExpandImagePanel);
            }

            if (_exitButton)
            {
                _exitButton.GetComponent<Button>().onClick.RemoveListener(GetComponent<UserExamine>().ExitVisualization);
            }

            if (_refuseButton)
            {
                _refuseButton.GetComponent<Button>().onClick.RemoveListener(GetComponent<UserExamine>().ExitVisualization);
            }
            //if (_infoPanel)
            //{
            //    _infoPanel.GetComponentInChildren<Button>().onClick.RemoveListener(CloseInfoPanel);
            //}

            //if (_infoExpandButton)
            //{
            //    _infoExpandButton.GetComponent<Button>().onClick.RemoveListener(ExpandInfoPanel);
            //}
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
            //_infoExpandButton.SetActive(isActive);

            if (!isActive)
            {
                StopAllCoroutines();

                _imagePanel.SetActive(false);
                _sceneLoadPanel.SetActive(false);
                //_infoPanel.SetActive(false);
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
            //_infoExpandButton.SetActive(false);
        }

        private void CloseImagePanel()
        {
            _imagePanel.SetActive(false);
            _imageExpandButton.SetActive(true);
            _exitButton.SetActive(true);
            //_infoExpandButton.SetActive(true);
        }

        //private void ExpandInfoPanel()
        //{
        //    _infoPanel.SetActive(true);
        //    _imageExpandButton.SetActive(false);
        //    _infoExpandButton.SetActive(false);
        //}

        //private void CloseInfoPanel()
        //{
        //    _infoPanel.SetActive(false);
        //    _imageExpandButton.SetActive(true);
        //    _infoExpandButton.SetActive(true);
        //}
    }
}