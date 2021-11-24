using UnityEngine;

namespace Gretas.Environment
{
    public class AmbientSoundTrigger : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("User"))
            {
                if (Vector3.Dot(transform.forward, transform.position - other.transform.position) < 0.0f)
                {
                    if (_audioSource.isPlaying)
                    {
                        _audioSource.mute = false;
                    }
                    else
                    {
                        _audioSource.Play();
                    }
                }
                else
                {
                    _audioSource.mute = true;
                }
            }
        }
    }
}