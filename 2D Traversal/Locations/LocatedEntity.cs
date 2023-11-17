using System;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.Locations
{
    public class LocatedEntity : MonoBehaviour
    {
        private string entityName = "";
        private Locations currentLocation = Locations.Unspecified;
        private Action<Locations> onLocationChange = locationName => {};

        public void Setup(string characterName)
        {
            this.entityName = characterName;
            LocationTracker.RegisterEntity(this);
        }

        public void AddLocationChangeListener(Action<Locations> callback)
        {
            onLocationChange += callback;
        }

        public string EntityName => entityName;

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