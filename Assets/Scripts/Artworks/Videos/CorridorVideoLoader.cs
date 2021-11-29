using System.Collections.Generic;
using System.Linq;
using Gretas.Environment;
using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos
{
    public class CorridorVideoLoader : MonoBehaviour
    {
        [SerializeField] private Transform[] _corridors;
        [SerializeField] private Transform _activeCorridor;
        [SerializeField] private TextAsset _videosJson;

        private Transform _lastVisitedCorridor;
        private List<VideoUrl> _videosToPlay;
        private Stack<Dictionary<string, VideoUrl>> _videosStackA;
        private Stack<Dictionary<string, VideoUrl>> _videosStackB;
        private Stack<Dictionary<string, VideoUrl>> _currentStack;

        private void Awake()
        {
            CreateRenderTextures();
            _videosToPlay = JsonUtility.FromJson<VideoDatabase>(_videosJson.text).videos.ToList();
            _videosStackA = new Stack<Dictionary<string, VideoUrl>>();
            _videosStackB = new Stack<Dictionary<string, VideoUrl>>();
            _currentStack = _videosStackA;
        }

        private void Start()
        {
            SelectRandomVideos(_activeCorridor);
        }

        private void OnEnable()
        {
            foreach (var corridor in _corridors)
            {
                corridor.GetComponent<CorridorSwapper>().OnActiveCorridor += ChangeActiveCorridor;
            }
        }

        private void OnDisable()
        {
            foreach (var corridor in _corridors)
            {
                if (corridor)
                {
                    corridor.GetComponent<CorridorSwapper>().OnActiveCorridor -= ChangeActiveCorridor;
                }
            }
        }

        public void ChangeActiveCorridor(Transform corridor)
        {
            if (corridor == _lastVisitedCorridor)
            {
                var otherStack = _currentStack == _videosStackA ? _videosStackB : _videosStackA;
                otherStack.Push(_currentStack.Pop());
                var corridorVideos = _currentStack.Pop();
                _currentStack = otherStack;
                LoadVideos(corridor, corridorVideos);
            }
            else
            {
                if (_videosToPlay.Count > 0 && (_videosStackA.Count == 0 || _videosStackB.Count == 0))
                {
                    SelectRandomVideos(corridor);
                }
                else
                {
                    var otherStack = _currentStack == _videosStackA ? _videosStackB : _videosStackA;

                    if (otherStack.Count == 0)
                    {
                        for (int i = 0; i < _currentStack.Count; i++)
                        {
                            otherStack.Push(_currentStack.Pop());
                        }
                    }

                    LoadVideos(corridor, otherStack.Pop());
                }
            }

            StopVideos(_activeCorridor);
            _lastVisitedCorridor = _activeCorridor;
            _activeCorridor = corridor;
        }

        private void SelectRandomVideos(Transform corridor)
        {
            var corridorVideos = new Dictionary<string, VideoUrl>();

            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                var videoUrl = _videosToPlay[Random.Range(0, _videosToPlay.Count - 1)];

                _videosToPlay.Remove(videoUrl);
                corridorVideos.Add(videoPlayer.name, videoUrl);

                PlayVideo(videoPlayer, videoUrl);
            }

            _currentStack.Push(corridorVideos);
        }

        private void LoadVideos(Transform corridor, Dictionary<string, VideoUrl> corridorVideos)
        {
            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                PlayVideo(videoPlayer, corridorVideos[videoPlayer.name]);
            }

            _currentStack.Push(corridorVideos);
        }

        private void PlayVideo(VideoPlayer videoPlayer, VideoUrl videoUrl)
        {
            videoPlayer.url = $"{videoUrl.baseUrl}/{videoUrl.videoId}";
            videoPlayer.SetDirectAudioVolume(0, 0.0f);
            videoPlayer.SetDirectAudioMute(0, true);
            videoPlayer.Play();
        }

        private void StopVideos(Transform corridor)
        {
            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                videoPlayer.SetDirectAudioVolume(0, 0.0f);
                videoPlayer.SetDirectAudioMute(0, true);
                videoPlayer.Stop();
            }
        }

        private void CreateRenderTextures()
        {
            foreach (var videoPlayer in FindObjectsOfType<VideoPlayer>())
            {
                var texture = new RenderTexture(1280, 720, 24);
                videoPlayer.targetTexture = texture;

                var material = new Material(Shader.Find("Unlit/Texture"))
                {
                    mainTexture = texture
                };

                videoPlayer.GetComponentInChildren<MeshRenderer>().material = material;
            }
        }
    }
}