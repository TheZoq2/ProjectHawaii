using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Messages;


public class Server : MonoBehaviour {

    bool isAtStartup = false;

    public void SetupServer()
    {
        NetworkServer.RegisterHandler(888, On888);
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

    void On888(NetworkMessage msg) {
        var sentMessage = msg.ReadMessage<TestMessage>();
        Debug.Log("got test message" + sentMessage.message);
    }

    List<ComponentMessage> GenerateSequence() {
        
    }
}
