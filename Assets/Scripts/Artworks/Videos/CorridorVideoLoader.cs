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
        private List<string> _videosToPlay;
        private Dictionary<string, double> _timestamps;
        private Stack<Dictionary<string, string>> _videosStackA;
        private Stack<Dictionary<string, string>> _videosStackB;
        private Stack<Dictionary<string, string>> _currentStack;

        private void Awake()
        {
            CreateRenderTextures();
            _videosToPlay = JsonUtility.FromJson<VideoDatabase>(_videosJson.text).videos.Select(videoUrl => $"{videoUrl.baseUrl}/{videoUrl.videoId}").ToList();
            _videosStackA = new Stack<Dictionary<string, string>>();
            _videosStackB = new Stack<Dictionary<string, string>>();
            _timestamps = new Dictionary<string, double>();
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
            var otherStack = _currentStack == _videosStackA ? _videosStackB : _videosStackA;

            StopVideos(_activeCorridor, _currentStack.Peek());

            if (corridor == _lastVisitedCorridor)
            {
                otherStack.Push(_currentStack.Pop());

                if (_currentStack.Count == 0)
                {
                    ReverseStacks(_currentStack, otherStack);
                }

                var corridorVideos = _currentStack.Pop();
                _currentStack = otherStack;
                ReloadVideos(corridor, corridorVideos);
            }
            else
            {
                if (_videosToPlay.Count > 0 && otherStack.Count == 0)
                {
                    SelectRandomVideos(corridor);
                }
                else
                {
                    if (otherStack.Count == 0)
                    {
                        ReverseStacks(otherStack, _currentStack);
                    }

                    ReloadVideos(corridor, otherStack.Pop());
                }
            }

            _lastVisitedCorridor = _activeCorridor;
            _activeCorridor = corridor;
        }

        private void SelectRandomVideos(Transform corridor)
        {
            var corridorVideos = new Dictionary<string, string>();

            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                var videoUrl = _videosToPlay[Random.Range(0, _videosToPlay.Count - 1)];

                _videosToPlay.Remove(videoUrl);
                corridorVideos.Add(videoPlayer.name, videoUrl);

                PlayVideo(videoPlayer, videoUrl);
            }

            _currentStack.Push(corridorVideos);
        }

        private void ReloadVideos(Transform corridor, Dictionary<string, string> corridorVideos)
        {
            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                PlayVideo(videoPlayer, corridorVideos[videoPlayer.name]);
            }

            _currentStack.Push(corridorVideos);
        }

        private void PlayVideo(VideoPlayer videoPlayer, string videoUrl)
        {
            videoPlayer.url = videoUrl;
            videoPlayer.SetDirectAudioVolume(0, 0.0f);
            videoPlayer.SetDirectAudioMute(0, true);

            if (_timestamps.ContainsKey(videoUrl))
            {
                videoPlayer.time = _timestamps[videoUrl];
            }

            videoPlayer.Play();
        }

        private void StopVideos(Transform corridor, Dictionary<string, string> corridorVideos)
        {
            foreach (var videoPlayer in corridor.GetComponentsInChildren<VideoPlayer>())
            {
                if (_timestamps.ContainsKey(videoPlayer.url))
                {
                    _timestamps[videoPlayer.url] = videoPlayer.time;
                }
                else
                {
                    _timestamps.Add(videoPlayer.url, videoPlayer.time);
                }

                videoPlayer.SetDirectAudioVolume(0, 0.0f);
                videoPlayer.SetDirectAudioMute(0, true);
                videoPlayer.Stop();
                videoPlayer.targetTexture.Release();
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

        private void ReverseStacks(Stack<Dictionary<string, string>> emptyStack, Stack<Dictionary<string, string>> fullStack)
        {
            while (fullStack.Count > 0)
            {
                emptyStack.Push(fullStack.Pop());
            }
        }
    }
}