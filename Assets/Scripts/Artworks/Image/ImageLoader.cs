using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Image
{
    public class ImageLoader : MonoBehaviour
    {
        [SerializeField] private ImageFrame[] _imageFrames;

        private const string _path = "https://gretasgaleria.blob.core.windows.net/data/images";

        private void Start()
        {
            _imageFrames = FindObjectsOfType<ImageFrame>();

            foreach (var imageFrame in _imageFrames)
            {
                imageFrame.GetComponentInChildren<ImageOptimizationManager>().OnProximity += OptimizeImage;

                string url = GetUrl(imageFrame.FrameId, ImageQuality.Low);
                StartCoroutine(GetTextureRequest(url, imageFrame));
            }
        }

        private IEnumerator GetTextureRequest(string url, ImageFrame imageFrame)
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

                    imageFrame.GetComponent<MeshRenderer>().material = material;
                    //imageFrame.transform.localScale = new Vector3(8.0f, GetHeight(texture.width, texture.height), 1);
                }
            }
        }

        private string GetUrl(string frameId, ImageQuality quality)
        {
            return $"{_path}/{quality.ToString().ToLower()}/{frameId}.png";
        }

        private float GetHeight(float width, float height)
        {
            return height * 8.0f / width;
        }

        private void OptimizeImage(string frameId)
        {
            foreach (var imageFrame in _imageFrames)
            {
                if (imageFrame.FrameId == frameId)
                {
                    StartCoroutine(GetTextureRequest(GetUrl(frameId, ImageQuality.Medium), imageFrame));
                }
            }
        }
    }
}