using UnityEngine;

namespace Gretas.Artworks.Videos
{
    public class VideoDisplay : MonoBehaviour, IDisplay
    {
        [SerializeField] private string _videoId;
        [SerializeField] private Vector2 _resolution;

        public string Id => _videoId;
        public Vector2 Resolution => _resolution;
    }
}