using System;
using System.Collections.Generic;
using System.Linq;

namespace Braindrops.AdventureToolkit.Traversal.Locations
{
    public static class LocationTracker
    {
        private static List<LocatedEntity> entities = new List<LocatedEntity>();

        public static void RegisterEntity(LocatedEntity locatedEntity)
        {
            entities.Add(locatedEntity);
        }

        public static void UnregisterEntity(LocatedEntity locatedEntity)
        {
            entities.Remove(locatedEntity);
        }

        public static Locations GetEntityLocation(string characterName)
        {
            var character = GetEntityByName(characterName);
            return character == null ? Locations.Unspecified : character.CurrentLocation;
        }

        public static void AddLocationChangeListener(string characterName, Action<Locations> callback)
        {
            var character = GetEntityByName(characterName);
            if (character != null)
                character.AddLocationChangeListener(callback);
        }

        private static LocatedEntity GetEntityByName(string characterName)
        {
            return entities.FirstOrDefault(entity => entity.EntityName == characterName);
        }
    }
}