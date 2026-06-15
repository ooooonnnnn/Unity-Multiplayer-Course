using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class Chat : NetworkBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject Content;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string username = "Player";

    public void CallMessageRPC()
    {
        string message = inputField.text;
        if (string.IsNullOrWhiteSpace(message)) return;

        if (!Object || !Object.IsValid)
        {
            Debug.LogWarning("Chat NetworkObject not valid > message not sent over network!");
            return;
        }

        RPC_SendMessage(username, message);
        inputField.text = "";
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string username, string message, RpcInfo rpcInfo = default)
    {
        AddMessage(username, message);
    }

    private void AddMessage(string username, string message)
    {
        GameObject instantiate = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, Content.transform);
        Message messageComponent = instantiate.GetComponent<Message>();
        if (!messageComponent)
        {
            Debug.LogError("Message component missing on messagePrefab!");
            return;
        }
        messageComponent.SetText($"{username}: {messageComponent}");
    }
}
