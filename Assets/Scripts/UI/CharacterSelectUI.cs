using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _characterBtnPrefab;
    [SerializeField]
    private Transform _characterBtnsParent;

    public event UnityAction<int> OnSelectedCharacter;

    private HashSet<CharacterButton> _buttons = new();

    private int[] _charactersSelected = Array.Empty<int>();

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void PopulateSelection(CharacterProperties[] characters)
    {
        foreach (Transform child in _characterBtnsParent)
        {
            Destroy(child.gameObject);
        }

        _buttons.Clear();

        foreach (var character in characters)
        {
            var button = Instantiate(_characterBtnPrefab, _characterBtnsParent).GetComponent<CharacterButton>();
            button.Setup(character);

            button.OnSelected += SelectCharacter;

            _buttons.Add(button);
        }

        UpdateSelectedCharacters(_charactersSelected);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    private void SelectCharacter(int characterID)
    {
        OnSelectedCharacter?.Invoke(characterID);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSelectedCharacters(int[] charactersSelected)
    {
        _charactersSelected = charactersSelected;

        foreach (var btn in _buttons)
        {
            if (btn.MyCharacterID == null) continue;

            btn.SetEnabled(!charactersSelected.Contains(btn.MyCharacterID.Value));
        }
    }
}
