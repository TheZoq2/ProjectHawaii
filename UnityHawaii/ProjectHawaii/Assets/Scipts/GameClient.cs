using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Messages;
using System.Linq;

public class GameClient : MonoBehaviour {
    NetworkClient client;

    bool isConnected = false;

    public TableControlsManager tableControlManager;
    public string url;
    public int port;
    private int id;

    // Use this for initialization
    void Start () {
        SetupClient();
    }

    // Update is called once per frame
    void Update () {
    }

    public void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MessageType.SequenceStart, OnSequenceStart);
        client.RegisterHandler(MessageType.NewClientMessage, OnNewClientMessage);
        client.Connect(url, port);
    }

    public void OnConnected(NetworkMessage rMessage)
    {
        Debug.Log("Connected to Server");

        isConnected = true;
    }

    void OnMouseDown() {
        Debug.Log("Starting client");

        if(!isConnected) {
            SetupClient();
        }
    }

    void OnSequenceStart(NetworkMessage msg) {
        var sequence = msg.ReadMessage<Sequence>();

        // Check if this sequence should be shown or handled on this client
        if(sequence.index % 2 == id % 2) {
            print("Got sequence to handle");
            tableControlManager.SupplySequence(sequence);
        }
        else {
            print("Got sequence to display");
        }
        // Debug.Log("Disaster type: " + sequence.disaster.ToString());
        // Debug.Log("Components: " + sequence.components.Length.ToString());
    }

    void OnNewClientMessage(NetworkMessage msg) {
        this.id = msg.ReadMessage<NewClientMessage>().id;
        print("got id: " + id);
    }
}
