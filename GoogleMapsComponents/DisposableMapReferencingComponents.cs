using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;

namespace GoogleMapsComponents
{
    internal static class DisposableMapReferencingComponents
    {
        private static readonly Dictionary<Map, List<IDisposable>> _componentsByMapLookup = new Dictionary<Map, List<IDisposable>>();
        private static readonly Dictionary<IDisposable, Map> _mapByComponentLookup = new Dictionary<IDisposable, Map>();

        internal static void AddOrUpdate(Map? map, IDisposable component)
        {
            Remove(component);

            if (map == null) return;
            
            if (!_componentsByMapLookup.TryGetValue(map, out var components))
            {
                components = new List<IDisposable>();
                _componentsByMapLookup.Add(map, components);
            }

            components.Add(component);
            _mapByComponentLookup.Add(component, map);
        }

        internal static void Remove(IDisposable component)
        {
            if (!_mapByComponentLookup.TryGetValue(component, out var map)) return;

            var componentsForMap = _componentsByMapLookup[map];

            if (componentsForMap.Count == 1)
            {
                _componentsByMapLookup.Remove(map);
            }
            else
            {
                componentsForMap.Remove(component);
            }

            _mapByComponentLookup.Remove(component);
        }

        internal static void DisposeAndRemoveComponentsReferencingMap(Map map)
        {
            if (!_componentsByMapLookup.TryGetValue(map, out var components)) return;
            
            _componentsByMapLookup.Remove(map);

            foreach (var component in components)
            {
                _mapByComponentLookup.Remove(component);
                component.Dispose();
            }
        }
    }
}
