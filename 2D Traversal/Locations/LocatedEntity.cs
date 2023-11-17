using Braindrops.Unolith.ServiceLocator;
using Cycas.Library;
using UnityEngine;
using UnityEngine.Events;

namespace Braindrops.AdventureToolkit.Traversal.Locations
{
    public class LocatedEntity : MonoBehaviour
    {
        [SerializeField] private CharacterNames entityName;
        private Locations currentLocation = Locations.Unspecified;
        public UnityEvent<Locations> onLocationChange;

        private LocationTracker locationTracker;

        private void Start()
        {
            locationTracker = ServiceLocator.Instance.GetService<LocationTracker>();
            locationTracker.RegisterEntity(this);
        }

        public void AddLocationChangeListener(UnityAction<Locations> callback)
        {
            onLocationChange.AddListener(callback);
        }

        public CharacterNames EntityName => entityName;

        public Locations CurrentLocation
        {
            get => currentLocation;
            set
            {
                currentLocation = value;
                onLocationChange.Invoke(value);
            }
        }
    }
}