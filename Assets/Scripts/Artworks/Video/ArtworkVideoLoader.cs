using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Video
{
    public class ArtworkVideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoFrame[] _videos;

        private void Start()
        {
            _videos = FindObjectsOfType<VideoFrame>();

            foreach (var video in _videos)
            {
                LoadVideo(video);
            }
        }

        private void LoadVideo(VideoFrame video)
        {
            var texture = new RenderTexture(1280, 720, 24);
            var videoplayer = video.GetComponent<VideoPlayer>();

            videoplayer.url = FindUrl(video.FrameId);
            videoplayer.targetTexture = texture;

            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            video.GetComponent<AudioSource>().mute = true;
            video.GetComponent<MeshRenderer>().material = material;
        }

        private string FindUrl(string id)
        {
            return id switch
            {
                "vid0001" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid01.mp4",
                "vid0002" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid02.mp4",
                _ => string.Empty,
            };
        }
    }
}