using System.Collections;
using Gretas.User;
using UnityEngine;

namespace Gretas.Environment
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Transform _targetPortal;
        [SerializeField] private Camera _portalCamera;
        [SerializeField] private bool _isEnabled;

        private Camera _mainCamera;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            GetComponent<BoxCollider>().enabled = _isEnabled;

            if (_isEnabled)
            {
                SetCameraTexture();
            }
        }

        public void LateUpdate()
        {
            if (_isEnabled)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-_targetPortal.forward, _targetPortal.up);
                Quaternion cameraRotation = Quaternion.Inverse(transform.rotation) * targetRotation;
                Vector3 offset = _mainCamera.transform.position - transform.position;

                _portalCamera.transform.SetPositionAndRotation(_targetPortal.position + cameraRotation * offset, cameraRotation * _mainCamera.transform.rotation);
                _portalCamera.projectionMatrix = _mainCamera.projectionMatrix;
            }
        }

        public IEnumerator ActivatePortal()
        {
            yield return new WaitForSeconds(0.25f); // Pode melhorar

            SetCameraTexture();
            GetComponent<BoxCollider>().enabled = true;
            _isEnabled = true;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("User"))
            {
                if (Vector3.Dot(transform.forward, other.transform.position - transform.position) < 0.0f)
                {
                    other.GetComponent<CharacterController>().enabled = false; // ver possíveis problemas
                    TeleportUser(other.transform);
                    other.GetComponent<CharacterController>().enabled = true;

                    _isEnabled = false;
                    GetComponent<BoxCollider>().enabled = false; // Pode melhorar
                    StartCoroutine(_targetPortal.GetComponent<Portal>().ActivatePortal());
                }
            }
        }

        private void TeleportUser(Transform user)
        {
            Quaternion currentRotation = Quaternion.LookRotation(-transform.forward, transform.up);
            Quaternion targetRotation = Quaternion.Inverse(currentRotation) * _targetPortal.rotation;
            Vector3 offset = user.position - transform.position;

            user.SetPositionAndRotation(_targetPortal.position + targetRotation * offset, targetRotation * user.rotation);
            user.GetComponent<UserMovement>().MoveAfterTeleport(_targetPortal);
        }

        private void SetCameraTexture()
        {
            if (_portalCamera.targetTexture)
            {
                _portalCamera.targetTexture.Release();
            }

            _portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            GetComponent<MeshRenderer>().material.mainTexture = _portalCamera.targetTexture;
        }
    }
}