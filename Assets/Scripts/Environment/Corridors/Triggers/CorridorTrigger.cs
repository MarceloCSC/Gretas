using System;
using UnityEngine;

namespace Gretas.Environment.Corridors.Triggers
{
    public class CorridorTrigger : MonoBehaviour
    {
        public event Action OnMoveForward = delegate { };
        public event Action OnMoveBackward = delegate { };

        private bool _isMovingForward;
        private bool _isMovingBackward;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("User"))
            {
                if (Vector3.Dot(transform.right, transform.position - other.transform.position) > 0.0f)
                {
                    _isMovingForward = true;
                }
                else
                {
                    _isMovingBackward = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("User"))
            {
                float dotProduct = Vector3.Dot(transform.right, transform.position - other.transform.position);

                if (dotProduct < 0.0f && _isMovingForward)
                {
                    OnMoveForward();
                }
                else if (dotProduct > 0.0f && _isMovingBackward)
                {
                    OnMoveBackward();
                }

                _isMovingForward = false;
                _isMovingBackward = false;
            }
        }
    }
}