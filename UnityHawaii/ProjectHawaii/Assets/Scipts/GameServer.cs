using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Messages;


public class GameServer : MonoBehaviour {

    bool isAtStartup = false;

    public void SetupServer() {
        NetworkServer.RegisterHandler(MessageType.SequenceComplete, OnSequenceComplete);
        NetworkServer.Listen(4444);
    }


    // Use this for initialization
    void Start () {
        //SetupServer();

    }

    // Update is called once per frame
    void Update () {

    }

    void OnMouseDown() {
        Debug.Log("Server starting");
        SetupServer();
    }

    void OnSequenceComplete(NetworkMessage msg) {
        var sentMessage = msg.ReadMessage<SequenceComplete>();
        Debug.Log("Got sequence complete message: " + sentMessage.correct);
    }
}
