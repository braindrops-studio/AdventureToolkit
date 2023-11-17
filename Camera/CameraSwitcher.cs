using System.Linq;
using Braindrops.AdventureToolkit.Traversal.Locations;
using Cinemachine;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Camera
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CameraData defaultCamera;
        [SerializeField] private CameraData[] cameras;

        private CinemachineVirtualCamera currentCamera;

        private void Awake()
        {
            currentCamera = defaultCamera.virtualCamera;
            currentCamera.Priority = 0;
        }

        public void SwitchToCamera(Locations locationName)
        {
            currentCamera.Priority = -1;
            currentCamera = FindCameraByLocation(locationName);
            currentCamera.Priority = 0;
        }

        private CinemachineVirtualCamera FindCameraByLocation(Locations location)
        {
            return (from cameraData in cameras where cameraData.location == location select cameraData.virtualCamera).FirstOrDefault();
        }
    }
}