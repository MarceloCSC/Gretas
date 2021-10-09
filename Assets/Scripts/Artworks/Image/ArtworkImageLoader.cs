using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Image
{
    public class ArtworkImageLoader : MonoBehaviour
    {
        [SerializeField] private ArtworkFrame[] _artworks;
        [SerializeField] private string _url;

        private void Start()
        {
            foreach (var artwork in _artworks)
            {
                StartCoroutine(GetTextureRequest(_url, artwork));
            }
        }

        private IEnumerator GetTextureRequest(string url, ArtworkFrame artwork)
        {
            using var webRequest = UnityWebRequestTexture.GetTexture(url);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(webRequest);

                    var material = new Material(Shader.Find("Unlit/Texture"))
                    {
                        mainTexture = texture
                    };

                    artwork.GetComponent<MeshRenderer>().material = material;
                    artwork.transform.localScale = new Vector3(texture.width / 150.0f, texture.height / 150.0f, 1);
                }
            }
        }
    }
}