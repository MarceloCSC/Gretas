using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Video
{
    public class VideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoFrame[] _videoFrames;

        private void Start()
        {
            _videoFrames = FindObjectsOfType<VideoFrame>();

            foreach (var videoFrame in _videoFrames)
            {
                LoadVideo(videoFrame);
            }
        }

        private void LoadVideo(VideoFrame videoFrame)
        {
            var texture = new RenderTexture(1280, 720, 24);
            var videoplayer = videoFrame.GetComponent<VideoPlayer>();

            videoplayer.url = FindUrl(videoFrame.FrameId);
            videoplayer.targetTexture = texture;

            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            videoFrame.GetComponent<AudioSource>().mute = true;
            videoFrame.GetComponent<MeshRenderer>().material = material;
        }

        private string FindUrl(string frameId)
        {
            return frameId switch
            {
                "vid0001" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid01.mp4",
                "vid0002" => "https://gretasgaleria.blob.core.windows.net/data/videos/vid02.mp4",
                _ => string.Empty,
            };
        }
    }
}