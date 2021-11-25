using System.Reflection;
using Gretas.Artworks.Images;
using UnityEditor;
using UnityEngine;

namespace Gretas.Core.Utils
{
    public class ImageResizer : MonoBehaviour
    {
#if UNITY_EDITOR

        private enum ImageSize
        {
            XtraSmall = 1,
            Small = 2,
            Medium = 0,
            Large = 3,
            XtraLarge = 4,
        }

        [SerializeField] private Material _material;
        [SerializeField] private ImageSize _size;
        [SerializeField] private bool _isResizerActive;
        [SerializeField] private bool _hasBeenResized;
        [SerializeField] private bool _isMaterialSet;

        private void OnValidate()
        {
            if (_isResizerActive && _material && !_hasBeenResized)
            {
                var surfaceToRender = GetComponent<ImageDisplay>().SurfaceToRender;

                //if (!_isMaterialSet)
                //{
                //    surfaceToRender.GetComponent<MeshRenderer>().material = _material;
                //    _isMaterialSet = true;
                //}

                if (!_hasBeenResized)
                {
                    _hasBeenResized = true;

                    GetImageOriginalSize(_material.mainTexture as Texture2D, out int originalWidth, out int originalHeight);

                    if (originalWidth != 0 && originalHeight != 0)
                    {
                        float newWidth = GetWidth(originalWidth > originalHeight);
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

        private float GetWidth(bool isHorizontal)
        {
            float newWidth = 0;

            switch (_size)
            {
                case ImageSize.XtraSmall:
                    newWidth = isHorizontal ? 3.15f : 2.25f;
                    break;

                case ImageSize.Small:
                    newWidth = isHorizontal ? 4.2f : 2.75f;
                    break;

                case ImageSize.Medium:
                    newWidth = isHorizontal ? 8.2f : 4.5f;
                    break;

                case ImageSize.Large:
                    newWidth = isHorizontal ? 9.45f : 5.25f;
                    break;

                case ImageSize.XtraLarge:
                    newWidth = isHorizontal ? 12.3f : 6.3f;
                    break;

                default:
                    break;
            }

            return newWidth;
        }

#endif
    }
}