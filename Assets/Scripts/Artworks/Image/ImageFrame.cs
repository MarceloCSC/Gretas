using UnityEngine;

namespace Gretas.Artworks.Image
{
    public class ImageFrame : MonoBehaviour, IFrame
    {
        [SerializeField] private string _frameId;

        public string FrameId => _frameId;
    }
}