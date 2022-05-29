using io.neuos;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Game controller behaviour
/// This class connectes between the "game" and the neuos client
/// Though a very basic implementation it demonstrates how to activate and use the 
/// NeuosClient and the information is provides.
/// This class recieves data from the NeuosClient via its serialized events
/// which are linked in the scene with the inspector.
/// See the Neuos Client / Game Controller GameObjecs in the Demo scene
/// </summary>
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
        UpdateUI();
    }

    public void OnHeadbandConnectionChange(int prev, int curr)
    {
        // values defined in NeuosStreamConstants.ConnectionState
        fields["HeadbandConnection"] = $"Current : {curr} Previous : {prev}";
        UpdateUI();
    }

    public void OnQAMessage(bool passed, int reason)
    {
        // reasons defined in NeuosStreamConstants.QAFailureType
        fields["QA"] = $"Passed : {passed} Reason : {reason}";
        UpdateUI();
    }

    public void OnError(string message)
    {
        fields["Last error"] = message;
        UpdateUI();
    }

    private void UpdateUI()
    {
        builder.Clear();
        foreach (var kvp in fields)
        {
            builder.AppendLine($"{kvp.Key} : {kvp.Value}");
        }
        valuesTextField.text = builder.ToString();
    }

    
}

