using UnityEngine;

namespace Gretas.Artworks.Videos
{
    public class VideoSoundTrigger : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("User"))
            {
                _audioSource.mute = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("User"))
            {
                _audioSource.mute = true;
            }
        }
    }
}