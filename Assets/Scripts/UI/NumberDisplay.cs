using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays a number with a specified format
/// </summary>
public class NumberDisplay : MonoBehaviour
{
    [SerializeField] private string format;
    [SerializeField] private float number;
    [SerializeField] private TMP_Text text;

    public void SetNumber(float number)
    {
        this.number = number;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        text.text = string.Format(format, number);
    }

    private void OnValidate()
    {
        UpdateDisplay();
    }
}
