using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LedScript : MonoBehaviour {

    public Sprite LedOff;
    public Sprite LedOn;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = LedOff;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LightUp()
    {
        GetComponent<Image>().sprite = LedOn;
    }

    public void LightOff()
    {
        GetComponent<Image>().sprite = LedOff;
    }
}
