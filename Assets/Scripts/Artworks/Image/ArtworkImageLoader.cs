using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Gretas.Artworks.Image
{
    public class ArtworkImageLoader : MonoBehaviour
    {
        [SerializeField] private ImageFrame[] _artworks;

        private void Start()
        {
            _artworks = FindObjectsOfType<ImageFrame>();

            foreach (var artwork in _artworks)
            {
                string url = FindUrl(artwork.FrameId);
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

        private string FindUrl(string id)
        {
            return id switch
            {
                "0001" => "https://i2.wp.com/profissaobiotec.com.br/wp-content/uploads/2020/01/cat-4262034_1920.jpg?fit=1088%2C725&ssl=1",
                "0002" => "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/cute-photos-of-cats-cuddling-1593203046.jpg",
                "0003" => "https://www.rd.com/wp-content/uploads/2021/04/GettyImages-145679137-scaled-e1619025176434.jpg",
                "0004" => "https://www.metrovetchicago.com/sites/default/files/08-cat-cancer-4.jpeg",
                "0005" => "https://image.cnbcfm.com/api/v1/image/105828578-1554223245858gettyimages-149052633.jpeg?v=1554223281",
                "0006" => "https://imagesvc.meredithcorp.io/v3/mm/image?url=https%3A%2F%2Fstatic.onecms.io%2Fwp-content%2Fuploads%2Fsites%2F34%2F2021%2F04%2F23%2Fclose-up-cats-eye-getty-0421-2000.jpg",
                "0007" => "https://imagesvc.meredithcorp.io/v3/mm/image?url=https%3A%2F%2Fstatic.onecms.io%2Fwp-content%2Fuploads%2Fsites%2F37%2F2021%2F03%2F15%2Fsiamese-cat-names.jpg",
                "0008" => "https://www.youtube.com/watch?v=mRLfyc5Mi7U",
                _ => string.Empty,
            };
        }

        private float GetHeight(float width, float height)
        {
            return height * 8.0f / width;
        }
    }
}