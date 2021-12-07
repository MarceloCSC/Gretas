using System.Text;
using TMPro;
using UnityEngine;

namespace Gretas.Artworks.Labels.Loaders
{
    public class LabelLoader : MonoBehaviour
    {
        [SerializeField] private LabelDisplay[] _labelsDisplay;
        [SerializeField] private TextAsset _labelsJson;

        private void Start()
        {
            if (_labelsDisplay.Length == 0)
            {
                _labelsDisplay = FindObjectsOfType<LabelDisplay>();
            }

            var labelDatabase = JsonUtility.FromJson<LabelDatabase>(_labelsJson.text);

            foreach (var display in _labelsDisplay)
            {
                foreach (var label in labelDatabase.labels)
                {
                    if (label.artworkId == display.Id)
                    {
                        LoadLabel(display, label);
                    }
                }
            }
        }

        private void LoadLabel(LabelDisplay labelDisplay, Label label)
        {
            var stringBuilder = new StringBuilder();

            if (label.title != string.Empty)
            {
                labelDisplay.transform.GetChild(0).GetComponent<TextMeshPro>().text = label.title;
            }

            if (label.artist != string.Empty)
            {
                stringBuilder.AppendLine(label.artist);
            }

            if (label.location != string.Empty)
            {
                stringBuilder.AppendLine(label.location);
            }

            if (label.date != string.Empty)
            {
                stringBuilder.AppendLine(label.date);
            }

            if (label.currentLocation != string.Empty)
            {
                stringBuilder.AppendLine(label.currentLocation);
            }

            if (label.dimensions != string.Empty)
            {
                stringBuilder.AppendLine(label.dimensions);
            }

            if (label.fileSize != string.Empty)
            {
                stringBuilder.AppendLine(label.fileSize);
            }

            if (label.materials != string.Empty)
            {
                stringBuilder.AppendLine(label.materials);
            }

            labelDisplay.transform.GetChild(1).GetComponent<TextMeshPro>().text = stringBuilder.ToString();
        }
    }
}