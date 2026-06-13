using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToMenuButton : MonoBehaviour
{
    [SerializeField, HideInInspector] private Button button;

    private void OnValidate()
    {
        button = GetComponent<Button>();
    }

    public void Start()
    {
        button.onClick.AddListener(GameManager.Instance.ReturnToMenu);
    }
}
