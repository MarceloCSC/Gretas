using UnityEngine;

namespace Gretas.Environment
{
    public class CorridorSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _frontCorridor;
        [SerializeField] private Transform _backCorridor;

        private Vector3 _meshSize;

        public Transform FrontCorridor => _frontCorridor;

        private void OnEnable()
        {
            GetComponentInChildren<CorridorTrigger>().OnMoveForward += PositionCorridorAsFirst;
            GetComponentInChildren<CorridorTrigger>().OnMoveBackward += PositionCorridorAsLast;
        }

        private void Start()
        {
            _meshSize = GetComponent<Renderer>().bounds.size;
        }

        private void OnDisable()
        {
            GetComponentInChildren<CorridorTrigger>().OnMoveForward -= PositionCorridorAsFirst;
            GetComponentInChildren<CorridorTrigger>().OnMoveBackward -= PositionCorridorAsLast;
        }

        private void PositionCorridorAsFirst()
        {
            _backCorridor.position = new Vector3(transform.position.x + _meshSize.x * 2, transform.position.y, transform.position.z);
        }

        private void PositionCorridorAsLast()
        {
            var firstCorridor = _frontCorridor.GetComponent<CorridorSpawner>().FrontCorridor;
            firstCorridor.position = new Vector3(transform.position.x - _meshSize.x, transform.position.y, transform.position.z);
        }
    }
}