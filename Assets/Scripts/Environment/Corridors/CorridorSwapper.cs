using Gretas.Environment.Corridors.Triggers;
using System;
using UnityEngine;

namespace Gretas.Environment.Corridors
{
    public class CorridorSwapper : MonoBehaviour
    {
        public event Action<Transform> OnActiveCorridor = delegate { };

        [SerializeField] private Transform _frontCorridor;
        [SerializeField] private Transform _backCorridor;

        private Vector3 _meshSize;
        private CorridorTrigger _corridorTrigger;
        public Transform FrontCorridor => _frontCorridor;

        private void Awake()
        {
            _corridorTrigger = GetComponentInChildren<CorridorTrigger>(true);
        }

        private void OnEnable()
        {
            if (_corridorTrigger == null)
            {
                _corridorTrigger = GetComponentInChildren<CorridorTrigger>(true);
            }

            if (_corridorTrigger == null)
            {
                Debug.LogError($"CorridorTrigger not found in {name}", this);
                return;
            }

            _corridorTrigger.OnMoveForward += PlaceCorridorInFront;
            _corridorTrigger.OnMoveBackward += PlaceCorridorInBack;
        }

        private void Start()
        {
            _meshSize = GetComponent<Renderer>().bounds.size;
        }

        private void OnDisable()
        {
            if (_corridorTrigger == null)
            {
                return;
            }

            _corridorTrigger.OnMoveForward -= PlaceCorridorInFront;
            _corridorTrigger.OnMoveBackward -= PlaceCorridorInBack;
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