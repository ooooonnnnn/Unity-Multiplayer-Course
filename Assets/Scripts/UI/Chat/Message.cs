using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private Text messageText;

    public void SetText(string text)
    {
        messageText.text = text;
    }
}
