using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos
{
    public class VideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoDisplay[] _videos;

        private const string _path = "https://player.vimeo.com/external";

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

            videoPlayer.url = $"{_path}/{video.Id}";
            videoPlayer.targetTexture = texture;

            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            video.GetComponent<AudioSource>().mute = true;
            video.GetComponentInChildren<MeshRenderer>().material = material;
        }
    }
}