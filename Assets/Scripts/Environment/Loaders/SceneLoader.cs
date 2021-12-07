using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gretas.Environment.Loaders
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Scene Load Panel")]
        [SerializeField] private GameObject _acceptButton;
        [SerializeField] private TextMeshProUGUI _messageDisplay;

        [Space]
        [SerializeField] private string _firstSceneMessage;
        [SerializeField] private string _otherScenesMessage;
        [SerializeField] private string _lastSceneMessage;

        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                _messageDisplay.text = _lastSceneMessage;
            }
            else if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                _messageDisplay.text = _otherScenesMessage;
            }
            else
            {
                _messageDisplay.text = _firstSceneMessage;
            }
        }

        private void OnEnable()
        {
            _acceptButton.GetComponent<Button>().onClick.AddListener(LoadNextScene);
        }

        private void OnDisable()
        {
            if (_acceptButton)
            {
                _acceptButton.GetComponent<Button>().onClick.RemoveListener(LoadNextScene);
            }
        }

        private void LoadNextScene()
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