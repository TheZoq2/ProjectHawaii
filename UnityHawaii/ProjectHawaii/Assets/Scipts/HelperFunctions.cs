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
        if (color == Color.blue)
            return "Blue";
        if (color == Color.green)
            return "Green";
        if (color == Color.yellow)
            return "Yellow";
        if (color == Color.red)
            return "Red";

        return "";
    }
}
