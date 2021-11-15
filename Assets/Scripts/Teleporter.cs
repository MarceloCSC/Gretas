using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform _destination;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("User"))
        {
            var relativePosition = transform.InverseTransformPoint(other.transform.position);
            var newPosition = new Vector3(-relativePosition.x, relativePosition.y, relativePosition.z);

            print("Teleported!");

            other.transform.SetPositionAndRotation(_destination.TransformPoint(newPosition), other.transform.rotation * Quaternion.Euler(0, 180.0f, 0));
        }
    }
}