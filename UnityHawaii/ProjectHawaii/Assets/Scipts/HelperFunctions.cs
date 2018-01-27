using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static string ParseColorToString(Color color)
    {
        switch (color.ToString())
        {
            case "RGBA(1.000, 0.000, 0.000, 1.000)":
                return "Red";
            case "RGBA(0.000, 1.000, 0.000, 1.000)":
                return "Green";
            case "RGBA(0.000, 0.000, 1.000, 1.000)":
                return "Blue";
            case "RGBA(1.000, 0.922, 0.016, 1.000)":
                return "Yellow";
        }
        return "";
    }
}
