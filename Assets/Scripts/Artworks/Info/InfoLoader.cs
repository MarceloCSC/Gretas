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

        private void LoadArtworkInfo(InfoLabel label, ArtworkInfo artworkInfo)
        {
            var stringBuilder = new StringBuilder();

            if (artworkInfo.title != string.Empty)
            {
                label.transform.GetChild(0).GetComponent<TextMeshPro>().text = artworkInfo.title;
            }

            if (artworkInfo.artist != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.artist);
            }

            if (artworkInfo.location != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.location);
            }

            if (artworkInfo.date != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.date);
            }

            if (artworkInfo.currentLocation != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.currentLocation);
            }

            if (artworkInfo.dimensions != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.dimensions);
            }

            if (artworkInfo.fileSize != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.fileSize);
            }

            if (artworkInfo.materials != string.Empty)
            {
                stringBuilder.AppendLine(artworkInfo.materials);
            }

            label.transform.GetChild(1).GetComponent<TextMeshPro>().text = stringBuilder.ToString();
        }
    }
}