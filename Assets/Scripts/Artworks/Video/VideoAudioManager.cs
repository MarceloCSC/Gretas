using UnityEngine;

namespace Gretas.Artworks.Video
{
    public class VideoAudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                _audioSource.mute = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                _audioSource.mute = true;
            }
        }
    }
}