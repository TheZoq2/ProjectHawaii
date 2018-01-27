using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Messages;
using System.Timers;


public class GameServer : MonoBehaviour {

    bool isAtStartup = false;

    public void SetupServer() {
        NetworkServer.RegisterHandler(MessageType.SequenceComplete, OnSequenceComplete);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnect);
        NetworkServer.Listen(2000);

        // var t = new Timer();
        // t.Elapsed += new ElapsedEventHandler(OnTimer);
        // t.Interval = 5000;
        // t.Start();

        StartCoroutine("TimerCoRoutine");
    }

    private IEnumerator TimerCoRoutine() {
        while(true) {
            yield return new WaitForSeconds(5);
            OnTimer();
        }
    }


    void OnTimer() {
        sendSequence();
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

    void OnSequenceComplete(NetworkMessage msg) {
        var sentMessage = msg.ReadMessage<SequenceComplete>();
        Debug.Log("Got sequence complete message: " + sentMessage.correct);
    }

    void OnClientConnect(NetworkMessage msg) {
        Debug.Log("Client connected");
    }

    void sendSequence() {
        var sequence = generateSequence();
        NetworkServer.SendToAll(MessageType.SequenceStart, sequence);
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
            timer = 0
         };
    }

    ComponentState generate_component_state() {
        var component = (Messages.Component)Random.Range(0, (int)Messages.Component.Total);
        var targets = new List<int>();
        switch (component)
        {
            case Messages.Component.Lever:
                targets = lever_targets();
                break;
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

    List<int> lever_targets() {
        var result = list_of_four();
        result[0] = Random.Range(0,4);
        return result;
    }
    List<int> wheel_targets() {
        var result = list_of_four();
        result[0] = Random.Range(0,360);
        return result;
    }
    List<int> switch_targets() {
        return new List<int>(){
            Random.Range(0,1),
            Random.Range(0,1),
            Random.Range(0,1),
            Random.Range(0,1)
        };
    }
    List<int> scroll_targets() {
        var result = list_of_four();
        result[0] = Random.Range(0,100);
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
