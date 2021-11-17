using UnityEngine;

namespace Gretas.Artworks.Info
{
    public class InfoLabel : MonoBehaviour, IDisplay
    {
        [SerializeField] private string _artworkId;

        public string Id => _artworkId;
    }
}