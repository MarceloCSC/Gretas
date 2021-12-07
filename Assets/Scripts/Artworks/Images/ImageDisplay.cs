using UnityEngine;

namespace Gretas.Artworks.Images
{
    public class ImageDisplay : MonoBehaviour
    {
        [SerializeField] private string _imageId;
        [SerializeField] private Transform _surfaceToRender;

        public string Id => _imageId;
        public Transform SurfaceToRender => _surfaceToRender;
    }
}