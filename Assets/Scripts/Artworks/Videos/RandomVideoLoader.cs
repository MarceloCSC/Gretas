using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos
{
    public class RandomVideoLoader : MonoBehaviour
    {
        [SerializeField] private VideoDisplay[] _videosDisplay;
        [SerializeField] private TextAsset _videosJson;

        private List<VideoUrl> _videosToPlay;
        private List<VideoUrl> _videosPlayed;

        private void Start()
        {
            if (_videosDisplay.Length == 0)
            {
                _videosDisplay = FindObjectsOfType<VideoDisplay>();
            }

            _videosToPlay = JsonUtility.FromJson<VideoDatabase>(_videosJson.text).videos.ToList();
            _videosPlayed = new List<VideoUrl>();

            foreach (var display in _videosDisplay)
            {
                var videoUrl = _videosToPlay[Random.Range(0, _videosToPlay.Count - 1)];
                _videosToPlay.Remove(videoUrl);
                _videosPlayed.Add(videoUrl);

                var videoPlayer = display.GetComponent<VideoPlayer>();
                videoPlayer.GetComponent<AudioSource>().mute = true;
                videoPlayer.loopPointReached += SelectRandomVideo;

                LoadVideo(videoPlayer, videoUrl);
            }
        }

        private void LoadVideo(VideoPlayer videoPlayer, VideoUrl videoUrl)
        {
            var texture = new RenderTexture(1280, 720, 24);

            videoPlayer.url = $"{videoUrl.baseUrl}/{videoUrl.videoId}";
            videoPlayer.targetTexture = texture;

            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            videoPlayer.GetComponentInChildren<MeshRenderer>().material = material;
        }

        private void SelectRandomVideo(VideoPlayer videoPlayer)
        {
            if (_videosToPlay.Count > 0)
            {
                var videoUrl = _videosToPlay[Random.Range(0, _videosToPlay.Count - 1)];
                _videosToPlay.Remove(videoUrl);
                _videosPlayed.Add(videoUrl);

                LoadVideo(videoPlayer, videoUrl);
            }
            else
            {
                foreach (var video in _videosPlayed.ToList())
                {
                    _videosPlayed.Remove(video);
                    _videosToPlay.Add(video);
                }

                SelectRandomVideo(videoPlayer);
            }
        }
    }
}