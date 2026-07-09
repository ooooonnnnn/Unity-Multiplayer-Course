using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MapList", menuName = "Scriptable Objects/MapList", order = 1)]
    public class MapList : ScriptableObject
    {
        [SerializeField] private Map[] maps;

        public IEnumerable<string> GetMapNames() => maps.Select(map => map.MapName);
    }
}