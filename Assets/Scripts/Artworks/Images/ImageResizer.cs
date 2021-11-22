using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gretas.Artworks.Images
{
    public class ImageResizer : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private bool _hasBeenResized;
        [SerializeField] private bool _isResizerActive;

        private void OnValidate()
        {
            if (_isResizerActive && _material && !_hasBeenResized)
            {
                var surfaceToRender = GetComponent<ImageDisplay>().SurfaceToRender;

                surfaceToRender.GetComponent<MeshRenderer>().material = _material;

                if (!_hasBeenResized)
                {
                    _hasBeenResized = true;

                    GetImageOriginalSize(_material.mainTexture as Texture2D, out int originalWidth, out int originalHeight);

                    if (originalWidth != 0 && originalHeight != 0)
                    {
                        float newWidth = (float)Mathf.Round(transform.localScale.x * 100.0f) / 100.0f;
                        float newHeight = newWidth * ((float)originalHeight / originalWidth);

                        transform.localScale = Vector3.one;
                        surfaceToRender.localScale = new Vector3(newWidth, newHeight, 1.0f);
                        transform.GetChild(1).localScale = new Vector3(newWidth, newHeight, 0.15f);
                        transform.GetChild(2).localScale = new Vector3(newWidth * 2, newHeight, 9.0f);
                        transform.GetChild(2).localPosition = new Vector3(0, GetGroundLevelPosition(transform, transform.GetChild(2)), -4.5f);
                    }
                }
            }
        }

        private void GetImageOriginalSize(Texture2D texture, out int width, out int height)
        {
            if (texture != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(texture);
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (importer != null)
                {
                    object[] args = new object[2] { 0, 0 };
                    var methodInfo = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                    methodInfo.Invoke(importer, args);

                    width = (int)args[0];
                    height = (int)args[1];

                    return;
                }
            }

            height = width = 0;
        }

        private float GetGroundLevelPosition(Transform parent, Transform child)
        {
            return -(parent.localPosition.y - child.localScale.y / 2);
        }

    }
}