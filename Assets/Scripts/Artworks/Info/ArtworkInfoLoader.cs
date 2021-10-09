using System.IO;
using UnityEngine;

namespace Gretas.Artworks.Info
{
    public class ArtworkInfoLoader : MonoBehaviour
    {
        [SerializeField] private ArtworkDatabaseScriptable _artworkDatabase;

        private readonly string _directory = "/Database/";
        private readonly string _fileName = "artworkData";
        private readonly string _fileExtension = ".json";

        public ArtworkInfo GetArtworkInfo(string frameId)
        {
            foreach (var artworkInfo in _artworkDatabase.ArtworksInfo)
            {
                if (artworkInfo.frameId == frameId)
                {
                    return artworkInfo;
                }
            }

            return LoadArtworkInfo(frameId);
        }

        private ArtworkInfo LoadArtworkInfo(string frameId)
        {
            var fullPath = Application.persistentDataPath + _directory + _fileName + frameId + _fileExtension;

            if (File.Exists(fullPath))
            {
                var artworkInfo = JsonUtility.FromJson<ArtworkInfo>(File.ReadAllText(fullPath));

                _artworkDatabase.ArtworksInfo.Add(artworkInfo);

                return artworkInfo;
            }

            Debug.LogWarning("The corresponding artwork could not be found in our database.");

            return null;
        }
    }
}