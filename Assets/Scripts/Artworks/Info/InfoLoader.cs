using System.Text;
using TMPro;
using UnityEngine;

namespace Gretas.Artworks.Info
{
    public class InfoLoader : MonoBehaviour
    {
        [SerializeField] private InfoLabel[] _labels;
        [SerializeField] private TextAsset _infoJson;

        private void Start()
        {
            if (_labels.Length == 0)
            {
                _labels = FindObjectsOfType<InfoLabel>();
            }

            var infoDatabase = JsonUtility.FromJson<InfoDatabase>(_infoJson.text);

            foreach (var label in _labels)
            {
                foreach (var artworkInfo in infoDatabase.data)
                {
                    if (artworkInfo.artworkId == label.Id)
                    {
                        LoadArtworkInfo(label, artworkInfo);
                    }
                }
            }
        }

        public ArtworkInfo GetArtworkInfo(string artworkId)
        {
            //foreach (var artworkInfo in _infoDatabase.data)
            //{
            //    if (artworkInfo.artworkId == artworkId)
            //    {
            //        return artworkInfo;
            //    }
            //}

            //Debug.LogWarning("The corresponding artwork could not be found in our database.");

            return null;
        }

        private void LoadArtworkInfo(InfoLabel label, ArtworkInfo artworkInfo)
        {
            var stringBuilder = new StringBuilder();

            if (artworkInfo.artist != string.Empty)
            {
                stringBuilder.AppendLine($"Artista: {artworkInfo.artist}");
            }

            if (artworkInfo.title != string.Empty)
            {
                stringBuilder.AppendLine($"Título: {artworkInfo.title}");
            }

            if (artworkInfo.location != string.Empty)
            {
                stringBuilder.AppendLine($"Local: {artworkInfo.location}");
            }

            if (artworkInfo.date != string.Empty)
            {
                stringBuilder.AppendLine($"Data: {artworkInfo.date}");
            }

            if (artworkInfo.currentLocation != string.Empty)
            {
                stringBuilder.AppendLine($"Localização atual: {artworkInfo.currentLocation}");
            }

            if (artworkInfo.dimensions != string.Empty)
            {
                stringBuilder.AppendLine($"Dimensões: {artworkInfo.dimensions}");
            }

            if (artworkInfo.materials != string.Empty)
            {
                stringBuilder.AppendLine($"Materiais: {artworkInfo.materials}");
            }

            label.GetComponentInChildren<TextMeshPro>().text = stringBuilder.ToString();
        }
    }
}