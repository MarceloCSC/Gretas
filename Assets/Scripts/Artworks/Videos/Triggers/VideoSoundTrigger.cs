using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Gretas.Artworks.Videos.Triggers
{
    public class VideoSoundTrigger : MonoBehaviour
    {
        [SerializeField] private float _maxVolume;
        [SerializeField] private bool _hasAudio;

        private VideoPlayer _videoPlayer;

        private void Awake()
        {
            if (_hasAudio)
            {
                _videoPlayer = GetComponentInParent<VideoPlayer>();
                _videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasAudio && other.CompareTag("User"))
            {
                _videoPlayer.SetDirectAudioMute(0, false);
                StartCoroutine(ChangeVolume(_maxVolume, 0.001f));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_hasAudio && other.CompareTag("User"))
            {
                StartCoroutine(ChangeVolume(0.0f, -0.001f));
            }
        }

        private IEnumerator ChangeVolume(float desiredVolume, float increment)
        {
            float currentVolume = _videoPlayer.GetDirectAudioVolume(0);

            while (Mathf.Abs(currentVolume - desiredVolume) > 0.01f)
            {
                _videoPlayer.SetDirectAudioVolume(0, currentVolume += increment);

                yield return null;
            }

            _videoPlayer.SetDirectAudioMute(0, desiredVolume == 0.0f);
        }
    }
}