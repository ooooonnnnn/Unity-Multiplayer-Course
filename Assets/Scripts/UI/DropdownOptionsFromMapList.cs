using System.Linq;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownOptionsFromMapList : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private MapList mapList;

        private void OnValidate()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            if (!mapList) return;

            dropdown.options = mapList.GetMapNames().Select(
                mapName => new TMP_Dropdown.OptionData(mapName))
                .ToList();
        }
    }
}