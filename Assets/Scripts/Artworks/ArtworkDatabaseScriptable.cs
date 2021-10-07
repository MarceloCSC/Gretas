using System.Collections.Generic;
using UnityEngine;

namespace Gretas.Artworks
{
    [CreateAssetMenu(fileName = "NewArtworkDatabase", menuName = "New Artwork Database")]
    public class ArtworkDatabaseScriptable : ScriptableObject
    {
        [SerializeField] private List<Artwork> _artworks;

        public List<Artwork> Artworks => _artworks;
    }
}