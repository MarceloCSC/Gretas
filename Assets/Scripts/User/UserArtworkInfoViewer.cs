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

        private Artwork _currentArtwork;
        private ArtworkLoader _artworkLoader;

        private void Awake()
        {
            _artworkInfoPanel.SetActive(false);
            _expandButton.SetActive(false);
            _artworkLoader = FindObjectOfType<ArtworkLoader>();
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
            _currentArtwork = _artworkLoader.GetArtwork(frameId);

            if (_currentArtwork != null)
            {
                var stringBuilder = new StringBuilder();

                if (_currentArtwork.artist != string.Empty)
                {
                    stringBuilder.AppendLine($"Artista: {_currentArtwork.artist}");
                }

                if (_currentArtwork.title != string.Empty)
                {
                    stringBuilder.AppendLine($"Título: {_currentArtwork.title}");
                }

                if (_currentArtwork.location != string.Empty)
                {
                    stringBuilder.AppendLine($"Local: {_currentArtwork.location}");
                }

                if (_currentArtwork.date != string.Empty)
                {
                    stringBuilder.AppendLine($"Data: {_currentArtwork.date}");
                }

                if (_currentArtwork.currentLocation != string.Empty)
                {
                    stringBuilder.AppendLine($"Localização atual: {_currentArtwork.currentLocation}");
                }

                if (_currentArtwork.dimensions != string.Empty)
                {
                    stringBuilder.AppendLine($"Dimensões: {_currentArtwork.dimensions}");
                }

                if (_currentArtwork.materials != string.Empty)
                {
                    stringBuilder.AppendLine($"Materiais: {_currentArtwork.materials}");
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