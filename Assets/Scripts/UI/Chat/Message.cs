using System;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private Text messageText;

    private void Start()
    {
    }

    public void SetText(string text)
    {
        messageText.text = text;
    }
}
