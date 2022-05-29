using io.neuos;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private InputField ipField;
    [SerializeField]
    private InputField portField;
    [SerializeField]
    private Text valuesTextField;
    [SerializeField]
    private Button connectButton;
    [SerializeField]
    private Button disconnectButton;
    [SerializeField]
    private string ApiKey;
    [SerializeField]
    private NeuosStreamClient neuosStreamClient;
    
    
    StringBuilder builder = new StringBuilder();
    private Dictionary<string, string> fields = new Dictionary<string, string>();

    public void ConnectToServer()
    {
        if (!neuosStreamClient.IsConnected)
        {
            neuosStreamClient.ApiKey = ApiKey;
            neuosStreamClient.ConnectToServer(ipField.text, int.Parse(portField.text));
        }
    }
    public void DisconnectFromServer()
    {
        if (neuosStreamClient.IsConnected)
        {
            neuosStreamClient.Disconnect();
        }
    }

    public void OnServerConnected()
    {
        connectButton.gameObject.SetActive(false);
        disconnectButton.gameObject.SetActive(true);
    }

    public void OnServerDisconnected()
    {
        connectButton.gameObject.SetActive(true);
        disconnectButton.gameObject.SetActive(false);
    }

    public void OnValueChanged(string key, float value)
    {
        fields[key] = value.ToString();
        updateUI();
    }

    public void OnHeadbandConnectionChange(int prev, int curr)
    {

    }

    public void OnQAMessage(bool passed, int reason)
    {

    }

    public void OnError(string message)
    {

    }

    private void updateUI()
    {
        builder.Clear();
        foreach (var kvp in fields)
        {
            builder.AppendLine($"{kvp.Key} : {kvp.Value}");
        }
        valuesTextField.text = builder.ToString();
    }

    
}

