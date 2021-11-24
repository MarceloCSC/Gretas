using UnityEngine;

namespace Gretas.Artworks.Labels
{
    public class LabelDisplay : MonoBehaviour, IDisplay
    {
        [SerializeField] private string _artworkId;

        public string Id => _artworkId;
    }
}