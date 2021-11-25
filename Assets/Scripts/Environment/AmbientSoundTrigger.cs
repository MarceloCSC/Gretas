using System.Collections;
using UnityEngine;

namespace Gretas.Environment
{
    public class AmbientSoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private float _maxVolume;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audioSource.clip = _audioClip;
            _audioSource.mute = true;
            _audioSource.volume = 0;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("User"))
            {
                if (Vector3.Dot(transform.forward, transform.position - other.transform.position) < 0.0f)
                {
                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play();
                    }

                    _audioSource.mute = false;
                    StartCoroutine(ChangeVolume(_maxVolume, 0.001f));
                }
                else
                {
                    StartCoroutine(ChangeVolume(0.0f, -0.01f));
                }
            }
        }

        private IEnumerator ChangeVolume(float desiredVolume, float increment)
        {
            while (Mathf.Abs(_audioSource.volume - desiredVolume) > 0.01f)
            {
                _audioSource.volume += increment;

                yield return null;
            }

            _audioSource.mute = desiredVolume == 0.0f;
        }
    }
}