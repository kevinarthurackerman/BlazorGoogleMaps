using GoogleMapsComponents.Maps;
using System.Collections.Generic;

namespace GoogleMapsComponents
{
    internal static class MapObjectPool
    {
        private static readonly Stack<Map> _pooledMaps = new Stack<Map>();

        internal static bool TryCheckOut(out Map? map)
        {
            if (_pooledMaps.Count == 0)
            {
                map = null;
                return false;
            }

            map = _pooledMaps.Pop();
            return true;
        }

        internal static void CheckIn(Map map)
            => _pooledMaps.Push(map);
    }
}
