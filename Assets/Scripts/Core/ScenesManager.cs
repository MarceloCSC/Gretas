using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Gretas.Core
{
    public class ScenesManager : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                LoadNewScene();
            }
        }

        private void LoadNewScene()
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}