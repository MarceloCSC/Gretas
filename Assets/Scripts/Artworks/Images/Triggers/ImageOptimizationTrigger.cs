using System;
using UnityEngine;

namespace Gretas.Artworks.Images.Triggers
{
    public class ImageOptimizationTrigger : MonoBehaviour
    {
        public event Action<string> OnProximity = delegate { };

        private bool _isOptimized;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOptimized && other.CompareTag("User"))
            {
                _isOptimized = true;
                OnProximity(GetComponentInParent<ImageDisplay>().Id);
            }
        }
    }
}