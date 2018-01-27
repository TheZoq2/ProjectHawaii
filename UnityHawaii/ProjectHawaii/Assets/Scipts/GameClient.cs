using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Messages;

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
        client.Connect("localhost", 4444);
    }

    public void OnConnected(NetworkMessage rMessage)
    {
        Debug.Log("Connected to Server");

        var message = new SequenceComplete();
        message.correct = true;
        message.index = 5;

        client.Send(MessageType.SequenceComplete, message);

        isConnected = true;
    }

    void OnMouseDown() {
        Debug.Log("Starting client");

        if(!isConnected) {
            SetupClient();
        }
    }
}
