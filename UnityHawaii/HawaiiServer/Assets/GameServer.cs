using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Messages;
using System.Timers;
using System.Linq;


public class GameServer : MonoBehaviour {

    bool isAtStartup = false;

    List<int> clients = new List<int>();
    Dictionary<int, int> sequenceLengths = new Dictionary<int, int>();

    public void SetupServer() {
        NetworkServer.RegisterHandler(MessageType.ComponentComplete, OnComponentComplete);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnect);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
        NetworkServer.Listen(2000);
    }


    // Use this for initialization
    void Start () {
        SetupServer();
    }

    // Update is called once per frame
    void Update () {
        
    }

    void OnMouseDown() {
        Debug.Log("Server starting");
        SetupServer();
    }

    void OnComponentComplete(NetworkMessage msg) {
        print("Got component complete message");
        var connectionId = msg.conn.connectionId;

        if(sequenceLengths[connectionId] <= 1) {
            sendNewSequenceToAllClients();
        }
        else {
            sequenceLengths[connectionId]--;
            var otherClient = clients.Where((x) => connectionId != x).ToList()[0];
            print("Other client " + otherClient);
            NetworkServer.SendToClient(
                otherClient,
                MessageType.NotifyComponentComplete,
                new NotifyComponentComplete()
            );
        }
    }

    void OnClientConnect(NetworkMessage msg) {
        Debug.Log("Client connected");

        var connectionId = msg.conn.connectionId;

        clients.Add(connectionId);

        sendNewSequenceToAllClients();
    }

    void sendNewSequenceToAllClients() {
        if(clients.Count == 2) {
            sendSequence(clients[0], generateSequence());
            sendSequence(clients[1], generateSequence());
        }
    }

    void OnClientDisconnect(NetworkMessage msg) {
        Debug.Log("Client disconnected");

        var connectionId = msg.conn.connectionId;
        clients.Where((x) => connectionId != x);
    }

    void sendSequence(int handlingClientId, Sequence sequence) {
        sequenceLengths[handlingClientId] = sequence.components.Length;

        foreach(var client in clients) {
            if(client == handlingClientId) {
                sequence.shouldHandle = true;
            }
            else {
                sequence.shouldHandle = false;
            }
            print("Client: " + client + " handlingId: " + handlingClientId + " shouldHandle" + sequence.shouldHandle);

            NetworkServer.SendToClient(client, MessageType.SequenceStart, sequence);
        }
    }

     Sequence generateSequence() {
         var length = Random.Range(4, 10);
         var components = new List<ComponentState>();
         for(int i = 0; i < length; i++) {
            components.Add(generate_component_state());
         }

         return new Sequence(){
            index = Random.Range(0,1000000),
            disaster = (DisasterType) Random.Range(0, (int) DisasterType.Total),
            components = components.ToArray(),
            timer = Random.Range(5,10),
         };
    }

    ComponentState generate_component_state() {
        var component = (Messages.Component)Random.Range(0, (int)Messages.Component.Total);
        var targets = new List<int>();
        switch (component)
        {
            case Messages.Component.Wheel:
                targets = wheel_targets();
                break;
            case Messages.Component.Switches:
                targets = switch_targets();
                break;
            case Messages.Component.Scroll:
                targets = scroll_targets();
                break;
            case Messages.Component.Sliders:
                targets = slider_targets();
                break;
            default:
                break;
        }
        var result = new ComponentState();
        result.targets = targets.ToArray();
        result.component = component;
        return result;
    }

    List<int> wheel_targets() {
        var result = list_of_four();
        result[0] = Random.Range(0,360);
        return result;
    }
    List<int> switch_targets() {
        return new List<int>(){
            Random.Range(0,2),
            Random.Range(0,2),
            Random.Range(0,2),
            Random.Range(0,2)
        };
    }
    List<int> scroll_targets() {
        var result = list_of_four();
        result[0] = Random.Range(1,100);
        return result;
    }
    List<int> slider_targets() {
        var result = list_of_four();
        result[0] = Random.Range(0,100);
        result[1] = Random.Range(0,100);
        result[2] = Random.Range(0,100);
        return result;
    }

    List<int> list_of_four() {
        return new List<int>(){0, 0, 0, 0};
    }
}
