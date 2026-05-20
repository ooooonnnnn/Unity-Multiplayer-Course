using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldDetectEmpty : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    public UnityEvent OnBecomeEmpty;
    public UnityEvent OnBecomeNonEmpty;

    private void OnValidate()
    {
        if (!inputField) inputField = GetComponent<TMP_InputField>();
    }

    private void Awake()
    {
        inputField.onValueChanged.AddListener(CheckStringEmpty);
    }

    private void CheckStringEmpty(string s)
    {
        if (string.IsNullOrEmpty(s)) OnBecomeEmpty.Invoke();
        else OnBecomeNonEmpty.Invoke();
    }
}
