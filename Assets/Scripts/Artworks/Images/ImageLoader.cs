using System.Collections;
using System.Collections.Generic;
using Gretas.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Images
{
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] private ImageDisplay[] _images;

        private List<string> _requestedImages;
        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/images";

        private void Awake()
        {
            if (_images.Length == 0)
            {
                _images = FindObjectsOfType<ImageDisplay>();
            }

            _requestedImages = new List<string>();
        }

        private void Start()
        {
            foreach (var image in _images)
            {
                if (DataCache.Instance.Textures.TryGetValue($"{image.Id}-medium", out Texture2D texture))
                {
                    var material = new Material(Shader.Find("Unlit/Texture"))
                    {
                        mainTexture = texture
                    };

                    image.GetComponent<MeshRenderer>().material = material;
                }
                else
                {
                    image.GetComponentInChildren<ImageOptimizationTrigger>().OnProximity += OptimizeImage;
                }
            }
        }

        public void GetHighResTexture(ImageDisplay image)
        {
            if (!_requestedImages.Contains(image.Id))
            {
                _requestedImages.Add(image.Id);
                StartCoroutine(GetTextureRequest(image, ImageQuality.High));
            }
        }

        private void OptimizeImage(string imageId)
        {
            foreach (var image in _images)
            {
                if (image.Id == imageId)
                {
                    StartCoroutine(GetTextureRequest(image, ImageQuality.Medium));
                }
            }
        }

        private IEnumerator GetTextureRequest(ImageDisplay image, ImageQuality quality)
        {
            string url = $"{_path}/{quality.ToString().ToLower()}/{image.Id}.png";

            using var webRequest = UnityWebRequestTexture.GetTexture(url);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);

                if (_requestedImages.Contains(image.Id))
                {
                    _requestedImages.Remove(image.Id);
                }
            }
            else
            {
                if (webRequest.isDone)
                {
                    var downloadedTexture = DownloadHandlerTexture.GetContent(webRequest);

                    var texture = new Texture2D(downloadedTexture.width, downloadedTexture.height);

                    texture.SetPixels32(downloadedTexture.GetPixels32());
                    texture.Apply();

                    if (quality == ImageQuality.Medium)
                    {
                        CreateMaterial(image, texture);
                    }
                    else if (quality == ImageQuality.High)
                    {
                    }

                    DataCache.Instance.Textures.Add($"{image.Id}-{quality.ToString().ToLower()}", texture);
                }
            }
        }

        private void CreateMaterial(ImageDisplay image, Texture2D texture)
        {
            var material = new Material(Shader.Find("Unlit/Texture"))
            {
                mainTexture = texture
            };

            image.GetComponent<MeshRenderer>().material = material;
        }
    }
}