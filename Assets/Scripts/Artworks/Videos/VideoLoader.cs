using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos
{
    public class VideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoDisplay[] _videos;

        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/videos";

        private void Awake()
        {
            if (_videos.Length == 0)
            {
                _videos = FindObjectsOfType<VideoDisplay>();
            }
        }

        private void Start()
        {
            if (_videos.Length > 0)
            {
                foreach (var video in _videos)
                {
                    LoadVideo(video);
                }
            }
        }

        private void LoadVideo(VideoDisplay video)
        {
            var texture = new RenderTexture(1280, 720, 24);
            var videoPlayer = video.GetComponent<VideoPlayer>();

            videoPlayer.url = FindUrl(video.Id);
            videoPlayer.targetTexture = texture;

            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            video.GetComponent<AudioSource>().mute = true;
            video.GetComponent<MeshRenderer>().material = material;
        }

        private string FindUrl(string videoId)
        {
            return $"{_path}/{videoId}.mp4";
        }
    }
}