using System;
using TMPro;
using UnityEngine;

public class SetTextFromInt : MonoBehaviour
{
    [SerializeField, HideInInspector] private TMP_Text text;

    private void OnValidate()
    {
        text = GetComponent<TMP_Text>();
    }
    
    public void SetText(int value)
    {
        text.text = value.ToString();
    }
}
