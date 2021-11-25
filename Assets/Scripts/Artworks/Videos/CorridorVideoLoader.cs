using UnityEngine;

namespace Gretas.Artworks.Videos
{
    public class CorridorVideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoDisplay[] _videosDisplay;
        [SerializeField] private TextAsset _videosJson;

        private void Start()
        {
            if (_videosDisplay.Length == 0)
            {
                _videosDisplay = FindObjectsOfType<VideoDisplay>();
            }

            var corridorVideos = JsonUtility.FromJson<VideoDatabase>(_videosJson.text);

            foreach (var display in _videosDisplay)
            {
                foreach (var video in corridorVideos.videos)
                {
                    if (video.videoId == display.Id)
                    {

                    }
                }
            }
        }
    }
}