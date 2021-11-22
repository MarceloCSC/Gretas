using System.Collections.Generic;
using UnityEngine;

namespace Gretas.Core
{
    public class DataCache : MonoBehaviour
    {
        private Dictionary<string, Texture2D> _textures;
        private static DataCache _instance;

        public Dictionary<string, Texture2D> Textures => _textures;
        public static DataCache Instance => _instance;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);

                if (_textures == null)
                {
                    _textures = new Dictionary<string, Texture2D>();
                }
            }
        }
    }
}