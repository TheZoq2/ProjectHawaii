using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Emergency_Codes : NetworkBehaviour {

    private List<int> sequence;

    // Use this for initialization
    void Start () {
        // Sequance 1
        sequence.Add(0);  // Vulcano
        sequence.Add(1);  // # sequences of (1)
        sequence.Add(3);  // 3 (Switches)
        sequence.Add(1);  // 1 (First Switch)
        sequence.Add(1);  // 1 (true)
    }
	
	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer)
        {
            return;
        }
		
	}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Client logic for this player

    }


}
