using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProperties", menuName = "Scriptable Objects/CharacterProperties")]
public class CharacterProperties : ScriptableObject
{
    public string characterName;
    public Color characterColor;
    public GameObject spawnObject;
}
