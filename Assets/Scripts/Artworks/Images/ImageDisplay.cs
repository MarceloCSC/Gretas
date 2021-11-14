using UnityEngine;

namespace Gretas.Artworks.Images
{
    public class ImageDisplay : MonoBehaviour, IDisplay
    {
        [SerializeField] private string _imageId;

        public string Id => _imageId;
    }
}