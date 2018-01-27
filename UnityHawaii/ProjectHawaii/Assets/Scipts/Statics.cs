using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Statics
{
    private static List<GameObject> _warnings;
    public static List<GameObject> Warnings => _warnings ?? (_warnings = new List<GameObject>());

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
