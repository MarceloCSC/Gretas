using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Image
{
    public class ArtworkImageLoader : MonoBehaviour
    {
        [SerializeField] private ImageFrame[] _artworks;

        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/images";

        private void Start()
        {
            _artworks = FindObjectsOfType<ImageFrame>();

            foreach (var artwork in _artworks)
            {
                string url = GetUrl(artwork.FrameId, ImageQuality.Low);
                StartCoroutine(GetTextureRequest(url, artwork));
            }
        }

        private IEnumerator GetTextureRequest(string url, ImageFrame artwork)
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
                    artwork.transform.localScale = new Vector3(8.0f, GetHeight(texture.width, texture.height), 1);
                }
            }
        }

        private string GetUrl(string id, ImageQuality quality)
        {
            return $"{_path}/{quality.ToString().ToLower()}/{id}.png";
        }

        private float GetHeight(float width, float height)
        {
            return height * 8.0f / width;
        }
    }
}