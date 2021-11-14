using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos
{
    public class VideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoDisplay[] _videos;

        private void Start()
        {
            _videos = FindObjectsOfType<VideoDisplay>();

            foreach (var video in _videos)
            {
                LoadVideo(video);
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
            return videoId switch
            {
                "vid0001" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid01.mp4",
                "vid0002" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid02.mp4",
                _ => string.Empty,
            };
        }
    }
}