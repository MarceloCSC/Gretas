using UnityEngine;

namespace Gretas.Artworks
{
    public class ArtworkFrame : MonoBehaviour
    {
        [SerializeField] private string _frameId;

        public string FrameId => _frameId;
    }
}