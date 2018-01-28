using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    public delegate void SequenceItemCompletedDelegate();
    public static event SequenceItemCompletedDelegate OnSequenceItemCompleted;

    public delegate void SequenceItemChangedDelegate(SequenceWithQueue s);
    public static event SequenceItemChangedDelegate OnSequenceItemChanged;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void CalamitiesChanged()
    {
        OnClicked?.Invoke();
    }

    public static void SequenceItemCompleted()
    {
        OnSequenceItemCompleted?.Invoke();
    }

    public static void SequenceItemHasChanged(SequenceWithQueue s)
    {
        OnSequenceItemChanged?.Invoke(s);
    }
}
