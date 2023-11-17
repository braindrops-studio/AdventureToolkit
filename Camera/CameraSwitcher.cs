using System.Linq;
using Braindrops.AdventureToolkit.Traversal.Locations;
using Braindrops.Unolith.ServiceLocator;
using Cinemachine;
using Cycas.Library;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Camera
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CameraData defaultCamera;
        [SerializeField] private CameraData[] cameras;

        private CinemachineVirtualCamera currentCamera;
        private LocationTracker locationTracker;
        
        private void Awake()
        {
            locationTracker = ServiceLocator.Instance.GetService<LocationTracker>();
            currentCamera = defaultCamera.virtualCamera;
            currentCamera.Priority = 0;
        }

        private void Start()
        {
            locationTracker.AddLocationChangeListener(CharacterNames.Floriqua, SwitchToCamera);
        }

        public void SwitchToCamera(Locations locationName)
        {
            currentCamera.Priority = -1;
            if (locationName == Locations.Unspecified)
                currentCamera = defaultCamera.virtualCamera;
            else
                currentCamera = FindCameraByLocation(locationName);
            currentCamera.Priority = 0;
        }

        private CinemachineVirtualCamera FindCameraByLocation(Locations location)
        {
            return (from cameraData in cameras where cameraData.location == location select cameraData.virtualCamera).FirstOrDefault();
        }
    }
}