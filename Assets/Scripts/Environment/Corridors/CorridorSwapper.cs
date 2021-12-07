using System;
using Gretas.Environment.Corridors.Triggers;
using UnityEngine;

namespace Gretas.Environment.Corridors
{
    public class CorridorSwapper : MonoBehaviour
    {
        public event Action<Transform> OnActiveCorridor = delegate { };

        [SerializeField] private Transform _frontCorridor;
        [SerializeField] private Transform _backCorridor;

        private Vector3 _meshSize;

        public Transform FrontCorridor => _frontCorridor;

        private void OnEnable()
        {
            GetComponentInChildren<CorridorTrigger>().OnMoveForward += PlaceCorridorInFront;
            GetComponentInChildren<CorridorTrigger>().OnMoveBackward += PlaceCorridorInBack;
        }

        private void Start()
        {
            _meshSize = GetComponent<Renderer>().bounds.size;
        }

        private void OnDisable()
        {
            GetComponentInChildren<CorridorTrigger>().OnMoveForward -= PlaceCorridorInFront;
            GetComponentInChildren<CorridorTrigger>().OnMoveBackward -= PlaceCorridorInBack;
        }

        private void PlaceCorridorInFront()
        {
            _backCorridor.position = new Vector3(transform.position.x + _meshSize.x * 2, transform.position.y, transform.position.z);

            CorridorManager.Instance.PlaceCorridorEnds(_backCorridor, transform, _meshSize.x);

            OnActiveCorridor(_frontCorridor);
        }

        private void PlaceCorridorInBack()
        {
            var firstCorridor = _frontCorridor.GetComponent<CorridorSwapper>().FrontCorridor;
            firstCorridor.position = new Vector3(transform.position.x - _meshSize.x, transform.position.y, transform.position.z);

            CorridorManager.Instance.PlaceCorridorEnds(_frontCorridor, firstCorridor, _meshSize.x);

            OnActiveCorridor(transform);
        }
    }
}