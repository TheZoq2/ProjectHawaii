using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Messages;
using System.Linq;

public class GameClient : MonoBehaviour {
    NetworkClient client;

    bool isConnected = false;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    public void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MessageType.SequenceStart, OnSequenceStart);
        client.Connect("localhost", 2000);
    }

    public void OnConnected(NetworkMessage rMessage)
    {
        Debug.Log("Connected to Server");

        SendCompleteMessage();

        isConnected = true;
    }

    void SendCompleteMessage() {
        var message = new SequenceComplete();
        message.correct = true;
        message.index = 5;

        client.Send(MessageType.SequenceComplete, message);
    }

    void OnMouseDown() {
        Debug.Log("Starting client");

        if(!isConnected) {
            SetupClient();
        }
    }

    void OnSequenceStart(NetworkMessage msg) {
        var sequence = msg.ReadMessage<Sequence>();
        Debug.Log("Disaster type: " + sequence.disaster.ToString());
        Debug.Log("Components: " + sequence.components.Length.ToString());
    }
}
