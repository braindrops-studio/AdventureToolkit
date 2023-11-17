using System;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.Locations
{
    [RequireComponent(typeof(Collider2D))]
    public class LocationTrigger : MonoBehaviour
    {
        [SerializeField] private Locations locationName;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var locatedEntity = other.GetComponent<LocatedEntity>();
            if (locatedEntity != null)
                locatedEntity.CurrentLocation = locationName;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var locatedEntity = other.GetComponent<LocatedEntity>();
            if (locatedEntity != null)
                locatedEntity.CurrentLocation = Locations.Unspecified;
        }
    }
}