using UnityEngine;

namespace Braindrops.AdventureToolkit.Camera
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] public Vector3 movementScale;

        Vector3 initialOffset;
        Vector3 previousCameraPosition;

        private void Awake() {
            initialOffset = transform.position - mainCamera.position;
            initialOffset.z = 0;
            previousCameraPosition = mainCamera.transform.position;
        }

        private void LateUpdate() {
            var currentCameraPosition = mainCamera.transform.position;
            transform.position += Vector3.Scale(currentCameraPosition - previousCameraPosition, movementScale);
            previousCameraPosition = currentCameraPosition;
        } 
    }
}