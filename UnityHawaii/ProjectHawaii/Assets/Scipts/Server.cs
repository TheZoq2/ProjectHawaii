using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Server : MonoBehaviour {

    NetworkClient client;
    bool isAtStartup = false;

    public void SetupServer()
    {
        NetworkServer.Listen(4444);
    }

    public void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
    }

    public void OnConnected(NetworkMessage rMessage)
    {
        Debug.Log("Connected to Server");
    }

	// Use this for initialization
	void Start () {
        SetupServer();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
