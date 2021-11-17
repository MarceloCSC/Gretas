using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Images
{
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] private ImageDisplay[] _images;

        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/images";

        private void Start()
        {
            if (_images.Length == 0)
            {
                _images = FindObjectsOfType<ImageDisplay>();
            }

            foreach (var image in _images)
            {
                image.GetComponentInChildren<ImageOptimizationManager>().OnProximity += OptimizeImage;

                //string url = GetUrl(image.Id, ImageQuality.Low);
                //StartCoroutine(GetTextureRequest(url, image));
            }
        }

        private void OptimizeImage(string imageId)
        {
            foreach (var image in _images)
            {
                if (image.Id == imageId)
                {
                    StartCoroutine(GetTextureRequest(GetUrl(imageId, ImageQuality.Medium), image));
                }
            }
        }

        private IEnumerator GetTextureRequest(string url, ImageDisplay image)
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
                    var downloadedTexture = DownloadHandlerTexture.GetContent(webRequest);

                    var texture = new Texture2D(downloadedTexture.width, downloadedTexture.height);

                    texture.SetPixels(downloadedTexture.GetPixels());
                    texture.Apply();

                    var material = new Material(Shader.Find("Unlit/Texture"))
                    {
                        mainTexture = texture
                    };

                    image.GetComponent<MeshRenderer>().material = material;
                    //imageFrame.transform.localScale = new Vector3(8.0f, GetHeight(texture.width, texture.height), 1);
                }
            }
        }

        private string GetUrl(string imageId, ImageQuality quality)
        {
            return $"{_path}/{quality.ToString().ToLower()}/{imageId}.png";
        }

        private float GetHeight(float width, float height)
        {
            return height * 8.0f / width;
        }
    }
}