using UnityEngine;

namespace Gretas.Environment.Corridors
{
    public class CorridorManager : MonoBehaviour
    {
        [SerializeField] private Transform _corridorHead;
        [SerializeField] private Transform _corridorTail;

        private static CorridorManager _instance;
        public static CorridorManager Instance => _instance;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject); return;
            }

            _instance = this;
        }

        public void PlaceCorridorEnds(Transform frontCorridor, Transform backCorridor, float meshSizeX)
        {
            _corridorHead.position = new Vector3(frontCorridor.position.x + meshSizeX, frontCorridor.position.y, frontCorridor.position.z);
            _corridorTail.position = new Vector3(backCorridor.position.x - meshSizeX, backCorridor.position.y, backCorridor.position.z);
        }
    }
}