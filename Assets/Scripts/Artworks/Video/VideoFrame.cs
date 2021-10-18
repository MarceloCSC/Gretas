using UnityEngine;

namespace Gretas.Artworks.Video
{
    public class VideoFrame : MonoBehaviour, IFrame
    {
        [SerializeField] private string _frameId;

        public string FrameId => _frameId;
    }
}