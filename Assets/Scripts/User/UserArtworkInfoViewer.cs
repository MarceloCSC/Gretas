using System.Text;
using Gretas.Artworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gretas.User
{
    public class UserArtworkInfoViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _artworkInfoPanel;
        [SerializeField] private GameObject _expandButton;

        private ArtworkInfo _currentArtworkInfo;
        private ArtworkInfoLoader _artworkInfoLoader;

        private void Awake()
        {
            _artworkInfoPanel.SetActive(false);
            _expandButton.SetActive(false);
            _artworkInfoLoader = FindObjectOfType<ArtworkInfoLoader>();
        }

        private void OnEnable()
        {
            _expandButton.GetComponent<Button>().onClick.AddListener(ExpandPanel);
            _artworkInfoPanel.GetComponentInChildren<Button>().onClick.AddListener(ClosePanel);
        }

        private void OnDisable()
        {
            if (_expandButton)
            {
                _expandButton.GetComponent<Button>().onClick.RemoveListener(ExpandPanel);
            }

            if (_artworkInfoPanel)
            {
                _artworkInfoPanel.GetComponentInChildren<Button>().onClick.RemoveListener(ClosePanel);
            }
        }

        public void LoadArtworkInfo(string frameId)
        {
            _currentArtworkInfo = _artworkInfoLoader.GetArtworkInfo(frameId);

            if (_currentArtworkInfo != null)
            {
                var stringBuilder = new StringBuilder();

                if (_currentArtworkInfo.artist != string.Empty)
                {
                    stringBuilder.AppendLine($"Artista: {_currentArtworkInfo.artist}");
                }

                if (_currentArtworkInfo.title != string.Empty)
                {
                    stringBuilder.AppendLine($"Título: {_currentArtworkInfo.title}");
                }

                if (_currentArtworkInfo.location != string.Empty)
                {
                    stringBuilder.AppendLine($"Local: {_currentArtworkInfo.location}");
                }

                if (_currentArtworkInfo.date != string.Empty)
                {
                    stringBuilder.AppendLine($"Data: {_currentArtworkInfo.date}");
                }

                if (_currentArtworkInfo.currentLocation != string.Empty)
                {
                    stringBuilder.AppendLine($"Localização atual: {_currentArtworkInfo.currentLocation}");
                }

                if (_currentArtworkInfo.dimensions != string.Empty)
                {
                    stringBuilder.AppendLine($"Dimensões: {_currentArtworkInfo.dimensions}");
                }

                if (_currentArtworkInfo.materials != string.Empty)
                {
                    stringBuilder.AppendLine($"Materiais: {_currentArtworkInfo.materials}");
                }

                _artworkInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = stringBuilder.ToString();
            }
        }

        public void ActivatePanel(bool isActive)
        {
            _expandButton.SetActive(isActive);

            if (!isActive)
            {
                _artworkInfoPanel.SetActive(isActive);
            }
        }

        public void ExpandPanel()
        {
            _artworkInfoPanel.SetActive(true);
            _expandButton.SetActive(false);
        }

        public void ClosePanel()
        {
            _artworkInfoPanel.SetActive(false);
            _expandButton.SetActive(true);
        }
    }
}