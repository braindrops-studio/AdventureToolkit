using System;
using System.Collections.Generic;
using System.Linq;
using Braindrops.Unolith.ServiceLocator;
using Cycas.Library;
using UnityEngine.Events;

namespace Braindrops.AdventureToolkit.Traversal.Locations
{
    public class LocationTracker : GameService
    {
        private List<LocatedEntity> entities = new List<LocatedEntity>();

        public void RegisterEntity(LocatedEntity locatedEntity)
        {
            if (entities.Contains(locatedEntity))
                return;
            entities.Add(locatedEntity);
        }

        public void UnregisterEntity(LocatedEntity locatedEntity)
        {
            entities.Remove(locatedEntity);
        }

        public Locations GetEntityLocation(CharacterNames characterName)
        {
            var character = GetEntityByName(characterName);
            return character == null ? Locations.Unspecified : character.CurrentLocation;
        }

        public void AddLocationChangeListener(CharacterNames characterName, UnityAction<Locations> callback)
        {
            var character = GetEntityByName(characterName);
            if (character != null)
                character.AddLocationChangeListener(callback);
        }

        private LocatedEntity GetEntityByName(CharacterNames characterName)
        {
            return entities.FirstOrDefault(entity => entity.EntityName == characterName);
        }
    }
}