using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Map", menuName = "Scriptable Objects/Map", order = 0)]
    public class Map : ScriptableObject
    {
        [SerializeField] private string mapName;
        
        public string MapName => mapName;
        
        public Map(string name)
        {
            mapName = name;
        }
    }
}