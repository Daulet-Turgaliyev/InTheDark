using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Realtime;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using System;
using TMPro;
using ExitGames.Client.Photon;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

public class ChatSystem : MonoBehaviour, IChatClientListener
{
    public string UserName = "Freeman";

    private string _oldTextChat;
    private string _newTextChat;

    public TextMeshProUGUI TextChat;

    private string selectedChannelName; // mainly used for GUI/input
    public ChatClient chatClient;

    protected internal ChatAppSettings chatAppSettings;

    public InputField InputFieldChat;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (string.IsNullOrEmpty(this.UserName))
        {
            this.UserName = "user" + Environment.TickCount % 99; //made-up username
        }

#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
#endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        if (!appIdPresent)
        {
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
        }
        else
        {
            Connect();
        }

    }


    public void Connect()
    {

        this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        this.chatClient.AuthValues = new AuthenticationValues(this.UserName);
        this.chatClient.ConnectUsingSettings(this.chatAppSettings);

        Debug.Log("Connecting as: " + this.UserName);
    }

    public void OnClickSend()
    {
        if (InputFieldChat != null)
        {
            TextChat.text += UserName + ": " + InputFieldChat.text + "\n";
            SendChatMessage(InputFieldChat.text);

            this.InputFieldChat.text = "";
        }
    }

    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        Debug.Log("Send: " + inputLine);
        chatClient.PublishMessage("channelA", inputLine);

    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new NotImplementedException();
    }

    public void OnConnected()
    {
        throw new NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log(string.Format("{0}{1}={2}, ", msgs, senders[i], messages[i]));
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }
}
