using System.Text;
using Gretas.Artworks.Info;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gretas.User.Artwork.Info
{
    public class UserInfoViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _expandButton;

        private ArtworkInfo _currentArtworkInfo;
        private InfoLoader _infoLoader;

        private void Awake()
        {
            _infoPanel.SetActive(false);
            _expandButton.SetActive(false);
            _infoLoader = FindObjectOfType<InfoLoader>();
        }

        private void OnEnable()
        {
            _expandButton.GetComponent<Button>().onClick.AddListener(ExpandPanel);
            _infoPanel.GetComponentInChildren<Button>().onClick.AddListener(ClosePanel);
        }

        private void OnDisable()
        {
            if (_expandButton)
            {
                _expandButton.GetComponent<Button>().onClick.RemoveListener(ExpandPanel);
            }

            if (_infoPanel)
            {
                _infoPanel.GetComponentInChildren<Button>().onClick.RemoveListener(ClosePanel);
            }
        }

        public void LoadArtworkInfo(string artworkId)
        {
            _currentArtworkInfo = _infoLoader.GetArtworkInfo(artworkId);

            if (_currentArtworkInfo != null)
            {
                var stringBuilder = new StringBuilder();

                _infoPanel.GetComponentInChildren<TextMeshProUGUI>().text = stringBuilder.ToString();
            }
        }

        public void ActivatePanel(bool isActive)
        {
            _expandButton.SetActive(isActive);

            if (!isActive)
            {
                _infoPanel.SetActive(isActive);
            }
        }

        public void ExpandPanel()
        {
            _infoPanel.SetActive(true);
            _expandButton.SetActive(false);
        }

        public void ClosePanel()
        {
            _infoPanel.SetActive(false);
            _expandButton.SetActive(true);
        }
    }
}