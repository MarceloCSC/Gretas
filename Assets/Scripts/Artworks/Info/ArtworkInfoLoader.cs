using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Info
{
    public class ArtworkInfoLoader : MonoBehaviour
    {
        [SerializeField] private ArtworksInfo _artworksInfo;

        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/artworksInfo.json";

        private void Start()
        {
            StartCoroutine(GetInfoRequest());
        }

        public ArtworkInfo GetArtworkInfo(string frameId)
        {
            foreach (var artworkInfo in _artworksInfo.data)
            {
                if (artworkInfo.frameId == frameId)
                {
                    return artworkInfo;
                }
            }

            Debug.LogWarning("The corresponding artwork could not be found in our database.");

            return null;
        }

        private IEnumerator GetInfoRequest()
        {
            using var webRequest = UnityWebRequest.Get(_path);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    _artworksInfo = JsonUtility.FromJson<ArtworksInfo>(webRequest.downloadHandler.text);
                }
            }
        }
    }
}