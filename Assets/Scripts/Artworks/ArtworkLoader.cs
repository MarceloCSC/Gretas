using System.IO;
using UnityEngine;

namespace Gretas.Artworks
{
    public class ArtworkLoader : MonoBehaviour
    {
        [SerializeField] private ArtworkDatabaseScriptable _artworkDatabase;

        private readonly string _directory = "/Database/";
        private readonly string _fileName = "artworkData";
        private readonly string _fileExtension = ".json";

        public Artwork GetArtwork(string frameId)
        {
            foreach (var artwork in _artworkDatabase.Artworks)
            {
                if (artwork.frameId == frameId)
                {
                    return artwork;
                }
            }

            return LoadArtwork(frameId);
        }

        private Artwork LoadArtwork(string frameId)
        {
            var fullPath = Application.persistentDataPath + _directory + _fileName + frameId + _fileExtension;

            if (File.Exists(fullPath))
            {
                var artwork = JsonUtility.FromJson<Artwork>(File.ReadAllText(fullPath));

                _artworkDatabase.Artworks.Add(artwork);

                return artwork;
            }

            Debug.LogWarning("The corresponding artwork could not be found in our database.");

            return null;
        }
    }
}