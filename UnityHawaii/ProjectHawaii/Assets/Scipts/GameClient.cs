using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Messages;
using System.Linq;
using UnityEngine.UI;
using Component = Messages.Component;

public class GameClient : MonoBehaviour
{
    NetworkClient client;

    bool isConnected = false;

    public TableControlsManager tableControlManager;
    public GameObject _sequencePanelPrefab;
    public GameObject _warningImagePrefab;

    public Sprite CycloneSprite;
    public Sprite EarthquakeSprite;
    public Sprite MissileSprite;
    public Sprite VolcanoSprite;

    private Dictionary<DisasterType, Sprite> _spriteDictionary;

    public string url;
    public int port;
    private int id;

    //Testing
    //private SequencePanelScript sps = null;

    // Use this for initialization
    void Start()
    {
        SetupClient();

        _spriteDictionary = new Dictionary<DisasterType, Sprite>
        {
            { DisasterType.Earthquake,EarthquakeSprite},
            { DisasterType.Tornado,CycloneSprite},
            { DisasterType.Missle,MissileSprite},
            { DisasterType.Vulcano,VolcanoSprite}
        };
        EventManager.OnSequenceItemCompleted += ComponentComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")) {
            print("Товарищ, спускайся");
            ComponentComplete();
        }
        //Testing
        //---------------------//
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    SetSequence(new Sequence(5, DisasterType.Earthquake, 1999,
        //        new ComponentState(Component.Scroll, 50),
        //        new ComponentState(Component.Sliders, 0, 30, 100),
        //        new ComponentState(Component.Wheel, 90),
        //        new ComponentState(Component.Lever, 3)
        //        ));
        //}

        //if (Input.GetKeyDown(KeyCode.E) && sps != null) sps.PopPanel();
    }

    public void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MessageType.SequenceStart, OnSequenceStart);
        client.RegisterHandler(MessageType.NotifyComponentComplete, OnNotifyComponentComplete);
        client.Connect(url, port);
    }

    public void OnConnected(NetworkMessage rMessage)
    {
        Debug.Log("Connected to Server");

        isConnected = true;
    }

    void OnMouseDown()
    {
        Debug.Log("Starting client");

        if (!isConnected)
        {
            SetupClient();
        }
    }

     void OnSequenceStart(NetworkMessage msg)
    {
        var sequence = msg.ReadMessage<Sequence>();
        SetSequence(sequence);
    }

    void SetSequence(Sequence sequence)
    {

        // Check if this sequence should be shown or handled on this client
        if (sequence.shouldHandle)
        {
            //только для чтения, товарищ
            print("Got sequence to handle");
            TableControlsManager.SupplyExecutionSequence(sequence);
        }
        else
        {
            print("Got sequence to display");

            // Add a sequence panel to the holder and a image to the image holder, make sure that these can later be accessed to be deleted/modified
            GameObject holder = GameObject.Find("SequencePanelsHolder");
            GameObject panel = Instantiate(_sequencePanelPrefab, holder.transform);
            panel.name = "CorrectSequencePanel";
            var script = panel.GetComponent<SequencePanelScript>();

            script.SetSequenceAndId(sequence, Statics.Panels.Count);
            //Testing
            //sps = script;
            //Testing

            holder = GameObject.Find("WarningPanel");
            GameObject warningImage = Instantiate(_warningImagePrefab, holder.transform);

            warningImage.GetComponent<Image>().sprite = _spriteDictionary[sequence.disaster];
            warningImage.GetComponent<Image>().color = new Color32(0, 0, 0, 255);

            script.WarningImage = warningImage;
            Statics.Panels.Add(panel);

            TableControlsManager.SupplyCommunicationSequence(sequence);
        }
        // Debug.Log("Disaster type: " + sequence.disaster.ToString());
        // Debug.Log("Components: " + sequence.components.Length.ToString());
    }

    void ComponentComplete() {
        client.Send(MessageType.ComponentComplete, new ComponentComplete());
    }

    void OnNotifyComponentComplete(NetworkMessage msg) {
        //TODO Fix
        print("Got component complete notification");
        GameObject.Find("CorrectSequencePanel").GetComponent<SequencePanelScript>().PopPanel();
    }
}
