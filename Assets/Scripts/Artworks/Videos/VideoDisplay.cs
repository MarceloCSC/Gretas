using UnityEngine;

namespace Gretas.Artworks.Videos
{
    public class VideoDisplay : MonoBehaviour, IDisplay
    {
        [SerializeField] private string _videoId;

        public string Id => _videoId;
    }
}