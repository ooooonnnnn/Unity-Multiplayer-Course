using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public event UnityAction<int> OnSelected;

    private CharacterProperties _myCharacter;

    [SerializeField]
    private Image _btnSpr;
    [SerializeField]
    private Button _btn;
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _spawnName;

    public int? MyCharacterID => _myCharacter == null ? null : _myCharacter.CharacterID;

    public void Setup(CharacterProperties character)
    {
        _myCharacter = character;

        _btnSpr.color = character.characterColor;

        _name.text = $"Name: {character.characterName}";
        _spawnName.text = $"Spawns: {(character.spawnObject == null ? "None" : character.spawnObject.name)}";
    }

    public void OnClick()
    {
        if (_myCharacter == null) return;
        
        OnSelected?.Invoke(_myCharacter.CharacterID);
    }

    public void SetEnabled(bool enabled)
    {
        _btn.interactable = enabled;
    }
}
